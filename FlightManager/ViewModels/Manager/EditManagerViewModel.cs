using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Manager
{
    public class EditManagerViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Enter Valid EGN!")]
        public string EGN { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
    }
}
