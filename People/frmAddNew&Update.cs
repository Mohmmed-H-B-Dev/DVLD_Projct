using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
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

namespace DVLD_With_MY_teatcher.People
{
    public partial class frmAddNew_Update : Form
    {
        // Declare a delegate
        public delegate void DataBackEvenHandler(object sender, int PersonId);
        // Declare an event using the delegate
        public event DataBackEvenHandler DataBack; 
        enum enMode { AddNew = 1, Update = 2 }
        enum enGendor{Male=0,Female=1}

        clsPerson _Person;
        private int _PersonId = -1;
        enMode _Mode;
        enGendor _Gender;
        public frmAddNew_Update()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddNew_Update(int PersonID)
        {
            InitializeComponent();
            _PersonId=PersonID;
            _Mode = enMode.Update;
        }

        void _FillCountriesInComoboBox()
        {
            DataTable dt=clsCountry.GetAllCountries();

            foreach(DataRow dr in dt.Rows)
            {

                cmbCountries.Items.Add(dr["CountryName"]);
            }
        }



        void _ResetDefualtValues()
        {
            _FillCountriesInComoboBox();

            //this will initialize the reset the defaule values
            _FillCountriesInComoboBox();

            if (_Mode == enMode.AddNew)
            {
                lbTitle.Text = "Add New Person";
                this.Text="Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lbTitle.Text = "Update Person";
                this.Text="Update Person";
            }

            if (rbtnMale.Checked)
            {
                pbPersonImage.Image=Resources.Male_512;

            }
            if(rbtnFemale.Checked)
            {
                pbPersonImage.Image=Resources.Female_512;
            }


            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);


            dptDateOfBirth.MaxDate=DateTime.Now.AddYears(-18);
            dptDateOfBirth.Value= dptDateOfBirth.MaxDate;

            dptDateOfBirth.MinDate=DateTime.Now.AddYears(-100);

            cmbCountries.SelectedIndex=cmbCountries.FindString("Yemen");

            txtFirstName.Text="";
            txtSecondName.Text="";
            txtThirdName.Text="";
            txtLastName.Text="";
            txtNationalNo.Text="";
            txtPhone.Text="";
            txtEmail.Text="";
            txtAddress.Text="";
            rbtnMale.Checked=true;




        }
        void _LoadData()
        {
            _Person=clsPerson.Find(_PersonId);
            if (_Person==null)
            {
                MessageBox.Show("No Person with ID = " + _PersonId, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }


            lblPersonId.Text= _PersonId.ToString();
            txtFirstName.Text=_Person.FirstName;
            txtSecondName.Text=_Person.SecondName;
            txtThirdName.Text=_Person.ThirdName;
            txtLastName.Text=_Person.LastName;
            txtNationalNo.Text= _Person.NationalNo;
            txtPhone.Text= _Person.Phone;
            txtAddress.Text= _Person.Address;
            txtEmail.Text = _Person.Email;
            dptDateOfBirth.Value= _Person.DateOfBirth;

            if (_Person.Gendor==0)
                rbtnMale.Checked= true;
            else if(_Person.Gendor==1)
                rbtnFemale.Checked= true;

            if(_Person.ImagePath!="")
            {
                pbPersonImage.ImageLocation=_Person.ImagePath;
            }

            cmbCountries.SelectedIndex=cmbCountries.FindString(_Person.countryInfo.CountryName);

            llRemoveImage.Visible = (_Person.ImagePath != "");
        }


        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.

            if (_Person.ImagePath!=pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath!="")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }catch(Exception ex)
                    {
                        // We could not delete the file.
                        //log it later 
                    }

                }

                if(pbPersonImage.ImageLocation!=null)
                {
                    string SourceImageFile=pbPersonImage.ImageLocation.ToString();
                    if(clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation=SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbtnFemale_CheckedChanged(object sender, EventArgs e)
        {
            if(pbPersonImage.ImageLocation==null)
            pbPersonImage.Image = Resources.Female_512;
        }

        private void rbtnMale_CheckedChanged(object sender, EventArgs e)
        {
            //change the defualt image to male incase there is no image set.
            if (pbPersonImage.ImageLocation==null)
                pbPersonImage.Image = Resources.Male_512;
           
        }

        private void frmAddNew_Update_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();
            if (_Mode==enMode.Update)
            {
                _LoadData();
            }
   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_HandlePersonImage())
                return;
            int NationalityCountryID = clsCountry.Find(cmbCountries.Text.Trim()).CountryID;
           
            _Person.FirstName= txtFirstName.Text.Trim();
            _Person.SecondName= txtSecondName.Text.Trim();
            _Person.ThirdName= txtThirdName.Text.Trim();
            _Person.LastName= txtLastName.Text.Trim();
            _Person.NationalNo= txtNationalNo.Text.Trim();
            _Person.Phone= txtPhone.Text.Trim();
            _Person.Email= txtEmail.Text.Trim();
            _Person.Address= txtAddress.Text.Trim();
            _Person.DateOfBirth=dptDateOfBirth.Value;

            if (rbtnMale.Checked)
                _Person.Gendor=(byte)enGendor.Male;
            if(rbtnFemale.Checked)
                _Person.Gendor=(byte)enGendor.Female;

            if (pbPersonImage.ImageLocation!=null)
                _Person.ImagePath=pbPersonImage.ImageLocation.ToString();
            else 
                _Person.ImagePath="";

            _Person.NationalityCountryID=NationalityCountryID;

            if (_Person.Save())
            {

                lblPersonId.Text=_Person.PersonId.ToString();
                lbTitle.Text="Update Person";

              
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonId);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter="Image Files|*.jpg;*.jpeg;*.png;*gif;*.bmp";
            openFileDialog1.FilterIndex=1;
            openFileDialog1.RestoreDirectory=true;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string SelectedFilePath=openFileDialog1.FileName;
                pbPersonImage.Load(SelectedFilePath);
                llRemoveImage.Visible=true;
            }

        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation=null;

            if (rbtnFemale.Checked)
                pbPersonImage.Image=Resources.Female_512;
            if (rbtnMale.Checked)
                pbPersonImage.Image=Resources.Male_512;
            
            llRemoveImage.Visible=false;
           

        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This Field is required!");

            }
            else
            {

                e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            if(string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel= true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
            }
            else
            {
                e.Cancel= false;
                errorProvider1.SetError(txtNationalNo, null);

            }
           
            if(txtNationalNo.Text.Trim()!=_Person.NationalNo&& clsPerson.IsPersonExist(txtNationalNo.Text.Trim()) )
            {
                e.Cancel=true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);

            }
        }


        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim()=="")
                return;

            if (!clsValidatoin.ValidateEmail(txtEmail.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail,null);
            }
        }
    }
}
