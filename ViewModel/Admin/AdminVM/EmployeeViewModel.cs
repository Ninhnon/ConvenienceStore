using ConvenienceStore.Model;
using ConvenienceStore.Model.Admin;
using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.Utils.Helpers;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Admin;
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

namespace ConvenienceStore.ViewModel.Admin.AdminVM
{
    public class EmployeeViewModel : BaseViewModel
    {
        private int selectedid_;
        public int Selectedid
        { get { return selectedid_; } set { selectedid_ = value; } }
        private string selected_;
        public string Selected
        {
            get { return selected_; }
            set
            {
                selected_ = value;
                OnPropertyChanged();
            }
        }
        private static int salary;
        public static int Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        private List<string> admins = AccountDAL.Instance.ConvertDBToListString();
        public static List<Account> accounts = new List<Account>();
        public static List<SalaryBill> bills = new List<SalaryBill>();

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
        public ICommand InitManagerCommandEdit { get; set; }
        public ICommand InitManagerCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackCommandEdit { get; set; }
        public ICommand BackCommandPay { get; set; }
        public ICommand PayCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand SaveCommandEdit { get; set; }
        public ICommand UploadImageCommand { get; set; }
        public ICommand UploadImageCommandEdit { get; set; }
        public ICommand LoadCommand { get; set; }
        private string searchContent_;
        public string SearchContent
        {
            get { return searchContent_; }
            set
            {
                searchContent_ = value;
                OnPropertyChanged();
                if (SearchContent == "")
                {

                    accounts = AccountDAL.Instance.ConvertDataTableToListEmployeeAdmin();
                    ObservableEmployee = new ObservableCollection<Account>(accounts);

                }
            }
        }
        private ObservableCollection<Account> observableEmployee_;
        public ObservableCollection<Account> ObservableEmployee
        {
            get { return observableEmployee_; }
            set
            {
                observableEmployee_ = value;
                OnPropertyChanged();

            }
        }
        private ObservableCollection<SalaryBill> observableBill_;
        public ObservableCollection<SalaryBill> ObservableBill
        {
            get { return observableBill_; }
            set
            {
                observableBill_ = value;
                OnPropertyChanged();

            }
        }


        public ICommand SearchCommand { get; set; }
        public ICommand AddEmployeeCommand { get; set; }
        public ICommand DeleteEmployeeCommand { get; set; }
        public ICommand EditEmployeeCommand { get; set; }
        public ICommand PaySalaryCommand { get; set; }

        public EmployeeViewModel()
        {

            accounts = AccountDAL.Instance.ConvertDataTableToListEmployeeAdmin();
            bills = AccountDAL.Instance.LoadSalaryBill();

            ObservableEmployee = new ObservableCollection<Account>(accounts);
            ObservableBill = new ObservableCollection<SalaryBill>(bills);

            AddEmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => AddEmployee(parameter));
            SearchCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Search());
            DeleteEmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Delete(parameter));


            LoadCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Load(parameter));

            BackCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Back(parameter));
            BackCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => BackEdit(parameter));
            BackCommandPay = new RelayCommand<PaySalaryView>(parameter => true, parameter => BackPay(parameter));
            SaveCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => Save(parameter));
            SaveCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => SaveEdit(parameter));
            UploadImageCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => UploadImage(parameter));


            InitManagerCommand = new RelayCommand<AddEmployeeView>(parameter => true, parameter => AddManagerItemSource(parameter));
            InitManagerCommandEdit = new RelayCommand<EditEmployeeView>(parameter => true, parameter => AddManagerItemSourceEdit(parameter));
            EditEmployeeCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => Edit(parameter));
            PaySalaryCommand = new RelayCommand<EmployeeView>(parameter => true, parameter => OpenPay(parameter));
            PayCommand = new RelayCommand<PaySalaryView>(parameter => true, parameter => Pay(parameter));

        }
        public void Load(EmployeeView parameter)
        {
            accounts = AccountDAL.Instance.ConvertDataTableToListEmployeeAdmin();
            bills = AccountDAL.Instance.LoadSalaryBill();
            ObservableEmployee = new ObservableCollection<Account>(accounts);
            ObservableBill = new ObservableCollection<SalaryBill>(bills);
            parameter.AccountsDataGrid.ItemsSource = ObservableEmployee;
            parameter.HistoryDataGrid.ItemsSource = ObservableBill;
        }
        public void AddEmployee(EmployeeView parameter)
        {
            AddEmployeeView addView = new AddEmployeeView();
            Selected = DatabaseHelper.FetchingAccounAdminWithIdData(CurrentAccount.idAccount).Name;
            addView.ShowDialog();
            int i = 1;
            foreach (var account in accounts)
            {
                account.Number = i;
                i++;
            }

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
            int i = 1;
            foreach (var account in accounts)
            {

                if (account.IdAccount == ((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount)
                {
                    ObservableEmployee.Remove(account);
                    foreach (var e in ObservableEmployee)
                    {
                        e.Number = i;
                        i++;
                    }
                    parameter.AccountsDataGrid.Items.Refresh();
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


            int j = ((Account)parameter.AccountsDataGrid.SelectedItem).ManagerId;
            Selected = DatabaseHelper.FetchingAccounAdminWithIdData(j).Name;
            editView.salaryTxtbox.textBox.Text = AccountDAL.Instance.GetSalary(((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount).ToString();





            editView.ShowDialog();
            parameter.AccountsDataGrid.Items.Refresh();

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
            else
            if (!parameter.emailTxtbox.textBox.Text.Contains('@'))
            {
                parameter.emailTxtbox.ErrorMessage.Text = "Email không hợp lệ";
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
                          BaseViewModel.MD5Hash(BaseViewModel.MD5Hash(parameter.passwordTxtbox.passwordBox.Password.ToString()))
                           , ConvertImageToBytes(openDialog.FileName).ToArray(),
                          i
                           );
                acc.Number = accounts1[accounts1.Count - 1].Number + 1;
                acc.Salary = 0;
                AccountDAL.Instance.AddIntoDataBase(acc);
                EmployeeViewModel.accounts.Add(acc);

                MessageBoxCustom mb = new("Thông báo", "Thêm thành công", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                accounts = AccountDAL.Instance.ConvertDataTableToListEmployeeAdmin();
                ObservableEmployee = new ObservableCollection<Account>(accounts);

                parameter.Close();

            }



        }

        public void SaveEdit(EditEmployeeView parameter)
        {
            List<Account> accounts1 = AccountDAL.Instance.ConvertDataTableToList();

            parameter.ManagerErrorMessage.Text = "";
            parameter.salaryTxtbox.ErrorMessage.Text = "";


            if (parameter.ManagerCombobox.SelectedValue == null)
            {
                parameter.ManagerErrorMessage.Text = "Xin chọn người quản lý";
                parameter.ManagerCombobox.Focus();

            }
            else if (string.IsNullOrEmpty(parameter.salaryTxtbox.textBox.Text))
            {
                parameter.salaryTxtbox.ErrorMessage.Text = "Xin nhập mức lương mới";
                parameter.salaryTxtbox.textBox.Focus();
            }

            else if (!int.TryParse(parameter.salaryTxtbox.textBox.Text, out int n))
            {
                parameter.salaryTxtbox.ErrorMessage.Text = "Xin nhập mức lương là các chữ số";
                parameter.salaryTxtbox.textBox.Focus();
            }

            else if (int.TryParse(parameter.salaryTxtbox.textBox.Text, out n) && int.Parse(parameter.salaryTxtbox.textBox.Text) < 0)
            {
                parameter.salaryTxtbox.ErrorMessage.Text = "Xin nhập mức lương hợp lệ";
                parameter.salaryTxtbox.textBox.Focus();
            }
            else
            {
                int i = 0;

                foreach (var account in accounts1)
                {

                    if (account.Name == parameter.ManagerCombobox.SelectedValue.ToString())
                    {
                        i = account.IdAccount;
                    }
                }
                AccountDAL.Instance.UpdateManager(Selectedid, i);
                AccountDAL.Instance.SetNewSalary(int.Parse(parameter.salaryTxtbox.textBox.Text), Selectedid);


                MessageBoxCustom mb = new("Thông báo", "Sửa thành công", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                ObservableEmployee.Clear();
                accounts = AccountDAL.Instance.ConvertDataTableToListEmployeeAdmin();
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


        public void AddManagerItemSourceEdit(EditEmployeeView parameter)
        {


            ItemSourceManager = new ObservableCollection<string>(admins);
        }
        public void AddManagerItemSource(AddEmployeeView parameter)
        {


            ItemSourceManager = new ObservableCollection<string>(admins);
        }

        public void BackPay(PaySalaryView parameter)
        {
            parameter.Close();
        }
        public void OpenPay(EmployeeView parameter)
        {
            Selectedid = ((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount;
            int daywork = 0;

            string salaryday = AccountDAL.Instance.GetSalaryDay(((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount);
            if (string.IsNullOrEmpty(salaryday))
            {
                daywork = AccountDAL.Instance.GetWorkDayNull(((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount);
            }
            else
            {
                daywork = AccountDAL.Instance.GetWorkDay(((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount);
            }
            Salary = ((int)(((AccountDAL.Instance.GetSalary(((Account)parameter.AccountsDataGrid.SelectedItem).IdAccount)) / 30f * daywork)));
            PaySalaryView payView = new PaySalaryView();

            payView.DayWork.textBox.Text = daywork.ToString();
            payView.DayGet.textBox.Text = salaryday;

            payView.salaryTxtbox.textBox.Text = Salary.ToString();





            payView.ShowDialog();

        }
        public void Pay(PaySalaryView parameter)
        {
            if (int.Parse(parameter.salaryTxtbox.textBox.Text) <= 0)
            {
                MessageBoxCustom mb = new("Thông báo", "Lương không hợp lệ", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
            else
            {
                AccountDAL.Instance.SetSalaryDate(Selectedid);
                AccountDAL.Instance.InsertSalaryBill(Selectedid, int.Parse(parameter.salaryTxtbox.textBox.Text));

                bills = AccountDAL.Instance.LoadSalaryBill();

                ObservableBill = new ObservableCollection<SalaryBill>(bills);

                MessageBoxCustom mb = new("Thông báo", "Trả lương thành công", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                parameter.Close();
            }
        }


    }
}
