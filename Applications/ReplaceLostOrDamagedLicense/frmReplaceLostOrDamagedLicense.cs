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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicense: Form
    {
        int _NewLicenseID = -1;

        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {

            if (rbDamagedLicense.Checked)
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

        }
        private clsLicense.enIssueReason _GetIssueReason()
        {

            if (rbDamagedLicense.Checked)
                return clsLicense.enIssueReason.DamagedReplacement;
            else
                return clsLicense.enIssueReason.LostReplacement;

        }
        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text=clsFormat.ToShort(DateTime.Now);
            lblCreatedByUser.Text=clsGlobal.CurrentUser.UserName;

            rbDamagedLicense.Checked=true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text="Replacement for Damaged License";
            this.Text=lblTitle.Text;
            lblApplicationFees.Text=clsApplicationType.GetApplicationTypeInfoByID(_GetApplicationTypeID()).ToString();
        
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text="Replacement for Lost License";
            this.Text=lblTitle.Text;
            lblApplicationFees.Text=clsApplicationType.GetApplicationTypeInfoByID(_GetApplicationTypeID()).ToString();

        }

        private void ctrlDrivingLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicenseID.Text=obj.ToString();
            llShowLicenseHistory.Enabled=(SelectedLicenseID!=-1);

            if (SelectedLicenseID==-1)
            {
                return;
            }


            
            if (!ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Active,Choose an active License  : "
              , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled=false;
                return;
            }
            btnIssueReplacement.Enabled=true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license.", "Confirm", MessageBoxButtons.YesNoCancel)==DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlDrivingLicenseWithFilter1.
                SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserId);
               

            if (NewLicense==null)
            {
                MessageBox.Show("Failed to Renew License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text=NewLicense.ApplicationID.ToString();
            _NewLicenseID=NewLicense.LicenseID;
            lblRreplacedLicenseID.Text=_NewLicenseID.ToString();
            MessageBox.Show("Licensed Renewed Successfully With ID : "+_NewLicenseID.ToString(), "Licensed Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);


            btnIssueReplacement.Enabled=false;
            gbReplacementFor.Enabled=false;
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            llShowLicenseInfo.Enabled=true;
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm =
                 new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmReplaceLostOrDamagedLicense_Activated(object sender, EventArgs e)
        {
            ctrlDrivingLicenseWithFilter1.TxtLicenseFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
