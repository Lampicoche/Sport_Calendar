using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
//Definition of the class Team with his attributes
namespace Sport_Calendar.Domain.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "The Sport field is required.")]
        [Display(Name = "Sport")]
        public int SportId { get; set; }

        public int? PlaceId { get; set; }

        [ValidateNever] public Sport? Sport { get; set; }
        [ValidateNever] public Place? Place { get; set; }
    }
}
