using DotNet5Crud.Models;
using System.Collections.Generic;

namespace Milestone.Models
{
    public class ViewModel
    {
        public AudioFile audiofile { get; set; }
        public List<Comment> comments { get; set; }

        public int userID { get; set; }

    }
}
