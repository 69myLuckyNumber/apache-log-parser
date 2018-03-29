using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Models;
using ApacheLogParser.Core.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApacheLogParser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogParser logParser;
        private readonly IMapper mapper;
        private readonly IRepository repository;
        private readonly IUnitOfWork uow;

        public HomeController(ILogParser logParser, IMapper mapper, IRepository repository, IUnitOfWork uow)
        {
            this.uow = uow;
            this.repository = repository;
            this.mapper = mapper;
            this.logParser = logParser;
        }

        public async Task<IActionResult> Index()
        {
            var hosts = await repository.GetAllHostsAsync();
            return View(new JournalViewModel { Hosts = mapper.Map<List<HostViewModel>>(hosts)});
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            long size = file.Length;
            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var logEntries = await logParser.ParseAsync(filePath);

            var requests = mapper.Map<List<Request>>(logEntries);

            var removedReqs = new List<Request>();
            foreach (var req in requests)
            {
                if (repository.isRequestPresent(req.RequestorIPAddress, req.DateTimeRequested))
                    removedReqs.Add(req);
            }
            foreach (var req in removedReqs)
            {
                requests.Remove(req);
            }

            await repository.AddRequestsAsync(requests);
            await uow.CommitAsync();

            var hosts = await repository.GetAllHostsAsync();
            var model = new JournalViewModel
            {
                Hosts = mapper.Map<List<HostViewModel>>(hosts)
            };

            return View("Index", model);

        }
    }
}