using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin.SubViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.SubViewModels
{
    public class SettingViewModel : BaseViewModel
    {

        private OpenFileDialog openDialog;
        private byte[] anh_;
        public byte[] Anh
        {
            get { return anh_; }
            set
            {
                anh_ = value;
                OnPropertyChanged();
            }
        }
        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand ChangePassCommand { get; set; }
        public SettingViewModel()
        {
            BackCommand = new RelayCommand<Setting>(parameter => true, parameter => Back(parameter));
            Anh = DatabaseHelper.LoadAvatar(CurrentAccount.idAccount);
            SaveCommand = new RelayCommand<Setting>(parameter => true, parameter => Save(parameter));
            UploadImageCommand = new RelayCommand<Setting>(parameter => true, parameter => UploadImage(parameter));
            ChangePassCommand = new RelayCommand<Setting>(parameter => true, parameter => ChangePass(parameter));
        }




        public void Back(Setting parameter)
        {
            parameter.Close();
        }

        public void Save(Setting parameter)
        {
            bool isValid = true;
            parameter.NameErrorMessage.Text = "";
            parameter.EmailErrorMessage.Text = "";
            parameter.PhoneErrorMessage.Text = "";
            parameter.AddressErrorMessage.Text = "";

            parameter.ImageErrorMessage.Text = "";
            if (string.IsNullOrEmpty(parameter.nameTxtbox.textBox.Text))
            {
                parameter.NameErrorMessage.Text = "Xin nhập họ tên";
                isValid = false;
            }
            else
            {
                if (parameter.nameTxtbox.textBox.Text.Any(char.IsDigit))
                {
                    parameter.NameErrorMessage.Text = "Tên không hợp lệ";
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(parameter.emailTxtbox.textBox.Text))
            {
                parameter.EmailErrorMessage.Text = "Xin nhập email";
                isValid = false;
            }
            if (string.IsNullOrEmpty(parameter.phoneTxtbox.textBox.Text))
            {
                parameter.PhoneErrorMessage.Text = "Xin nhập số điện thoại";
                isValid = false;
            }
            else
            {
                if (!parameter.phoneTxtbox.textBox.Text.All(char.IsDigit))
                {
                    parameter.PhoneErrorMessage.Text = "Số điện thoại không hợp lệ";
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(parameter.addressTxtbox.textBox.Text))
            {
                parameter.AddressErrorMessage.Text = "Xin nhập Địa Chỉ";
                isValid = false;
            }

            if (parameter.ImageProduct.ImageSource == null)
            {
                parameter.ImageErrorMessage.Text = "Xin chọn ảnh";
                isValid = false;
            }
            List<Account> accounts = AccountDAL.Instance.ConvertDataTableToList();
            foreach (var account in accounts)
            {

                if (parameter.phoneTxtbox.textBox.Text == account.Phone && parameter.phoneTxtbox.textBox.Text != CurrentAccount.Phone)
                {
                    parameter.PhoneErrorMessage.Text = "Số điện thoại đã được đăng ký";
                    isValid = false;
                    break;
                }
                if (parameter.emailTxtbox.textBox.Text == account.Email && parameter.emailTxtbox.textBox.Text != CurrentAccount.Email)
                {
                    parameter.EmailErrorMessage.Text = "Email đã được đăng ký";
                    isValid = false;
                    break;
                }


            }

            if (isValid)
            {

                byte[] Avatar = new byte[byte.MaxValue];
                if (openDialog == null)
                    Avatar = DatabaseHelper.LoadAvatar(CurrentAccount.idAccount);
                else
                    Avatar = (byte[])ConvertImageToBytes(openDialog.FileName).ToArray();
                DatabaseHelper.UpdateEmployee(parameter.nameTxtbox.textBox.Text.ToString(),
                    parameter.addressTxtbox.textBox.Text.ToString(),
                    parameter.phoneTxtbox.textBox.Text.ToString(),
                    parameter.emailTxtbox.textBox.Text.ToString(),
                   Avatar,
                    CurrentAccount.ManagerId, CurrentAccount.idAccount
                    );
                CurrentAccount.Name = parameter.nameTxtbox.textBox.Text;
                CurrentAccount.Address = parameter.addressTxtbox.textBox.Text;
                CurrentAccount.Email = parameter.emailTxtbox.textBox.Text;
                CurrentAccount.Phone = parameter.phoneTxtbox.textBox.Text;
                CurrentAccount.Avatar = Avatar;
                parameter.nameTxtbox.textBox.Text = CurrentAccount.Name;
                parameter.emailTxtbox.textBox.Text = CurrentAccount.Email;
                parameter.phoneTxtbox.textBox.Text = CurrentAccount.Phone;
                parameter.addressTxtbox.textBox.Text = CurrentAccount.Address;
                parameter.ImageProduct.ImageSource = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(CurrentAccount.idAccount));
                MessageBoxCustom mb = new("Thông báo", "Sửa thành công", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();



            }



        }

        public void ChangePass(Setting parameter)
        {
            ChangePasswordView view = new ChangePasswordView();
            parameter.Hide();
            view.ShowDialog();
            parameter.Show();
        }
        public void UploadImage(Setting parameter)
        {
            openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.jpeg;*.jpg;*.png";
            openDialog.FilterIndex = -1;

            BitmapImage bi = new BitmapImage();



            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                var bytes = System.IO.File.ReadAllBytes(openDialog.FileName);
                string s = Convert.ToBase64String(bytes);

                bi.BeginInit();
                bi.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
                bi.EndInit();
            }
            var imageBrush = (ImageBrush)parameter.ImageProduct;
            try
            {
                imageBrush.ImageSource = bi;
            }
            catch
            {
                /* Chỗ này phải xài try catch để bắt lỗi
                 * Người dùng mở File Exploer nhưng không chọn ảnh mà nhấn nút "Cancle" */
            }

        }


        public Byte[] ConvertImageToBytes(string imageFileName)
        {
            FileStream fs = new FileStream(imageFileName, FileMode.Open, FileAccess.Read);

            //Initialize a byte array with size of stream
            byte[] imgByteArr = new byte[fs.Length];

            //Read data from the file stream and put into the byte array
            fs.Read(imgByteArr, 0, Convert.ToInt32(fs.Length));

            //Close a file stream
            fs.Close();
            return imgByteArr;
        }

        public BitmapImage ConvertByteToBitmapImage(Byte[] image)
        {
            BitmapImage bi = new BitmapImage();
            MemoryStream stream = new MemoryStream();
            if (image == null)
            {
                return null;
            }
            stream.Write(image, 0, image.Length);
            stream.Position = 0;
            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }



    }
}
