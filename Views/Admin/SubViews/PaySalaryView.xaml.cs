using ConvenienceStore.ViewModel.Admin.AdminVM;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConvenienceStore.Views.Admin.SubViews
{
    /// <summary>
    /// Interaction logic for PaySalaryView.xaml
    /// </summary>
    public partial class PaySalaryView : Window
    {
        private int salaryAdded;
        public int SalaryAdded
        {
            get { return salaryAdded; }
            set { salaryAdded = value; }
        }
        private int salarySubtracted;
        public int SalarySubtracted 
        {
            get { return salarySubtracted; }
            set { salarySubtracted = value; }
        }
        public PaySalaryView()
        {
            InitializeComponent();
            salaryAdded = EmployeeViewModel.Salary;
            salarySubtracted = EmployeeViewModel.Salary;
        }

        private void plusTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.plustextBox.Text))
            {
                if (string.IsNullOrEmpty(this.minustextBox.Text))
                {
                salaryAdded = EmployeeViewModel.Salary;
                    this.salaryTxtbox.textBox.Text = EmployeeViewModel.Salary.ToString();
                }
                else
                {
                    int a = int.Parse(this.minustextBox.Text);
                    int b = EmployeeViewModel.Salary; ;
                    int result = b - a;
                    this.salaryTxtbox.textBox.Text = result.ToString();
                    salarySubtracted = result;
                }
            }
               
            else
            {
                if (string.IsNullOrEmpty(this.minustextBox.Text))
                    SalarySubtracted = EmployeeViewModel.Salary;
                int a = int.Parse(this.plustextBox.Text);
                int b = SalarySubtracted;
                int result = a + b;
                this.salaryTxtbox.textBox.Text = result.ToString();
                SalaryAdded = result;
            }
        }

        private void minusTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.minustextBox.Text))
            {
                if (string.IsNullOrEmpty(this.plustextBox.Text))
                {
                    salarySubtracted = EmployeeViewModel.Salary;

                    this.salaryTxtbox.textBox.Text = EmployeeViewModel.Salary.ToString();
                }
                else
                {
                    int a = int.Parse(this.plustextBox.Text);
                    int b = EmployeeViewModel.Salary; ;
                    int result = b + a;
                    this.salaryTxtbox.textBox.Text = result.ToString();
                    salaryAdded = result;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(this.plustextBox.Text))
                    SalaryAdded = EmployeeViewModel.Salary;
                int a = int.Parse(this.minustextBox.Text);
                int b = salaryAdded;
                int result = b-a;
                this.salaryTxtbox.textBox.Text = result.ToString();
                salarySubtracted = result;
            }
        }
    }
}
