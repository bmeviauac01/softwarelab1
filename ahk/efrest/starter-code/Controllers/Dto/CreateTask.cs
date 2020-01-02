using System.ComponentModel.DataAnnotations;

namespace api.Controllers.Dto
{
    // DO NOT CHANGE ANYTHING
    // NE VALTOZTASS MEG SEMMIT
    public class CreateTask
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
