using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FlightManager.DataModels
{
    public class Reservation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string SecondName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid EGN.")]
        public string EGN { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(60)]
        public string Nationality { get; set; }

        [Required]
        public string TicketType { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(0,50)]
        public int TicketsCount { get; set; }

        public bool IsConfirmed { get; set; } = false;
        public virtual Flight Flight { get; set; }
        
        public string FlightId { get; set; }

    }
}
