
using System.ComponentModel.DataAnnotations;

namespace ClientManagement.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime RegDate { get; set; }
    }
}

