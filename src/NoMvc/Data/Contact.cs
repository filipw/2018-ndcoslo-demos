using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NoMvc.Data
{
    public class Contact
    {
        public int ContactId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; }

        public string City { get; set; }
    }
}
