using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsPerson
    {

        enum enMode {AddNewPerson=1,UpdatePerson=2};
        enMode _Mode = enMode.AddNewPerson;
        public int PersonId { set; get; }
        public string FirstName {  set; get; }
        public string SecondName {  set; get; }
        public string ThirdName {  set; get; }  
        public string LastName { set; get; }

        public string FullName
        {
            get { return FirstName+" "+SecondName+" "+ThirdName+" "+LastName; } 
        }
        public string NationalNo {  set; get; } 
        public DateTime DateOfBirth { set; get; }
        public byte Gendor {  set; get; }
        public string Address {  set; get; }
        public string Phone {  set; get; }
        public string Email {  set; get; }
        public int NationalityCountryID {  set; get; }

        private string _ImagePath {  set; get; }

        public string ImagePath
        {

            get { return _ImagePath; }
            set { _ImagePath=value; } 
        }
        public clsCountry countryInfo;
        public clsPerson() 
        {
            this.PersonId = 0;
            this.FirstName="";
            this.SecondName="";
            this.ThirdName="";
            this.LastName="";
            this.NationalNo="";
            this.DateOfBirth=new DateTime();
            this.Gendor=0;
            this.Address="";
            this.Phone="";
            this.Email="";
            this.NationalityCountryID=-1;
            this.ImagePath="";
            _Mode=enMode.AddNewPerson;
        }

        public clsPerson(int PersonId,string FirstName,string SecondName,string ThirdName,string LastName,
       string NationalNo,DateTime DateOfBirth,byte Gendor,string Address, 
       string Phone, string Email,int NationalityCountryID, string ImagePath
            )
        {
            this.PersonId =PersonId;
            this.FirstName=FirstName;
            this.SecondName=SecondName;
            this.ThirdName=ThirdName;
            this.LastName=LastName;
            this.NationalNo=NationalNo;
            this.DateOfBirth=DateOfBirth;
            this.Gendor=Gendor;
            this.Address=Address;
            this.Phone=Phone;
            this.Email=Email;
            this.NationalityCountryID=NationalityCountryID;
            this.countryInfo=clsCountry.Find(NationalityCountryID);
            this.ImagePath=ImagePath;
            _Mode=enMode.UpdatePerson;
        }

        private bool _AddNewPerson()
        {
            this.PersonId=clsPeopleDataAccess.AddNewPerson(
                this.NationalNo, this.FirstName, this.SecondName,
                this.ThirdName, this.LastName, this.DateOfBirth,
                this.Gendor, this.Address, this.Phone, this.Email, 
                this.NationalityCountryID, this.ImagePath
                );

            if (this.PersonId>0)
            {
                return true;
            }else 

            return false;
        }


        private bool _UpdatePerson()
        {
          return   clsPeopleDataAccess.UpdatePerson(
                this.PersonId, this.FirstName, this.SecondName,
                this.ThirdName,this.LastName, this.NationalNo,this.DateOfBirth,
                this.Gendor, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath
                );

            
        }



        public static clsPerson Find(int PersonId)
        {
            string FirstName = ""; string SecondName = "";
            string ThirdName = ""; string LastName = ""; string NationalNo = "";
            DateTime DateOfBirth = DateTime.Now; byte Gendor = 0;
            string Address = ""; string Phone = ""; string Email = "";
            int NationalityCountryID = -1; string ImagePath = "";

            bool isfound = clsPeopleDataAccess.GetPersonByPersonId(PersonId, ref FirstName, ref SecondName,
          ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth,
           ref Gendor, ref Address, ref Phone, ref Email,
           ref NationalityCountryID, ref ImagePath);
            if (isfound)
            {
                return new clsPerson(PersonId, FirstName, SecondName, ThirdName, LastName, NationalNo,
                    DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else

                return null;

        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = ""; string SecondName = "";
            string ThirdName = ""; string LastName = ""; int PersonId  = -1 ;
            DateTime DateOfBirth = DateTime.Now; byte Gendor = 0;
            string Address = ""; string Phone = ""; string Email = "";
            int NationalityCountryID = -1; string ImagePath = "";

            bool isfound = clsPeopleDataAccess.GetPersonByNationalNo(
                NationalNo,ref PersonId, ref FirstName, ref SecondName,
          ref ThirdName, ref LastName, ref DateOfBirth,
           ref Gendor, ref Address, ref Phone, ref Email,
           ref NationalityCountryID, ref ImagePath);
            if (isfound)
            {
                return new clsPerson(PersonId, FirstName, SecondName, ThirdName, LastName, NationalNo,
                    DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else

                return null;

        }




        public bool Save()
        {
            switch( _Mode )
            {

                case enMode.AddNewPerson:
                    if (_AddNewPerson())
                    {
                        _Mode=enMode.UpdatePerson;
                        return true;
                    }
                    break;
                    case enMode.UpdatePerson:  
                    return _UpdatePerson();
                    
            }
            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPeopleDataAccess.GetAllPeople();
        }
        public static bool DeletePerson(int PersonId)
        {
            return clsPeopleDataAccess.DeletePerson(PersonId);
        }
        public static bool IsPersonExist(int PersonId)
        {
            return clsPeopleDataAccess.IsPersonExist(PersonId);
        }
        public static bool IsPersonExist(string NationalNo)
        {
            return clsPeopleDataAccess.IsPersonExist(NationalNo);
        }

    }
}
