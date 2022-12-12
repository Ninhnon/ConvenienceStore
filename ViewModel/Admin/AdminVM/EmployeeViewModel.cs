using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Views.Admin;
using ConvenienceStore.Views.Admin.SubViews;
using ConvenienceStore.Utils.Helpers;

using ConvenienceStore.Model.Staff;
using ConvenienceStore.Views;

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class EmployeeViewModel : BaseViewModel
    {
        private int selectedid_;
        public int Selectedid
        { get { return selectedid_; } set { selectedid_ = value; } }
        private string selected_;
        public string Selected {
            get { return selected_; }
            set { selected_ = value;
                OnPropertyChanged();
            }
        }
        private List<string> admins = AccountDAL.Instance.ConvertDBToListString();
        public static List<Account> accounts = new List<Account>();

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
        public ICommand InitManagerCommandEdit {get;set;}
        public ICommand InitManagerCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackCommandEdit { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveCommandEdit { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand UploadImageCommandEdit { get; set; }
        private string searchContent_;
        public string SearchContent
        {
            get { return searchContent_; }
            set { searchContent_ = value;
                OnPropertyChanged();
                if (SearchContent == "")
                    ObservableEmployee = new ObservableCollection<Account>(accounts);

            }
        }
        private ObservableCollection<Account> observableEmployee_;
        public ObservableCollection<Account> ObservableEmployee
        {
            get { return observableEmployee_; }
            set { observableEmployee_ = value;
                OnPropertyChanged();
            }
        }


        public ICommand SearchCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }

        public EmployeeViewModel()
        {
            accounts = AccountDAL.Instance.ConvertDataTableToListEmployee();
            AddEmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => AddEmployee(parameter));
            SearchCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Search());
            DeleteEmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Delete(parameter));
            ObservableEmployee = new ObservableCollection<Account>(accounts);
            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
            BackCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => BackEdit(parameter));
            SaveCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Save(parameter));
            SaveCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => SaveEdit(parameter));
            UploadImageCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => UploadImage(parameter));
            UploadImageCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => UploadImageEdit(parameter));
            ObservableEmployee = new ObservableCollection<Account>(accounts);
           InitManagerCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => AddManagerItemSource(parameter));
            InitManagerCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => AddManagerItemSourceEdit(parameter));
            EditEmployeeCommand=new RelayCommand<EmployeeView>(parameter=>true, parameter => Edit(parameter));
        
        }
        public void AddEmployee(EmployeeView parameter)
        {
            AddEmployeeView addView = new AddEmployeeView();
           
            addView.ShowDialog();
        }
        public void Search()
        {
           
            ObservableEmployee.Clear();
            for (int i = 0; i < accounts.Count; ++i)
            {
                if (accounts[i].Name.ToLower().Contains(SearchContent.ToLower()))
                {
                    ObservableEmployee.Add(accounts[i]);
                }
            }
        }
        public void Delete(EmployeeView parameter)
        {
            foreach (var account in accounts)
            {

                if(account.IdAccount== ((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount)
                {
                    ObservableEmployee.Remove(account);
                    accounts.Remove(account);
                    AccountDAL.Instance.DeleteAccount(account.IdAccount);
                    break;
                }
            }
            
        }

        public void Edit(EmployeeView parameter)
        {
            Selectedid = ((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount;
            EditEmployeeView editView = new EditEmployeeView();
            editView.nameTxtbox.textBox.Text = ((Account)parameter.AccountsDataGrid.SelectedItem).Name;
            editView.emailTxtbox.textBox.Text= ((Account)parameter.AccountsDataGrid.SelectedItem).Email;
            editView.phoneTxtbox.textBox.Text = ((Account)parameter.AccountsDataGrid.SelectedItem).Phone;
            editView.addressTxtbox.textBox.Text = ((Account)parameter.AccountsDataGrid.SelectedItem).Address;
            editView.ImageProduct.ImageSource = ConvertByteToBitmapImage(DatabaseHelper.LoadAvatar(Selectedid));

            int j = ((Account)parameter.AccountsDataGrid.SelectedItem).ManagerId;
            Selected = DatabaseHelper.FetchingAccounAdminWithIdData(j).Name;
           





            editView.ShowDialog();


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
            List<Account> accounts1 = AccountDAL.Instance.ConvertDataTableToList();
            foreach (var account in accounts1)
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

                foreach (var account in accounts1)
                {

                    if (account.Name == parameter.ManagerCombobox.SelectedValue.ToString())
                    {
                        i = account.IdAccount;
                    }
                }
                Account acc = new Account(accounts1[accounts1.Count - 1].IdAccount + 1, "0",
                           parameter.nameTxtbox.textBox.Text.ToString(),
                           parameter.addressTxtbox.textBox.Text.ToString(),
                           parameter.phoneTxtbox.textBox.Text.ToString(),
                           parameter.emailTxtbox.textBox.Text.ToString(),
                           parameter.usernameTxtbox.textBox.Text.ToString(),
                           parameter.passwordTxtbox.passwordBox.Password.ToString()
                           , ConvertImageToBytes(openDialog.FileName).ToArray(),
                          i
                           );
                acc.Number = accounts1[accounts1.Count - 1].Number + 1;
                AccountDAL.Instance.AddIntoDataBase(acc);
                EmployeeViewModel.accounts.Add(acc);

                MessageBox.Show("them thanh cong");
                ObservableEmployee = new ObservableCollection<Account>(accounts);

                parameter.Close();

            }



        }

        public void SaveEdit(EditEmployeeView parameter)
        {
            bool isValid = true;
            parameter.nameTxtbox.ErrorMessage.Text = "";
            parameter.emailTxtbox.ErrorMessage.Text = "";
            parameter.phoneTxtbox.ErrorMessage.Text = "";
            parameter.addressTxtbox.ErrorMessage.Text = "";
          
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
            List<Account> accounts1 = AccountDAL.Instance.ConvertDataTableToList();
            foreach (var account in accounts1)
            {
               
                if (parameter.phoneTxtbox.textBox.Text == account.Phone&&account.IdAccount!=Selectedid)
                {
                    parameter.phoneTxtbox.ErrorMessage.Text = "Số điện thoại đã được đăng ký";
                    isValid = false;
                    break;
                }
                if (parameter.emailTxtbox.textBox.Text == account.Email && account.IdAccount != Selectedid)
                {
                    parameter.emailTxtbox.ErrorMessage.Text = "Email đã được đăng ký";
                    isValid = false;
                    break;
                }


            }

           
            if (isValid)
            {
                int i = 0;

                foreach (var account in accounts1)
                {

                    if (account.Name == parameter.ManagerCombobox.SelectedValue.ToString())
                    {
                        i = account.IdAccount;
                    }
                }
                byte[] Avatar = new byte[byte.MaxValue];
                if (openDialog == null)
                    Avatar = DatabaseHelper.LoadAvatar(Selectedid);
                else
                Avatar = (byte[])ConvertImageToBytes(openDialog.FileName).ToArray();
                DatabaseHelper.UpdateEmployee(parameter.nameTxtbox.textBox.Text.ToString(),
                    parameter.addressTxtbox.textBox.Text.ToString(),
                    parameter.phoneTxtbox.textBox.Text.ToString(),
                    parameter.emailTxtbox.textBox.Text.ToString(),
                   Avatar,
                    i, Selectedid
                    );
                MessageBox.Show("Sua thanh cong");
                ObservableEmployee.Clear();
                accounts = AccountDAL.Instance.ConvertDataTableToListEmployee();
                ObservableEmployee = new ObservableCollection<Account>(accounts);
                parameter.Close();

            }
        }



            public void Back(AddEmployeeView parameter)
        {
            parameter.Close();
        }
        public void BackEdit(EditEmployeeView parameter)
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
        public void UploadImageEdit(EditEmployeeView parameter)
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

        public void AddManagerItemSourceEdit(EditEmployeeView parameter)
        {


            ItemSourceManager = new ObservableCollection<string>(admins);
        }
        public void AddManagerItemSource(AddEmployeeView parameter)
        {


            ItemSourceManager = new ObservableCollection<string>(admins);
        }
    }
}
