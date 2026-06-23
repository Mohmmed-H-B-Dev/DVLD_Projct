using DVLD_Buisness;
using DVLD_With_MY_teatcher.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private int _PersonId=-1;
        private clsPerson _Person;
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public int PersonId
        {
            get { return _PersonId; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public void ResetPersonInfo()
        {
            lbPerosnID.Text="[?????]";
            lbName.Text="[?????]";
            lbNationalNO.Text="[?????]";
            lbGendor.Text="[?????]";
            lbEmail.Text="[?????]";
            lbAddress.Text="[?????]";
            lbDateOfBirth.Text="[?????]";
            lbPhone.Text="[?????]";
            lbCountry.Text="[?????]";

            pbImage.Image= Resources.Male_512;

        }
        public void LoadPersonInfo(int PersonId)
        {
            _Person = clsPerson.Find(PersonId);

            if( _Person == null )
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = "+PersonId.ToString(), "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _PersonId=PersonId;
            _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);

            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No = "+NationalNo, "Error ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            _FillPersonInfo();
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gendor==0)
            {
                pbImage.Image = Resources.Male_512;
            }
            else
            {
                pbImage.Image = Resources.Female_512 ;
            }
            string ImagePath = _Person.ImagePath;
            if (ImagePath!="")
            {
                if(File.Exists(ImagePath)) 
                pbImage.ImageLocation=ImagePath;
            }
            else
            {
                MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void _FillPersonInfo()
        {
            _PersonId=_Person.PersonId;
            lbPerosnID.Text=_Person.PersonId.ToString();
            lbName.Text=_Person.FullName;
            lbNationalNO.Text=_Person.NationalNo;
            
            lbGendor.Text= _Person.Gendor  == 0 ? "Male ": "Female";
            lbEmail.Text=_Person.Email;
            lbAddress.Text=_Person.Address;
            lbDateOfBirth.Text=_Person.DateOfBirth.ToShortDateString();
            lbPhone.Text=_Person.Phone;
            lbCountry.Text=clsCountry.Find(_Person.NationalityCountryID).CountryName;


            _LoadPersonImage();
        }

        private void llbEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddNew_Update frm =new frmAddNew_Update(_PersonId);

            frm.ShowDialog();

            LoadPersonInfo(_PersonId);
        }
    }

}
