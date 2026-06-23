using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Buisness;
using DVLD_With_MY_teatcher.Globle;

namespace DVLD_With_MY_teatcher.Tests.Controls
{
    public partial class ctrlSchedule_Test : UserControl
    {

        public enum enMode { enAddNew = 0, enUpdate }
        private enMode _Mode = enMode.enAddNew;
        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 }

        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID=value;

                switch (_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text="Vision Test";
                            pbTestTypeImage.Image= DVLD_With_MY_teatcher.Properties.Resources.Vision_512;
                            break;
                        }
                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text="Written Test";
                            pbTestTypeImage.Image= DVLD_With_MY_teatcher.Properties.Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text="Street Test";
                            pbTestTypeImage.Image= DVLD_With_MY_teatcher.Properties.Resources.driving_test_512;
                            break;
                        }

                }

            }
        }
        public ctrlSchedule_Test()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int AppointmentID = -1)
        {
            if (AppointmentID==-1)
                _Mode=enMode.enAddNew;
            else
                _Mode=enMode.enUpdate;

            _LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplicationID;
            _TestAppointmentID=AppointmentID;
            _LocalDrivingLicenseApplication=clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
                _CreationMode=enCreationMode.RetakeTestSchedule;
            else
                _CreationMode=enCreationMode.FirstTimeSchedule;


            if (_CreationMode==enCreationMode.RetakeTestSchedule)
            {
                clsTestAppointment a = clsTestAppointment.GetLastTestAppointment(_LocalDrivingLicenseApplicationID, TestTypeID);

                
                if (a.RetakeTestApplicationID>=1)
                    lblRetakeAppFees.Text = clsApplicationType.GetApplicationTypeInfoByID(a.RetakeTestApplicationID).Fees.ToString();
                else
                    lblRetakeAppFees.Text=clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                lblTitle.Text="Schedule Retake Test.";
                gbRetakeTestInfo.Enabled=true;
                lblRetakeTestAppID.Text="0";

            }
            else
            {
                gbRetakeTestInfo.Enabled=false;
                lblTitle.Text="Schedule Test.";
                lblRetakeTestAppID.Text="N/A";
                lblRetakeAppFees.Text="0";




            }

            lblLocalDrivingLicenseAppID.Text=_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text=_LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text=_LocalDrivingLicenseApplication.PersonInfo.FullName;

            if (_Mode==enMode.enAddNew)
            {
                lblFees.Text=clsTestType.Find((int)_TestTypeID).Fees.ToString();
                dtpTestDate.MinDate=DateTime.Now;
                _TestAppointment=new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text=(Convert.ToSingle(lblFees.Text)+Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (!_HandleAppointmentLockedConstraint())
                return;
            if (!_HandlePerviousTestConstraint())
                return;


        }


        private bool _LoadTestAppointmentData()
        {
            _TestAppointment=clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text=clsTestAppointment.Find(_TestAppointmentID).PaidFees.ToString();

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate)<0)
                dtpTestDate.MinDate=DateTime.Now;
            else
                dtpTestDate.MinDate=_TestAppointment.AppointmentDate;

            dtpTestDate.Value=_TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID==-1)
            {
                lblRetakeAppFees.Text="0";
                lblRetakeTestAppID.Text="N/A";
            }
            else
            {
                lblRetakeAppFees.Text=_TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                lblTitle.Text="Schedule Retake Test.";
                gbRetakeTestInfo.Enabled=true;
                lblRetakeTestAppID.Text=_TestAppointment.RetakeTestApplicationID.ToString();

            }
            return true;
        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (!(_Mode==enMode.enAddNew&& clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID)))
            {
                return true;
            }

            lblUserMessage.Text = "Person Already have an active appointment for this test";
            btnSave.Enabled=false;
            dtpTestDate.Enabled=false;
            return false;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblUserMessage.Visible=true;
                lblUserMessage.Text = "Person already sat for the test, appointment locked.";
                btnSave.Enabled=false;
                dtpTestDate.Enabled=false;
                return false;

            } else
            {
                lblUserMessage.Visible=false;
            }

            return true;
        }
        private bool _HandlePerviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:

                    return true;

                case clsTestType.enTestType.WrittenTest:
                    
                        if (!clsLocalDrivingLicenseApplication.DoesPassTestType(_LocalDrivingLicenseApplicationID, clsTestType.enTestType.VisionTest))
                        {
                            lblUserMessage.Visible=true;
                            btnSave.Enabled=true; 
                            dtpTestDate.Enabled=true;
                            lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";

                            return false;
                        }
                        else
                        {
                            lblUserMessage.Visible=false;
                            btnSave.Enabled=true;
                            dtpTestDate.Enabled=true;
                        
                        }
                    return true;

                case clsTestType.enTestType.StreetTest:

                    if (!clsLocalDrivingLicenseApplication.DoesPassTestType(_LocalDrivingLicenseApplicationID, clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Visible=true;
                        btnSave.Enabled=false;
                        dtpTestDate.Enabled=false;
                        lblUserMessage.Text = "Cannot Schedule, Written Test should be passed first";

                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible=false;
                        btnSave.Enabled=true;
                        dtpTestDate.Enabled=true;

                    }
                    return true;


            }
            return true;
        }


        private bool _HandleRetakeApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linkit to the appoinment.
            if (_Mode == enMode.enAddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we link it with the appointment.

                //First Create Applicaiton 
                clsApplication Application = new clsApplication();

                Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicantPersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.RetakeTest).Fees;
                Application.CreatedByUserID = clsGlobal.CurrentUser.UserId;



                if (!Application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.RetakeTestApplicationID =Application.ApplicationID;

             
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!_HandleRetakeApplication())
                return;

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByUserID=clsGlobal.CurrentUser.UserId;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.enUpdate;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
    }
}
