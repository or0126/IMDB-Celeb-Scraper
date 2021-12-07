using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImdbScraperApi.Models
{

    ///celebrity model with key id

    public record Celebrity
    {
        [Key]
        public int Id { get; init; }
        [DisplayName("Name")]
        public string FullName { get; init; }
        [DisplayName("Date of birth")]
        public DateTime BirthDate { get; init; }
        [DisplayName("Gender")]
        public string Gender { get; init; }
        [DisplayName("Role")]
        public string Role { get; init; }
        [DisplayName("Image")]
        public string ImageUrl { get; init; }
    }
}
