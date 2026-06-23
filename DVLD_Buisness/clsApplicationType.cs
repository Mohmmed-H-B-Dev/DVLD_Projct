using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public  class clsApplicationType
    {
        public enum enMode {enAddNew=1, enUpdate=2 }
        enMode _Mode = enMode.enAddNew;
        public clsApplicationType(int ApplicationTypeID,string ApplicationTitle,float Fees)
        {
            this.ApplicationTypeID=ApplicationTypeID;
            this.ApplicationTitle=ApplicationTitle;
            this.Fees=Fees;
            this._Mode=enMode.enUpdate;
        }
        public clsApplicationType( )
        {
            this.ApplicationTypeID=-1;
            this.ApplicationTitle="";
            this.Fees=0;
            this._Mode=enMode.enAddNew;
        }

        public int ApplicationTypeID { get; set; }
        public string ApplicationTitle { get; set; }
        public float Fees { get;set; }


         public static clsApplicationType GetApplicationTypeInfoByID(int ApplicationTypeID)
        {
            string Title = "";
            float Fees = 0;
            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationTypeID, ref Title, ref Fees))
            {
                return new clsApplicationType(ApplicationTypeID, Title, Fees);
            }
            else
                return null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeData.GetAllApplicationTypes();

        }
        private  bool _AddNewApplicationType()
        {
            this.ApplicationTypeID=clsApplicationTypeData.AddNewApplicationType(this.ApplicationTitle, this.Fees);
            if (this.ApplicationTypeID>0)
                return true;
            else
                return false;

        }

        private  bool _UpdateApplicationType()
        {
            if (clsApplicationTypeData.UpdateApplicationType(this.ApplicationTypeID, this.ApplicationTitle, this.Fees))
                return true;
            else
                return false;
        }
        public bool Save()
        {
            switch (_Mode)
            {

                case enMode.enAddNew:
                    if (_AddNewApplicationType())
                    {
                        _Mode=enMode.enUpdate;
                        return true;
                    }
                    break;
                case enMode.enUpdate:
                    return _UpdateApplicationType();

            }
            return false;
        }

    }
}
