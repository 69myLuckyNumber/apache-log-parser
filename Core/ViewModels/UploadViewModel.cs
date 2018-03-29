using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ApacheLogParser.Core.ViewModels
{
    public class UploadViewModel
    {
        [Display(Name = "Upload log file using this form")]
        [Required(ErrorMessage = "Please, select a file")]
        public IFormFile File { get; set; }
    }
}