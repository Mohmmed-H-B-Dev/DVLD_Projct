using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
using DVLD_With_MY_teatcher.Globle;
using DVLD_With_MY_teatcher.Licenses;
using DVLD_With_MY_teatcher.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.Release_Detained_Licenses
{
    public partial class frmReleaseDetainedLicense: Form
    {
        int _SelectedLicenseID = -1;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicense(int selectedLicenseID)
        {
            InitializeComponent();
            ctrlDrivingLicenseWithFilter1.LoadLicenseInfo(selectedLicenseID);
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            _SelectedLicenseID=selectedLicenseID;
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {

        }

        private void ctrlDrivingLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID=obj;
            lblLicenseID.Text=_SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled=(_SelectedLicenseID!=-1);

            if (_SelectedLicenseID==-1)
            {
                return;

            }
            if (!ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License i is not detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationFees.Text=clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
            lblDetainID.Text=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();
            lblLicenseID.Text=ctrlDrivingLicenseWithFilter1.LicenseID.ToString();
            lblCreatedByUser.Text=clsGlobal.CurrentUser.UserName;


            lblDetainDate.Text=clsFormat.ToShort(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainDate);
            lblCreatedByUser.Text=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.CreatedByUserInfo.UserName;
            lblFineFees.Text=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text=(Convert.ToSingle(lblFineFees.Text.Trim())+Convert.ToSingle(lblApplicationFees.Text.Trim())).ToString();


            btnRelease.Enabled=true;
        }

        private void frmReleaseDetainedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDrivingLicenseWithFilter1.TxtLicenseFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.LicenseID);
            frm.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained  license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;
            bool IsReleaseDetained = ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserId, ref ApplicationID);

            if (!IsReleaseDetained)
            {
                MessageBox.Show("Filed release detained license ..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            else
            {
                MessageBox.Show("Detained License released successfully ..", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            btnRelease.Enabled=false;
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            llShowLicenseInfo.Enabled=true;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
