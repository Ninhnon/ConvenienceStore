using ConvenienceStore.Model;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.ViewModel.Admin.AdminVM;
using ConvenienceStore.Views.Admin.SubViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ConvenienceStore.ViewModel.SubViewModels
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        private OpenFileDialog openDialog;

        private ObservableCollection<string> itemSourceManager_;
        public ObservableCollection<string> ItemSourceManager
        {
            get { return itemSourceManager_; }
            set
            {
                itemSourceManager_ = value;
                OnPropertyChanged();
            }
        }
        public ICommand InitManagerCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public AddEmployeeViewModel()
        {
            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
            SaveCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Save(parameter));
            UploadImageCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => UploadImage(parameter));
            InitManagerCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => AddManagerItemSource(parameter));
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
            parameter.ManagerErrorMessage.Text = "";
            parameter.ImageErrorMessage.Text = "";
            if (string.IsNullOrEmpty(parameter.nameTxtbox.textBox.Text))
            {
                parameter.nameTxtbox.ErrorMessage.Text = "Xin nhập họ tên";
                isValid = false;
            }
            else
            {
                if (parameter.nameTxtbox.textBox.Text.Any(char.IsDigit))
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
                if (!parameter.phoneTxtbox.textBox.Text.All(char.IsDigit))
                {
                    parameter.phoneTxtbox.ErrorMessage.Text = "Số điện thoại không hợp lệ";
                    isValid = false;
                }
            }
            if (string.IsNullOrEmpty(parameter.addressTxtbox.textBox.Text))
            {
                parameter.addressTxtbox.ErrorMessage.Text = "Xin nhập Địa Chỉ";
                isValid = false;
            }
            if (string.IsNullOrEmpty(parameter.usernameTxtbox.textBox.Text))
            {
                parameter.usernameTxtbox.ErrorMessage.Text = "Xin nhập tên đăng nhập";
                isValid = false;
            }
            if (string.IsNullOrEmpty(parameter.passwordTxtbox.passwordBox.Password))
            {
                parameter.passwordTxtbox.ErrorMessage.Text = "Xin nhập mật khẩu";
                isValid = false;
            }
            if (string.IsNullOrEmpty(parameter.confirmPasswordTxtbox.passwordBox.Password))
            {
                parameter.confirmPasswordTxtbox.ErrorMessage.Text = "Xin xác nhận mật khẩu";
                isValid = false;
            }
            else
            {
                if (parameter.confirmPasswordTxtbox.passwordBox.Password != parameter.passwordTxtbox.passwordBox.Password)
                {
                    parameter.confirmPasswordTxtbox.ErrorMessage.Text = "Mật khẩu xác nhận không đúng, thử lại";
                    isValid = false;
                }
            }
            if (parameter.ManagerCombobox.SelectedValue == null)
            {
                parameter.ManagerErrorMessage.Text = "Xin chọn người quản lý";
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
                if (parameter.usernameTxtbox.textBox.Text == account.UserName)
                {
                    parameter.usernameTxtbox.ErrorMessage.Text = "Tên đăng nhập đã tồn tại";
                    isValid = false;
                    break;
                }
                if (parameter.phoneTxtbox.textBox.Text == account.Phone)
                {
                    parameter.phoneTxtbox.ErrorMessage.Text = "Số điện thoại đã được đăng ký";
                    isValid = false;
                    break;
                }
                if (parameter.emailTxtbox.textBox.Text == account.Email)
                {
                    parameter.emailTxtbox.ErrorMessage.Text = "Email đã được đăng ký";
                    isValid = false;
                    break;
                }


            }

            if (isValid)
            {
                int i = 0;

                foreach (var account in accounts)
                {

                    if (account.Name == parameter.ManagerCombobox.SelectedValue.ToString())
                    {
                        i = account.IdAccount;
                    }
                }
                Account acc = new Account(accounts[accounts.Count - 1].IdAccount + 1, "0",
                           parameter.nameTxtbox.textBox.Text.ToString(),
                           parameter.addressTxtbox.textBox.Text.ToString(),
                           parameter.phoneTxtbox.textBox.Text.ToString(),
                           parameter.emailTxtbox.textBox.Text.ToString(),
                           parameter.usernameTxtbox.textBox.Text.ToString(),
                           parameter.passwordTxtbox.passwordBox.Password.ToString()
                           , ConvertImageToBytes(openDialog.FileName).ToArray(),
                          i
                           );
                acc.Number = accounts[accounts.Count - 1].Number + 1;
                AccountDAL.Instance.AddIntoDataBase(acc);
                EmployeeViewModel.accounts.Add(acc);

                MessageBox.Show("them thanh cong");
            }



        }
        public void Back(AddEmployeeView parameter)
        {
            parameter.Close();
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
        public void UploadImage(AddEmployeeView parameter)
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

        public void AddManagerItemSource(AddEmployeeView parameter)
        {


            ItemSourceManager = new ObservableCollection<string>(AccountDAL.Instance.ConvertDBToListString());
        }

    }
}
