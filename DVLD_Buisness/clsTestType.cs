using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsTestType
    {
        public enum enMode { enAddNew = 1, enUpdate = 2 }
        enMode _Mode;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public clsTestType.enTestType ID { set; get; }
        public clsTestType()
        {
            this._Mode=enMode.enAddNew;
            this.TestTypeID=enTestType.VisionTest;
            this.Description="";
            this.Fees=0;
            this.TestTypeTitle="";
        }


        public clsTestType(enTestType TestTypeID, string TestTypeTitle, string description, float fees)
        {
            this._Mode=enMode.enUpdate;
            this.TestTypeID=TestTypeID;
            this.Description=description;
            this.Fees=fees;
            this.TestTypeTitle=TestTypeTitle;
        }

        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string Description { set; get; }
        public float Fees { set; get; }


        public static clsTestType Find(int TestTypeID)
        {
            string Description = ""; float Fees = 0; string TestTypeTitle = "";
            if (clsTestTypes.GetTestTypeInfoByID(TestTypeID, ref TestTypeTitle, ref Description, ref Fees))
            {
                return new clsTestType((enTestType) TestTypeID, TestTypeTitle, Description, Fees);
            }
            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypes.GetAllTestTypes();
        }


        private bool _AddNewTestType()
        {
            this.TestTypeID=(enTestType)clsTestTypes.AddNewTestType(this.TestTypeTitle, this.Description, this.Fees);
            if (this.TestTypeID>0)
                return true;
            else
                return false;
        }

        private bool _UpdateTestType()
        {
          return  clsTestTypes.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle, this.Description, this.Fees);
        }



        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.enAddNew:
                    if (_AddNewTestType())
                    {
                        _Mode=enMode.enUpdate;
                        return true;
                    }
                    break;

                case enMode.enUpdate:
                    if (_UpdateTestType())
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }


    }
}
