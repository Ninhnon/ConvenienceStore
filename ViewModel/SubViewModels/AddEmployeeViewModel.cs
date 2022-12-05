using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views.Admin.SubViews;
using System;
using System.IO.Packaging;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace ConvenienceStore.ViewModel.SubViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public AddEmployeeViewModel()
        {
            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
            SaveCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Save(parameter));
            UploadImageCommand=new RelayCommand<AddEmployeeView>(parameter => true, parameter => UploadImage(parameter));
        }


        public void Save(AddEmployeeView parameter)
        {
            bool isValid = true;
            parameter.nameTxtbox.ErrorMessage.Text = "";
            parameter.emailTxtbox.ErrorMessage.Text = "";
            parameter.phoneTxtbox.ErrorMessage.Text = "";
            parameter.addressTxtbox.ErrorMessage.Text = "";
            parameter.usernameTxtbox.ErrorMessage.Text = "";
            parameter.passwordTxtbox.ErrorMessage.Text = "";
            parameter.confirmPasswordTxtbox.ErrorMessage.Text = "";
            
            if (string.IsNullOrEmpty(parameter.nameTxtbox.textBox.Text))
            {
                parameter.nameTxtbox.ErrorMessage.Text = "Xin nhập họ tên";
                isValid = false;
            }
            else
            {
                if(parameter.nameTxtbox.textBox.Text.Any(char.IsDigit))
                {
                    parameter.nameTxtbox.ErrorMessage.Text = "Tên không hợp lệ";
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(parameter.emailTxtbox.textBox.Text))
            {
                parameter.emailTxtbox.ErrorMessage.Text = "Xin nhập email";
                isValid = false;
            }
            if (string.IsNullOrEmpty(parameter.phoneTxtbox.textBox.Text))
            {
                parameter.phoneTxtbox.ErrorMessage.Text = "Xin nhập số điện thoại";
                isValid = false;
            }
            else
            {
                if(!parameter.phoneTxtbox.textBox.Text.All(char.IsDigit))
                {
                    parameter.phoneTxtbox.ErrorMessage.Text = "Số điện thoại không hợp lệ";
                }
            }
            if (string.IsNullOrEmpty(parameter.addressTxtbox.textBox.Text))
            {
                parameter.addressTxtbox.ErrorMessage.Text = "Xin nhập Địa Chỉ";
                isValid = false;
            }
            if(string.IsNullOrEmpty(parameter.usernameTxtbox.textBox.Text))
            {
                parameter.usernameTxtbox.ErrorMessage.Text = "Xin nhập tên đăng nhập";
                isValid = false;
            }
            if(string.IsNullOrEmpty(parameter.passwordTxtbox.passwordBox.Password))
            {
                parameter.passwordTxtbox.ErrorMessage.Text = "Xin nhập mật khẩu";
                isValid = false;
            }
            if(string.IsNullOrEmpty(parameter.confirmPasswordTxtbox.passwordBox.Password))
            {
                parameter.confirmPasswordTxtbox.ErrorMessage.Text = "Xin xác nhận mật khẩu";
                isValid = false;
            }
            else
            {
                if(parameter.confirmPasswordTxtbox.passwordBox.Password !=parameter.passwordTxtbox.passwordBox.Password)
                {
                    parameter.confirmPasswordTxtbox.ErrorMessage.Text = "Mật khẩu xác nhận không đúng, thử lại";
                    isValid = false;
                }    
            }
            if (isValid)
            {
                Account acc = new Account("0",
                                   parameter.nameTxtbox.textBox.Text.ToString(),
                                   parameter.addressTxtbox.textBox.Text.ToString(),
                                   parameter.phoneTxtbox.textBox.Text.ToString(),
                                   parameter.emailTxtbox.textBox.Text.ToString(),
                                   parameter.usernameTxtbox.textBox.Text.ToString(),
                                   parameter.passwordTxtbox.passwordBox.Password.ToString());
                AccountDAL.Instance.AddIntoDataBase(acc);
                MessageBox.Show("them thanh cong");
            }



            }
        public void Back(AddEmployeeView parameter)
        {
            parameter.Close();
        }
       
        public void UploadImage(AddEmployeeView parameter)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Image files|*.jpeg;*.jpg;*.png";
            openDialog.FilterIndex = -1;

            BitmapImage bi = new BitmapImage();
          
                

            if (openDialog.ShowDialog()== DialogResult.OK)
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
    }
}
