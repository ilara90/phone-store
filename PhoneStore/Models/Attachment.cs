using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneStore.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public Guid UniqueFileName { get; set; }
        public byte[] Data { get; set; }
    }
}
