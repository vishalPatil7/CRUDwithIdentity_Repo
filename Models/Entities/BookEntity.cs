using System.ComponentModel.DataAnnotations;

namespace CRUDwithIdentity.Models.Entities
{
    public class BookEntity
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Should be greater than 1 or equal to 1")]
        public int Price { get; set; }
    }
}
