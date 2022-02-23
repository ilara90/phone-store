using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public string Company { get; set; } 
        public Attachment Image { get; set; }
    }
}
