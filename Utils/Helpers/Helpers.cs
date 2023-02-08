using ConvenienceStore.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ConvenienceStore.Utils.Helpers
{
    public class Helpers
    {
        public SqlConnection connection = DatabaseHelper.sqlCon;

        public static (string, List<string>) GetListCode(int quantity, int length, string firstChars, string lastChars)
        {
            List<string> ListCode = new List<string>();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            int randomLength = length - firstChars.Length - lastChars.Length;
            if (randomLength <= 0)
            {
                return ("Độ dài của voucher phải lớn hơn độ dài chuỗi kí tự đầu + độ dài chuỗi kí tự cuối", null);
            }
            if (randomLength < 4)
            {
                return ($"Độ dài của voucher phải lớn hơn độ dài chuỗi kí tự đầu + độ dài chuỗi kí tự cuối + 4 ", null);
            }

            for (int i = 0; i < quantity; i++)
            {

                var stringChars = new char[randomLength];
                for (int j = 0; j < stringChars.Length; j++)
                {
                    stringChars[j] = chars[random.Next(chars.Length)];
                }
                string newCode = new string(stringChars);
                var isExist = ListCode.Any(code => code == newCode);
                if (isExist)
                {
                    i--;
                    continue;
                }
                ListCode.Add(firstChars + newCode + lastChars);
            }

            return (null, ListCode);
        }

        public static string ConvertDoubleToPercentageStr(double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero).ToString("P", CultureInfo.InvariantCulture);
        }

        public static string MD5Hash(string str)
        {
            StringBuilder hash = new StringBuilder();
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(str));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("X2"));
            }
            return hash.ToString();
        }

        public static bool IsPhoneNumber(string number)
        {
            if (number is null) return false;
            return Regex.Match(number, @"(([03+[2-9]|05+[6|8|9]|07+[0|6|7|8|9]|08+[1-9]|09+[0-4|6-9]]){3})+[0-9]{7}\b").Success;
        }

        public static bool IsEmail(string email)
        {
            if (email is null) return false;
            return Regex.Match(email, @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$").Success;
        }

        public static bool IsValidAddress(string address)
        {
            if (address is null) return false;
            return Regex.Match(address, @"^[a-zA-Z0-9\p{L}\s,.'-/]{1,100}$").Success;
        }

        public static string GetHourMinutes(TimeSpan t)
        {
            return t.ToString(@"hh\:mm");
        }

        public static string GetImagePath(string imageName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\Images", $"{imageName}" /*SelectedItem.Image*/);
        }

        public static string GetEmailTemplatePath(string fileName)
        {
            return Path.Combine(Environment.CurrentDirectory, @"..\..\Resources\EmailTemplate", $"{fileName}" /*SelectedItem.Image*/);
        }

        public static string? FormatVNMoney(int money)
        {
            if (money == 0)
            {
                return "0 ₫";
            }
            return string.Format(CultureInfo.InvariantCulture,
                                "{0:#,#} ₫", money);
        }

        public static string FormatStatus(bool status)
        {
            if (status == true)
            {
                return "Đã kích hoạt";
            }
            return "Chưa kích hoạt";
        }

        public static string HSDStr(DateTime s)
        {
            DateTime dateTime = DateTime.Now;
            if (s < dateTime)
            {
                return "#FF0000";
            }
            return "#00000000";
        }

        public static string FormatDecimal(decimal n)
        {
            if (n == 0)
            {
                return "0";
            }
            return string.Format(CultureInfo.InvariantCulture,
                                "{0:#,#}", n);
        }
        public static string? GetStaffName(int StaffId)
        {
            return DatabaseHelper.GetName(StaffId);
        }
    }
}
