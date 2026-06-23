using DVLD.Login;
using DVLD_With_MY_teatcher.Applications.Application_Types;
using DVLD_With_MY_teatcher.Applications.International_Licenses;
using DVLD_With_MY_teatcher.Applications.Local_Driving_License;
using DVLD_With_MY_teatcher.Applications.Release_Detained_Licenses;
using DVLD_With_MY_teatcher.Applications.Renew_Local_License_Application;
using DVLD_With_MY_teatcher.Applications.ReplaceLostOrDamagedLicense;
using DVLD_With_MY_teatcher.Drivers;
using DVLD_With_MY_teatcher.Globle;
using DVLD_With_MY_teatcher.Licenses.Detain_Licenses;
using DVLD_With_MY_teatcher.People;
using DVLD_With_MY_teatcher.Test_Type;
using DVLD_With_MY_teatcher.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher
{
    public partial class frmMain : Form
    {
        frmLogin _frmLogin;
        
        public frmMain( frmLogin login)
        {
            InitializeComponent();
            _frmLogin =  login;
            
        }
       ~frmMain()
        {

            _frmLogin.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserId);
            frm.ShowDialog();

        }

        private void signInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (!_frmLogin.GetRememberMe())
            //{
            //    _frmLogin.Clean_txtUserName();
            //    _frmLogin.Clean_txtPassword();
            //}
            clsGlobal.CurrentUser=null;
            _frmLogin.Show();
            
            this.Close();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void peToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void tsmiPeople_Click(object sender, EventArgs e)
        {
            frmListPeople frm=new frmListPeople();
            frm.ShowDialog();
        }

        private void tsmiUsers_Click(object sender, EventArgs e)
        {
            frmListUsers frm = new frmListUsers();
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserId);
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes frm = new frmListApplicationTypes();
            frm.ShowDialog();

        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
        }

        private void tsmiDrivers_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();

        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicenseApplication frm = new frmRenewLocalDrivingLicenseApplication();
          
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicense frm = new frmReplaceLostOrDamagedLicense();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();

        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
                
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainLicenses frm = new frmListDetainLicenses();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplications frm = new frmDetainLicenseApplications();
            frm.ShowDialog();

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications frm = new frmListInternationalLicenseApplications();
            frm.ShowDialog();
        }

        private void localDrivingLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }
    }
}
