using Microsoft.AspNetCore.Mvc;
using PhoneStoreWebApi.Models;
using PhoneStoreWebApi.Services;

namespace PhoneStoreWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneController : ControllerBase
    {
        private FileIOServices _fileIOServices;
        private List<Phone> Phones { get; set; }
        private readonly string PATH = $"{Environment.CurrentDirectory}";

        public PhoneController()
        {
            _fileIOServices = new FileIOServices(PATH);
        }
       [HttpPost]
       [Route("AddPhone")]
        public async Task<IActionResult> AddPhone(Phone phone)
        {
            try
            {
                Phones = _fileIOServices.LoadData();
                if (phone.Image?.Data != null)
                    _fileIOServices.ByteArrayToFile(phone.Image.UniqueFileName.ToString(), phone.Image.Data);
                var lastPhone = Phones.OrderByDescending(x => x.Id).FirstOrDefault();
                phone.Id = lastPhone == null ? 0 : lastPhone.Id + 1;
                Phones.Add(phone);
                await _fileIOServices.SaveData(Phones);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("GetPhone")]
        public async Task<IActionResult> GetPhone()
        {
            try
            {                
                Phones = _fileIOServices.LoadData();
                return Ok(Phones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("DeletePhone/{phoneId}")]
        public async Task<IActionResult> DeletePhone(int phoneId)
        {
            try
            {
                Phones = _fileIOServices.LoadData();
                Phone phoneDelete = Phones.FirstOrDefault(c => c.Id == phoneId);
                if (phoneDelete?.Image != null)
                    _fileIOServices.DeleteImage(phoneDelete.Image.UniqueFileName.ToString());
                Phones.Remove(phoneDelete);
                await _fileIOServices.SaveData(Phones);
                return Ok(phoneId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Route("EditPhone")]
        public async Task<IActionResult> EditPhone(Phone phone)
        {
            try
            {
                Phones = _fileIOServices.LoadData();
                Phone phoneEdit = Phones.FirstOrDefault(c => c.Id == phone.Id);
                if (phone.Image != null)
                {
                    _fileIOServices.DeleteImage(phoneEdit.Image.UniqueFileName.ToString());
                    _fileIOServices.ByteArrayToFile(phone.Image.UniqueFileName.ToString(), phone.Image.Data);
                }
                Phones.Remove(phoneEdit);
                Phones.Insert((phone.Id - 1), phone);                
                await _fileIOServices.SaveData(Phones);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
