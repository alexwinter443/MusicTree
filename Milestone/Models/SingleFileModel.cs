using Microsoft.AspNetCore.Http;
using Milestone.Models;
using System.ComponentModel.DataAnnotations;

namespace Milestone.Models
{
    public class SingleFileModel : ResponseModel
    {
        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "please select file")]
        public IFormFile File { get; set; }
    }
}
