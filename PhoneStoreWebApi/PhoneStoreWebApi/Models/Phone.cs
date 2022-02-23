namespace PhoneStoreWebApi.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public Attachment Image { get; set; }
    }
}
