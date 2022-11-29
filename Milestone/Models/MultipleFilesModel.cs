using DotNet5Crud.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Milestone.Models
{
    public class MultipleFilesModel
    {
        public List<IFormFile> Files { get; set; }
    }
}
