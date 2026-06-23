using Microsoft.Win32;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Security.Policy;
using System.Security.Cryptography;
namespace DVLD_With_MY_teatcher.Globle
{
    public class clsGlobal
    {
        public static clsUser CurrentUser = null;


        public static string ComputeHashing(string Input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] InputHashing=sha256.ComputeHash(Encoding.UTF8.GetBytes(Input));

                return BitConverter.ToString(InputHashing).Replace("-","").ToLower();
            }
        }
        public static bool RememberUserNameAndPassword(string userName, string password)
        {

            try
            {


                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                string PathFile = CurrentDirectory+"\\data.txt";

                if (userName==""&& File.Exists(PathFile))
                {
                    File.Delete(PathFile);
                    return true;
                }

                string DateToSave = userName+"#||#"+ password;
                using (StreamWriter writer = new StreamWriter(PathFile))
                {
                    writer.WriteLine(DateToSave);
                    return true;
                
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred {ex.Message}");
                return false; 
            }


            return false;
        }

        public static bool GetStoredCredential(ref string username, ref string password) 
        {
            try
            {
                string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

                string PathFile = CurrentDirectory+"\\data.txt";

                if (File.Exists(PathFile)) 
                {
                    using (StreamReader reader = new StreamReader(PathFile))
                    {
                        string line;
                        while((line = reader.ReadLine()) != null)
                        {
                            string[] date =line.Split(new string[] { "#||#" }, StringSplitOptions.None);
                           
                            username= date[0];
                            password= date[1];
                            return true;       
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred{ex.Message}");
                return false;

            }
            return false;
        }



        public static bool GetStoredCredentialInRegistry(ref string Username, ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string PasswordUserHotelManagementName = "PasswordUserDVLD";
            string UserNameHotelManagementName = "UserNameDVLD";
            bool result = false;
            try
            {

                string Value = Registry.GetValue(KeyPath, PasswordUserHotelManagementName, null)as string;
                if (!string.IsNullOrEmpty(Value))
                {
                    Password=Value;
                    result=true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

            try
            {
                string Value = Registry.GetValue(KeyPath, UserNameHotelManagementName, null)as string;

                if (!string.IsNullOrEmpty(Value))
                {
                    Username=Value;
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return false;

        }

        public static bool RememberUsernameAndPasswordInRegistry(string Username, string Password)
        {

            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string PasswordUserHotelManagementName = "PasswordUserDVLD";
            string PasswordUserHotelManagementData = Password;
            string UserNameHotelManagementName = "UserNameDVLD";
            string UserNameHotelManagementData = Username;
            bool result = false;
            try
            {
                Registry.SetValue(KeyPath, PasswordUserHotelManagementName, PasswordUserHotelManagementData, RegistryValueKind.String);
                result=true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

            try
            {
                Registry.SetValue(KeyPath, UserNameHotelManagementName, UserNameHotelManagementData, RegistryValueKind.String);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
            return false;

        }


    }
}
