using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Data;
namespace DVLD_Buisness
{
    public class clsUser
    {

        enum enMode { AddNew=0, Update=1 }
        public clsUser(int UserId,int PersonId,string UserName,string Password ,bool IsActive) 
        {
            this.UserId = UserId;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonId=PersonId;
            Mode=enMode.Update;
        }

        public clsUser()
        {
            this.UserId = 0;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            this.PersonId=0;
            Mode=enMode.AddNew;
        }
        enMode Mode {  get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int PersonId { get; set; }

        //static public string Encoding(string Text)
        //{
        //  return  clsUntil.Encoding(Text);
        //}
        //static public string Decoding(string Text)
        //{
        //    return clsUntil.Decoding(Text);
        //}
        public static clsUser FindUserByUserID(int UserID)
        {
            int PersonID = -1;  string UserName = "";string Password = "";bool IsActive = false;
            if(clsUserDataAccess.FindUserByUserID(UserID,ref UserName,ref Password,ref PersonID,ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;

        }
        public static clsUser FindUserByPersonID(int PersonID)
        {
            int UserID = -1; string UserName = ""; string Password = ""; bool IsActive = false;
            if (clsUserDataAccess.FindUserByPersonID(ref UserID, ref UserName, ref Password,  PersonID, ref IsActive))
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;

        }
       
       public static clsUser ResetPasswordAndfindUser(string Text)
        {

            int UserID = 0;
            string UserName = ""; string Password= "";
            int PersonID = 0;
            bool IsActive = false;
            bool IsFound = clsUserDataAccess.ResetPasswordAndfindUser(Text, ref UserName,ref Password, ref UserID, ref PersonID, ref IsActive);
            if (IsFound==true)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            }

            return null;

        }

        public static clsUser FindUserByUserNameAndPassword(string UserName, string Password)
        {
            int UserID = 0;  
            int PersonID = 0;
            bool IsActive = false;
            bool IsFound=clsUserDataAccess.FindUserByUserNameAndPassword(UserName, Password,ref UserID,ref PersonID,ref IsActive);
            if (IsFound==true)
            {
                return new clsUser(UserID,PersonID,UserName,Password,IsActive);
            }

            return null;

        }

        public static DataTable GetAllUsers()
        {
            return clsUserDataAccess.GetAllUsers();
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserDataAccess.DeleteUser(UserID);
        }
        private bool _AddNewUser()
        {
            this.UserId=clsUserDataAccess.AddNewUser(this.PersonId, this.UserName, this.Password, this.IsActive);

            if (this.UserId>=1)
                return true;
            else
                return false;
        }

        private bool _UpdateUser()
        {
            return clsUserDataAccess.UpdateUser(this.UserId, this.PersonId, this.UserName, this.IsActive);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:

                    if (_AddNewUser())
                    {

                        Mode=enMode.Update;
                        return true;

                    }
                    break;
                case enMode.Update:

                    if (_UpdateUser())
                    {

                      
                        return true;

                    }
                    break;
                default:
                    return false;
                    break;
            }
            return false;
        }

        public static bool IsUserExis(int UserId)
        {

            return clsUserDataAccess.IsUserExistByUserID(UserId);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserDataAccess.IsUserExistByUserName(UserName);
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserDataAccess.IsUserExistForPersonID(PersonID); 
        }

        public static bool ChangePassword(int UserID, string NewPassword)
        {
           
            return clsUserDataAccess.ChangePassword(UserID, NewPassword);
        }
        public static bool IsPasswordCorrect(int UserID, string CurrentPassword)
        {
            return clsUserDataAccess.IsPasswordCorrect(UserID,CurrentPassword);
        }
        public  bool ChangePassword()
        {

            return clsUserDataAccess.ChangePassword(this.UserId, this.Password);
        }

    }
}
