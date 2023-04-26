using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Credentials
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public DateTime LastVisit { get; set; }
    }
}
