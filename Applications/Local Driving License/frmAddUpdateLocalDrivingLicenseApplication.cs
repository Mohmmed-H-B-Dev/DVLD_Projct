using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
using DVLD_With_MY_teatcher.Globle;
using DVLD_With_MY_teatcher.People.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.Local_Driving_License
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication: Form
    {

        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        clsLocalDrivingLicenseApplication _localDrivingLicenseApplication;

        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode=enMode.AddNew;
        }
        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _Mode=enMode.Update;
            _LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplicationID;
        }

        private void _FillLicenseClassesInComoboBox()
        {
            DataTable lc = clsLicenseClass.GetAllLicenseClasses();

            foreach(DataRow row in lc.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }

        }



        private void _ResetDefualValue()
        {
            _FillLicenseClassesInComoboBox();

            if (_Mode==enMode.AddNew)
            {
                lblTitle.Text="New Local Driving License Application.";
                this.Text="New Local Driving License Application.";
                _localDrivingLicenseApplication =new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter2.FilterFocus();

                tpApplicationInfo.Enabled=false;
                cbLicenseClass.SelectedIndex=2;
                lblFees.Text=clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text=DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text=clsGlobal.CurrentUser.UserName;


            }
            else if (_Mode==enMode.Update)
            {
                lblTitle.Text="Update Local Driving License Application.";
                this.Text="Update Local Driving License Application.";
                ctrlPersonCardWithFilter2.FilterEnabled=false;

                tpApplicationInfo.Enabled=true;
                btnSave.Enabled=true;
            }


        }

        private void _LoadDate()
        {
            ctrlPersonCardWithFilter2.FilterEnabled=false;
            _localDrivingLicenseApplication=clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (_localDrivingLicenseApplication==null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            ctrlPersonCardWithFilter2.LoadPersonInfo(_localDrivingLicenseApplication.ApplicantPersonID);
            lblApplicationDate.Text=clsFormat.ToShort(_localDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(_localDrivingLicenseApplication.LicenseClassInfo.ClassName);
            lblFees.Text=_localDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text=_localDrivingLicenseApplication.CreatedByUserInfo.UserName;
            tcApplicationInfo.SelectedTab=tcApplicationInfo.TabPages["tpApplicationInfo"];

        }



        private void ctrlPersonCardWithFilter2_Load(object sender, EventArgs e)
        {

        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualValue();
            if (_Mode==enMode.Update)
            {
                _LoadDate();
            }
        }

   

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode==enMode.Update)
            {
                btnSave.Enabled=true;
                tpApplicationInfo.Enabled=true;
                tcApplicationInfo.SelectedTab=tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter2.PersonId!=-1)
            {
                btnSave.Enabled=true;
                tpApplicationInfo.Enabled=true;
                tcApplicationInfo.SelectedTab=tcApplicationInfo.TabPages["tpApplicationInfo"];

            }
            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter2.FilterFocus();
            }
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter2.FilterFocus();
        }

        private void ctrlPersonCardWithFilter2_OnSelectedPerson(int obj)
        {
            _SelectedPersonID=obj;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(ctrlPersonCardWithFilter2.PersonId, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if(ActiveApplicationID!=-1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter2.PersonId, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _localDrivingLicenseApplication=new clsLocalDrivingLicenseApplication();
            _localDrivingLicenseApplication.ApplicantPersonID=ctrlPersonCardWithFilter2.PersonId;
            _localDrivingLicenseApplication.ApplicationDate=DateTime.Now;
            _localDrivingLicenseApplication.ApplicationTypeID=1;
            _localDrivingLicenseApplication.ApplicationStatus=clsApplication.enApplicationStatus.New;
            _localDrivingLicenseApplication.LastStatusDate=DateTime.Now;
            _localDrivingLicenseApplication.PaidFees=Convert.ToSingle(lblFees.Text.Trim());
            _localDrivingLicenseApplication.LicenseClassID=LicenseClassID;
            _localDrivingLicenseApplication.CreatedByUserID=clsGlobal.CurrentUser.UserId;


            if (_localDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text=_localDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString() ;
                _Mode=enMode.Update;

                lblTitle.Text="Update Local Driving License Application.";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

    }
}
