using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Milestone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace DotNet5Crud.Models
{
    /*
     * Alex Vergara
     * 4/24/2022
     * cst-451
     * Capstone
     * 
     * This is the model for our audio file
     */
    public partial class AudioFile
    {
        // properties with getters and setters
        public int AudioFileId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Key { get; set; }
        public string BPM { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }

        public string filepath { get; set; }

        public int FK_audioID { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
        [NotMapped]
        public int NumberOfLikes { get; set; }
        [NotMapped]
        public int NumberOfDislikes { get; set; }
    }
}
