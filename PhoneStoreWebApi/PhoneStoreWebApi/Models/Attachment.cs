using Newtonsoft.Json;

namespace PhoneStoreWebApi.Models
{
    public class Attachment
    {
        public string FileName { get; set; }
        public Guid UniqueFileName { get; set; }
        [JsonIgnore]
        public byte [] Data { get; set; }
    }
}
