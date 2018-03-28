using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApacheLogParser.Core.Abstract;
using ApacheLogParser.Core.Models;
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

        public IActionResult Index()
        {
            return View();
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

            await repository.AddRequestsAsync(requests);
            await uow.CommitAsync();

            return Ok(logEntries);

        }
    }
}