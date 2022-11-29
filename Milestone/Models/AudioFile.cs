using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Milestone.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required]
        public string Name { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string BPM { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FileName { get; set; }

        public string filepath { get; set; }
        public int FK_audioID { get; set; }
        public int likes { get; set; }

        // azure details
        public string outputassetname  { get; set; }
        public string fileformat { get; set; }
        public string jpgassetname { get; set; }

        [Required]
        [NotMapped]
        public IFormFile File { get; set; }
        [Required]
        [NotMapped]
        public IFormFile File2 { get; set; }


        [NotMapped]
        public int NumberOfLikes { get; set; }
        [NotMapped]
        public int NumberOfDislikes { get; set; }
    }
}
