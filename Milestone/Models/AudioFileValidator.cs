using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet5Crud.Models
{

    /*
     * Alex Vergara
     * 4/24/2022
     * cst-451
     * Capstone
     * 
     * This the audio file validator for our audio file model that verifies data was inputted correctly
     */
    public class AudioFileValidator
    {
        // properties

        [MaxLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }
    }

    [ModelMetadataType(typeof(AudioFileValidator))]
    public partial class AudioFile
    {
    }
}
