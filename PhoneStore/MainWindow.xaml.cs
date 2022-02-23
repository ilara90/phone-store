using Newtonsoft.Json;
using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PhoneStore
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();
        public readonly string ServerAdress = "http://localhost/api/phone/";
        private byte[] data;

        public MainWindow()
        {
            InitializeComponent();
            GetPhonesList();
        }

        private async void GetPhonesList()
        {
            string json = await client.GetStringAsync(ServerAdress + "GetPhone");
            phonesList.ItemsSource = JsonConvert.DeserializeObject<List<Phone>>(json);
        }

        private void phonesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Phone p = (Phone)phonesList.SelectedItem;
        }
        private async void  Save_Click(object sender, RoutedEventArgs e)
        {
            Phone phone = (Phone)phonesList.SelectedItem;            
            if (phone == null)
            {
                phone = new Phone();
                phone.Company = Company.Text;
                phone.Title = Title.Text;
                if(data != null)
                {
                    Attachment phoneImage = new Attachment();
                    phoneImage.Data = data;
                    phoneImage.FileName = Title.Text + ".jpeg";
                    phoneImage.UniqueFileName = Guid.NewGuid();
                    phone.Image = phoneImage;
                }                
                var content = JsonConvert.SerializeObject(phone);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                await client.PostAsync(ServerAdress + "AddPhone", httpContent);
            } 
            else
            {
                int phoneId = phone.Id;
                phone.Company = Company.Text;
                phone.Title = Title.Text;
                phone.Id = phoneId;
                if(data != null)
                {
                    Attachment phoneImage = new Attachment();
                    phoneImage.Data = data;
                    phoneImage.FileName = Title.Text + ".jpeg";
                    phoneImage.UniqueFileName = Guid.NewGuid();
                    phone.Image = phoneImage;
                }
                var content = JsonConvert.SerializeObject(phone);
                var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                await client.PostAsync(ServerAdress + "EditPhone", httpContent);
            }
            
            GetPhonesList();
            Company.Text = "";
            Title.Text = "";
            data = null;
        }
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            Phone phone = (Phone)phonesList.SelectedItem;
            await client.DeleteAsync(ServerAdress + $"DeletePhone/{phone.Id}");
            GetPhonesList();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Phone phone = (Phone)phonesList.SelectedItem;
            Company.Text = phone.Company;
            Title.Text = phone.Title;
        }
        private void Upload_Click(object sender, RoutedEventArgs e)
        {          
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                          "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                          "Portable Network Graphic (*.png)|*.png";
            var result = dlg.ShowDialog();

            if (result.Value)
            {
                BitmapImage bm =
                        new BitmapImage(new Uri(dlg.FileName));
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bm));
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    data = ms.ToArray();
                }
            }
        }
    }    
}
