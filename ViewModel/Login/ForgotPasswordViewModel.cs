using ConvenienceStore.Utils.DataLayerAccess;
using ConvenienceStore.ViewModel.Admin;
using ConvenienceStore.Views;
using ConvenienceStore.Views.Login;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Windows.Input;

namespace ConvenienceStore.ViewModel.Login
{
    public class ForgotPasswordViewModel : BaseViewModel
    {
        private string email;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private int authen;
        public int Authencode
        {
            get { return authen; }
            set { authen = value; }
        }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand BackAuthenCommand { get; set; }
        public ICommand AuthenCodeCommand { get; set; }
        public ICommand NewPasswordCommand { get; set; }
        public ICommand NewBackCommand { get; set; }
        public ForgotPasswordViewModel()
        {
            BackCommand = new RelayCommand<ForgotPasswordWindow>(parameter => true, parameter => Back(parameter));
            ForgotPasswordCommand = new RelayCommand<ForgotPasswordWindow>(parameter => true, parameter => Forgot(parameter));
            BackAuthenCommand = new RelayCommand<AuthenCodeWindow>(parameter => true, parameter => BackAuthen(parameter));
            AuthenCodeCommand = new RelayCommand<AuthenCodeWindow>(parameter => true, parameter => Authen(parameter));
            NewBackCommand = new RelayCommand<NewPasswordWindow>(parameter => true, parameter => NewBack(parameter));
            NewPasswordCommand = new RelayCommand<NewPasswordWindow>(parameter => true, parameter => NewPass(parameter));
        }
        public void Forgot(ForgotPasswordWindow parameter)
        {
            if (string.IsNullOrEmpty(parameter.textEmail.textBox.Text.ToString()))
            {
                MessageBoxCustom mb = new("Cảnh báo", "Hãy nhập email!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
            else
            {
                Email = parameter.textEmail.textBox.Text.ToString();
                Random rnd = new Random();
                Authencode = rnd.Next(100000, 999999);

                string cs = @ConfigurationManager.ConnectionStrings["Default"].ToString();
                string query = "select* from Users where Email=" + "\'" + parameter.textEmail.textBox.Text.ToString() + "\'";

                SqlConnection con = new SqlConnection(cs);
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows == true)
                {
                    reader.Read();
                    string email = reader["Email"].ToString();


                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.EnableSsl = true;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential("minhku031103@gmail.com", "ytgealdlxalxznzb");
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("minhku031103@gmail.com");
                    mail.To.Add(email);
                    mail.Subject = "Your Authentication Code";
                    mail.Body = "Mã xác minh của bạn là: " + Authencode;
                    client.Send(mail);
                    client.Dispose();
                    AuthenCodeWindow authen = new AuthenCodeWindow();
                    reader.Close();
                    con.Close();

                    authen.ShowDialog();
                    parameter.Close();






                }
                else
                {
                    MessageBoxCustom mb = new("Cảnh báo", "Email không tồn tại!", MessageType.Warning, MessageButtons.OK);
                    mb.ShowDialog();
                }

            }
        }
        public void Back(ForgotPasswordWindow parameter)
        {
            parameter.Close();
        }
        public void BackAuthen(AuthenCodeWindow parameter)
        { parameter.Close(); }
        public void NewBack(NewPasswordWindow parameter)
        {
            parameter.Close();
        }

        public void Authen(AuthenCodeWindow parameter)
        {
            parameter.textAuthen.ErrorMessage.Text = "";
            if (string.IsNullOrEmpty(parameter.textAuthen.textBox.Text.ToString()))
            {
                parameter.textAuthen.ErrorMessage.Text = "Hãy nhập mã xác nhận";
                parameter.textAuthen.Focus();

            }
            else if (!int.TryParse(parameter.textAuthen.textBox.Text.ToString(), out int n))
            {
                parameter.textAuthen.ErrorMessage.Text = "Hãy nhập mã xác nhận chỉ gồm 6 chữ số";
                parameter.textAuthen.Focus();
            }
            else if (int.Parse(parameter.textAuthen.textBox.Text) == Authencode)
            {


                NewPasswordWindow newpass = new NewPasswordWindow();

                newpass.ShowDialog();
                parameter.Close();



            }
            else
            {
                MessageBoxCustom mb = new("Cảnh báo", "Mã xác nhận không chính xác!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }
        }




        public void NewPass(NewPasswordWindow parameter)
        {
            parameter.newpass.ErrorMessage.Text = "";
            parameter.confirmpass.ErrorMessage.Text = "";
            if (string.IsNullOrEmpty(parameter.newpass.passwordBox.Password.ToString()))
            {
                parameter.newpass.ErrorMessage.Text = "Hãy nhập mật khẩu mới";
                parameter.newpass.Focus();
            }
            else if (string.IsNullOrEmpty(parameter.confirmpass.passwordBox.Password.ToString()))
            {
                parameter.confirmpass.ErrorMessage.Text = "Hãy xác nhận mật khẩu mới";
                parameter.confirmpass.Focus();
            }
            else if ((parameter.newpass.passwordBox.Password.ToString()) == (parameter.confirmpass.passwordBox.Password.ToString()))
            {
                string pass = MD5Hash(MD5Hash(parameter.newpass.passwordBox.Password.ToString()));
                string mail = Email;
                AccountDAL.Instance.UpdatePassword(pass, mail);

                MessageBoxCustom mb = new("Thành công", "Đã thay đổi mật khẩu mới", MessageType.Success, MessageButtons.OK);
                mb.ShowDialog();
                parameter.Close();



            }
            else
            {


                MessageBoxCustom mb = new("Cảnh báo", "Mật khẩu xác nhận không chính xác!", MessageType.Warning, MessageButtons.OK);
                mb.ShowDialog();
            }


        }




    }






}

