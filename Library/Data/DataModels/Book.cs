using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Library.Data.Common.DataConstants;

namespace Library.Data.DataModels
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(BookAuthorMaxLength)]
        public string Author { get; set; } = string.Empty;

        [Required]
        [MaxLength(BookDescriptionMaxLength)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public decimal Rating { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        public IEnumerable<IdentityUserBook> UsersBooks { get; set; } = new List<IdentityUserBook>();
    }
}
