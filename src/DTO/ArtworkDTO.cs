using System.ComponentModel.DataAnnotations;
using Backend_Teamwork.src.Entities;
using static Backend_Teamwork.src.DTO.UserDTO;

namespace Backend_Teamwork.src.DTO
{
    public class ArtworkDTO
    {
        // create artwork
        public class ArtworkCreateDto
        {
            [
                Required(ErrorMessage = "Title shouldn't be null"),
                MinLength(6, ErrorMessage = "Title should be at at least 6 characters"),
                MaxLength(30, ErrorMessage = "Title shouldn't be more than 30 characters")
            ]
            public required string Title { get; set; }

            [
                Required(ErrorMessage = "Description shouldn't be null"),
                MinLength(30, ErrorMessage = "Description should be at at least 30 characters"),
                MaxLength(200, ErrorMessage = "Description shouldn't be more than 200 characters")
            ]
            public required string Description { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than zero.")]
            public int Quantity { get; set; }

            [Range(1.0, double.MaxValue, ErrorMessage = "Price should be greater than zero.")]
            public decimal Price { get; set; }

            [
                Required(ErrorMessage = "Url shouldn't be null"),
                MinLength(30, ErrorMessage = "Url should be at at least 30 characters"),
            ]
            public required string ImageUrl { get; set; }
            public DateTime? CreatedAt { get; set; } = DateTime.Now;

            [Required(ErrorMessage = "Category Id shouldn't be null")]
            public required Guid CategoryId { get; set; }
        }

        // update artwork
        public class ArtworkUpdateDTO
        {
            [
                Required(ErrorMessage = "Title shouldn't be null"),
                MinLength(6, ErrorMessage = "Title should be at at least 6 characters"),
                MaxLength(30, ErrorMessage = "Title shouldn't be more than 30 characters")
            ]
            public required string Title { get; set; }

            [
                Required(ErrorMessage = "Description shouldn't be null"),
                MinLength(30, ErrorMessage = "Description should be at at least 30 characters"),
                MaxLength(200, ErrorMessage = "Description shouldn't be more than 200 characters")
            ]
            public required string Description { get; set; }

            [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than zero.")]
            public int Quantity { get; set; }

            [Range(1.0, double.MaxValue, ErrorMessage = "Price should be greater than zero.")]
            public decimal Price { get; set; }

            [
                Required(ErrorMessage = "Url shouldn't be null"),
                MinLength(30, ErrorMessage = "Url should be at at least 30 characters"),
            ]
            public required string ImageUrl { get; set; }
        }

        // get artwork
        public class ArtworkReadDto
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public string ImageUrl { get; set; }
            public DateTime CreatedAt { get; set; }
            public Category Category { get; set; }
            public UserReadDto User { get; set; }
        }
    }
}
