using Newtonsoft.Json;
using PhoneStoreWebApi.Models;

namespace PhoneStoreWebApi.Services
{
    public class FileIOServices
    {
        public readonly string PATH;
        public readonly string AttachmentPath;

        public FileIOServices(string path)
        {
            PATH = path + "\\phones.json";
            AttachmentPath = path + "/Attachments/";
        }
        public List<Phone> LoadData()
        {
            var fileExists = File.Exists(PATH);
            if (!fileExists)
            {
                File.CreateText(PATH).Dispose();
                return new List<Phone>();
            }
            using (var reader = File.OpenText(PATH))
            {
                var fileText = reader.ReadToEnd();
                var phones = JsonConvert.DeserializeObject<List<Phone>>(fileText) ?? new List<Phone>();
                foreach (var item in phones)
                {
                    if (item?.Image != null && File.Exists(AttachmentPath + $"{item.Image.UniqueFileName}"))
                    {
                        item.Image.Data = File.ReadAllBytes(AttachmentPath + $"{item.Image.UniqueFileName}");
                    }
                }
                return phones;
            }
        }

        public async Task SaveData(List<Phone> phones)
        {
            using (StreamWriter writer = File.CreateText(PATH))
            {
                string output = JsonConvert.SerializeObject(phones);
                await writer.WriteAsync(output);
            }
        }

        public bool ByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (var fs = new FileStream(AttachmentPath + fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
                return true;
            }
        }

        public async Task DeleteImage(string fileName)
        {
           if (File.Exists(AttachmentPath + $"{fileName}"))
           {
                File.Delete(AttachmentPath + $"{fileName}");
           }
        }
    }
}
