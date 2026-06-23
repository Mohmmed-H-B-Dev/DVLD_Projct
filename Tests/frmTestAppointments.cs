using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Buisness.clsTestType;

namespace DVLD_With_MY_teatcher.Tests
{
    public partial class frmListTestAppointments: Form
    {

        DataTable _dtTestAppointments;
        int _LocalDrivingLicenseApplicationID;
        clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public frmListTestAppointments(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplicationID;
            _TestTypeID=TestTypeID;
        }
        void _LoadTestTypeImageAndTitle()
        {
            switch (_TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text="Vision Test";
                    this.Text=lblTitle.Text;
                    pbTestTypeImage.Image=DVLD_With_MY_teatcher.Properties.Resources.Vision_512;
                    break;
                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text="Written Test";
                    this.Text=lblTitle.Text;
                    pbTestTypeImage.Image=DVLD_With_MY_teatcher.Properties.Resources.Written_Test_512;
                    break;
                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text="Street Test";
                    this.Text=lblTitle.Text;
                    pbTestTypeImage.Image=DVLD_With_MY_teatcher.Properties.Resources.driving_test_512;
                    break;
            }
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _dtTestAppointments=clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestTypeID);
            
            dgvLicenseTestAppointments.DataSource=_dtTestAppointments;

            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if (dgvLicenseTestAppointments.Rows.Count>0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText="Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width=150;

               dgvLicenseTestAppointments.Columns[1].HeaderText="Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width=200;

               dgvLicenseTestAppointments.Columns[2].HeaderText= "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width=150;

               dgvLicenseTestAppointments.Columns[3].HeaderText= "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width=100;


            }



        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestTypeID))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestTypeID);

            if (LastTest==null)
            {
                frmScheduleTests frm1 = new frmScheduleTests(_LocalDrivingLicenseApplicationID, _TestTypeID);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
                    
            }

            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTests frm2 = new frmScheduleTests(LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestTypeID);
            frm2.ShowDialog();
            frmListTestAppointments_Load(null, null);


        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmScheduleTests frm = new frmScheduleTests(_LocalDrivingLicenseApplicationID, _TestTypeID, TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmTackTest frm = new frmTackTest((int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value, _TestTypeID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
