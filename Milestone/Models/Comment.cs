namespace Milestone.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public int FK_AudioFileID { get; set; }


    }
}
