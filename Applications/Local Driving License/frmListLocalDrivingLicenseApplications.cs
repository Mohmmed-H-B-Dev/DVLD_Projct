using DVLD_Buisness;
using DVLD_With_MY_teatcher.Licenses;
using DVLD_With_MY_teatcher.Licenses.Local_Licenses;
using DVLD_With_MY_teatcher.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_With_MY_teatcher.Applications.Local_Driving_License
{

    public partial class frmListLocalDrivingLicenseApplications: Form
    {
        DataTable _dtLocalDrivingLicenseApplication = new DataTable();

        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }







        private void _ScheduleTests(clsTestType.enTestType TestType)
        {
            frmListTestAppointments frm = new frmListTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, TestType);

            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void btnAddNewLocalDrivingApp_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLocalDrivingLicenseApplication=clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource=_dtLocalDrivingLicenseApplication;
            cbFilterBy.SelectedIndex=0;
            lblRecordsCount.Text=dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
            if (dgvLocalDrivingLicenseApplications.Rows.Count>0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText="L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width=110;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText="Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width=300;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText="National No";
                dgvLocalDrivingLicenseApplications.Columns[2].Width=130;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText="Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width=357;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText="Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width=190;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText="Passed Test";
                dgvLocalDrivingLicenseApplications.Columns[5].Width=90;


            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm =
                       new frmLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterBy.Visible=(cbFilterBy.Text!="None");

            if (txtFilterBy.Visible)
            {
                txtFilterBy.Text="";
                txtFilterBy.Focus();
            }
            _dtLocalDrivingLicenseApplication.DefaultView.RowFilter="";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {

            string FilteringColumn = "";
            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    FilteringColumn="LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilteringColumn="NationalNo";
                    break;
                case "Full Name":
                    FilteringColumn="FullName";
                    break;
                case "Status":
                    FilteringColumn="Status";
                    break;
                default:
                    FilteringColumn="None";
                    break;

            }


            if (txtFilterBy.Text==""|| FilteringColumn=="None")
            {
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter="";
                lblRecordsCount.Text=dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (FilteringColumn=="LocalDrivingLicenseApplicationID")
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter=string.Format("[{0}] = {1}", FilteringColumn, txtFilterBy.Text);
            else
                _dtLocalDrivingLicenseApplication.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%' ", FilteringColumn, txtFilterBy.Text);


            lblRecordsCount.Text=dgvLocalDrivingLicenseApplications.Rows.Count.ToString();


        }

        private void txtFilterBy_KeyPress(object sender, KeyPressEventArgs e)
        {
            //We allow input jest a number
            if (cbFilterBy.Text=="L.D.L.AppID")
                // e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
                e.Handled= !char.IsNumber(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication licenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(localDrivingLicenseApplicationID);

            int TotalPassedTest = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = licenseApplication.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled=((TotalPassedTest==3)&& !LicenseExists);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled=!(licenseApplication.ApplicationStatus!=clsApplication.enApplicationStatus.New);

            editToolStripMenuItem.Enabled=!LicenseExists && !(licenseApplication.ApplicationStatus!=clsApplication.enApplicationStatus.New);
            ScheduleTestsMenue.Enabled=!LicenseExists;

            CancelApplicaitonToolStripMenuItem.Enabled=(licenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.New);
            DeleteApplicationToolStripMenuItem.Enabled=(licenseApplication.ApplicationStatus!=clsApplication.enApplicationStatus.Completed);


            bool VisionTest = licenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool WrittenTest = licenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool StreetTest = licenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            
            ScheduleTestsMenue.Enabled=(!VisionTest||!WrittenTest||!StreetTest) && (licenseApplication.ApplicationStatus==clsApplication.enApplicationStatus.New);
            editToolStripMenuItem.Enabled=!(VisionTest||WrittenTest||StreetTest);



            if (ScheduleTestsMenue.Enabled)
            {
                scheduleVisionTestToolStripMenuItem.Enabled=  !VisionTest;
                scheduleWrittenTestToolStripMenuItem.Enabled= VisionTest && !WrittenTest;
                scheduleStreetTestToolStripMenuItem.Enabled= VisionTest && WrittenTest && !StreetTest;


            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmAddUpdateLocalDrivingLicenseApplication frm =
                         new frmAddUpdateLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you do want to delete this Application..", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.No)
                return;

            clsLocalDrivingLicenseApplication drivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            if (drivingLicenseApplication!=null)
            {
                if (drivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete application,other data depends on it. ", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            else
            {
                MessageBox.Show("This object is Empty there is not data in it by ID :" +dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelApplicaitonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you do want to Cancel this Application..", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.No)
                return;

            clsLocalDrivingLicenseApplication drivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            if (drivingLicenseApplication!=null)
            {
                if (drivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Canceled Successfully.", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not Cancel application,other data depends on it. ", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            }
            else
            {
                MessageBox.Show("This object is Empty there is not data in it by ID :" +dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScheduleTestsMenue_Click(object sender, EventArgs e)
        {

        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTests(clsTestType.enTestType.VisionTest);

        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTests(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTests(clsTestType.enTestType.StreetTest);
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);


        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplication.
                FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID).
                GetActiveLicenseID();
            if (LicenseID!=-1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else
            {
                MessageBox.Show("No license found!!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value).ApplicantPersonID;  
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
    }
}
