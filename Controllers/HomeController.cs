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

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var hosts = await repository.GetAllHostsAsync();
            var hostsModel = mapper.Map<List<HostViewModel>>(hosts);

            var model = new HomeViewModel
            {
                Journal = new JournalViewModel { Hosts = hostsModel },
                UploadForm = new UploadViewModel()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(UploadViewModel uploadModel)
        {
            var hosts = await repository.GetAllHostsAsync();
            var hostsModel = mapper.Map<List<HostViewModel>>(hosts);

            if (!ModelState.IsValid)
                return View("Index", new HomeViewModel
                {
                    Journal = new JournalViewModel { Hosts = mapper.Map<List<HostViewModel>>(hosts) },
                    UploadForm = uploadModel
                });
            var validExts = new string[] {".txt", ".log"};
            var filename = uploadModel.File.FileName;

            if(!validExts.Any(s => s.Equals(filename.Substring(filename.LastIndexOf("."))))) 
            {
                ModelState.AddModelError(string.Empty, "Select valid file");
                return View("Index", new HomeViewModel
                {
                    Journal = new JournalViewModel { Hosts = mapper.Map<List<HostViewModel>>(hosts) },
                    UploadForm = uploadModel
                });
            }
            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadModel.File.CopyToAsync(stream);
            }

            var logEntries = await logParser.ParseAsync(filePath);
            
            var requests = mapper.Map<List<Request>>(logEntries);

            //remove present requests to avoid duplication
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

            hosts = await repository.GetAllHostsAsync();
            hostsModel = mapper.Map<List<HostViewModel>>(hosts);

            var model = new HomeViewModel
            {
                Journal = new JournalViewModel { Hosts = hostsModel },
                UploadForm = new UploadViewModel()
            };

            return View("Index", model);
        }
    }
}