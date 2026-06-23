using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsUserDataAccess
    {
        // تم إرساء نص الاتصال كمتغير للقراءة فقط، ولم نعد نفتح اتصالاً عاماً مشتركاً
        private static readonly string strConnection = clsDataAccessSitting.ConnectionString;

        public static bool FindUserByUserNameAndPassword(string UserName, string Password,
            ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE Username = @Username and Password=@Password;";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                isFound = true;
                                UserID = (int)rdr["UserID"];
                                PersonID = (int)rdr["PersonID"];
                                UserName = (string)rdr["UserName"];
                                Password = (string)rdr["Password"];
                                IsActive = (bool)rdr["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ FindUserByUserNameAndPassword", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            } // يتم إغلاق وتفريغ الاتصال تلقائياً هنا

            return isFound;
        }

        public static bool ResetPasswordAndfindUser(string Text, ref string UserName, ref string Password,
         ref int UserID, ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand("GetUserInfoToResetPassword", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure; // تحديد نوع الأمر كـ Stored Procedure
                    cmd.Parameters.AddWithValue("@Text", string.IsNullOrEmpty(Text) ? (object)DBNull.Value : Text);

                    try
                    {
                        conn.Open();
                        // تعديل مهم: تم تغيير السيرفر ليقرأ من الـ cmd المحلي التابع للـ using وليس conn العام القديم
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                isFound = true;
                                UserID = (int)rdr["UserID"];
                                PersonID = (int)rdr["PersonID"];
                                UserName = (string)rdr["UserName"];
                                Password = (string)rdr["Password"];
                                IsActive = (bool)rdr["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ ResetPasswordAndfindUser", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return isFound;
        }

        public static bool FindUserByUserID(int UserID, ref string UserName, ref string Password,
            ref int PersonID, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE UserID = @UserID;";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                isFound = true;
                                PersonID = (int)rdr["PersonID"];
                                UserName = (string)rdr["UserName"];
                                Password = (string)rdr["Password"];
                                IsActive = (bool)rdr["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ FindUserByUserID", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }

            return isFound;
        }

        public static bool FindUserByPersonID(ref int UserID, ref string UserName, ref string Password,
            int PersonID, ref bool IsActive)
        {
            bool isFound = false;
            string query = "SELECT * FROM Users WHERE PersonID = @PersonID;";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                isFound = true;
                                UserID = (int)rdr["UserID"];
                                UserName = (string)rdr["UserName"];
                                Password = (string)rdr["Password"];
                                IsActive = (bool)rdr["IsActive"];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ FindUserByPersonID", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }

            return isFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {
            int UserID = -1;
            // تعديل: تصحيح الكلمة الإملائية الخاطئة INSSERT INTo إلى INSERT INTO
            string query = @"INSERT INTO Users (PersonID, UserName, Password, IsActive) 
                            VALUES (@PersonID, @UserName, @Password, @IsActive);
                            SELECT SCOPE_IDENTITY();";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@Password", Password);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            UserID = ID;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ AddNewUser", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }

            return UserID;
        }

        public static bool UpdateUser(int UserID, int PersonID, string UserName, bool IsActive)
        {
            int RowsAffected = 0;
            string query = @"UPDATE Users 
                            SET PersonID = @PersonID,
                                UserName = @UserName,
                                IsActive = @IsActive 
                            WHERE UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    cmd.Parameters.AddWithValue("@IsActive", IsActive);

                    try
                    {
                        conn.Open();
                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ UpdateUser", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }

            return (RowsAffected > 0);
        }

        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            string query = @"SELECT Users.UserID, Users.PersonID, 
                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName, 
                            Users.UserName, Users.IsActive 
                            FROM Users 
                            INNER JOIN People ON Users.PersonID = People.PersonID;";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.HasRows)
                            {
                                dt.Load(rdr);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ GetAllUsers", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return null;
                    }
                }
            }

            return dt;
        }

        public static bool DeleteUser(int UserID)
        {
            string query = "DELETE Users WHERE UserID = @UserID;";
            int RowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        conn.Open();
                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ DeleteUser", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            return (RowsAffected > 0);
        }

        public static bool IsUserExistByUserID(int UserID)
        {
            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int num))
                        {
                            rowsAffected = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ IsUserExistByUserID", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsUserExistByUserName(string UserName)
        {
            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int num))
                        {
                            rowsAffected = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ IsUserExistByUserName", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@PersonID", PersonID);
                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int num))
                        {
                            rowsAffected = num;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ IsUserExistForPersonID", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            return (rowsAffected > 0);
        }

        public static bool IsPasswordCorrect(int UserID, string CurrentPassword)
        {
            bool isPasswordValid = false;

            
            string query = "sp_CheckPasswordToChange";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Password", CurrentPassword);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        // إذا وجد السجل، فإن ExecuteScalar ستعيد 1
                        if (result != null && int.TryParse(result.ToString(), out int val) && val == 1)
                        {
                            isPasswordValid = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ IsPasswordCorrect", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                        return false;
                    }
                }
            }
            return isPasswordValid;
        }

        public static bool ChangePassword(int UserID, string Password)
        {
            int RowsAffected = 0;
            string query = "UPDATE [dbo].[Users] SET [Password] = @Password WHERE UserID=@UserID;";

            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@Password", Password);

                    try
                    {
                        conn.Open();
                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        clsUntil.HandelEventViewerExceptions("clsUserDataAccess _ ChangePassword", "Error : " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }

            return (RowsAffected > 0);
        }
    }
}