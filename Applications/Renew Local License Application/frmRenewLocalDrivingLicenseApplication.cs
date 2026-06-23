using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
using DVLD_With_MY_teatcher.Globle;
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

namespace DVLD_With_MY_teatcher.Applications.Renew_Local_License_Application
{
    public partial class frmRenewLocalDrivingLicenseApplication: Form
    {
        int _NewLicenseID = -1;

        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text=clsFormat.ToShort(DateTime.Now);
            lblIssueDate.Text=lblApplicationDate.Text;

            lblExpirationDate.Text="[????]";
            lblApplicationFees.Text= clsApplicationType.GetApplicationTypeInfoByID((int) clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedByUser.Text=clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDrivingLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            lblOldLicenseID.Text=SelectedLicenseID.ToString();
            lbllShowLicenseHistroy.Enabled=(SelectedLicenseID!=-1);
            if (SelectedLicenseID==-1)
            {
                return;
            }
            int DefaultValidityLength = ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.LicenseClassIfo.DefaultValidityLength;
            lblExpirationDate.Text=clsFormat.ToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.LicenseClassIfo.ClassFees.ToString();
            lblTotalFees.Text=(Convert.ToSingle(lblLicenseFees.Text)+Convert.ToSingle(lblApplicationFees.Text)).ToString();
            txtNotes.Text=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.Notes;

            //check the License is not Expired.
            if (!ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expiared, it will expire on: " + clsFormat.ToShort(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.ExpirationDate)
               , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled=false;
                return;
            }


            if (!ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Active,Choose an active License  : "
              , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled=false;
                return;
            }
            btnRenewLicense.Enabled=true;

        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to Renew the license.", "Confirm", MessageBoxButtons.YesNoCancel)==DialogResult.No)
            {
                return;
            }

            clsLicense NewLicense = ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.
                RenewLicense(txtNotes.Text, clsGlobal.CurrentUser.UserId);

            if (NewLicense==null)
            {
                MessageBox.Show("Failed to Renew License.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text=NewLicense.ApplicationID.ToString();
            _NewLicenseID=NewLicense.LicenseID;
            lblRenewedLicenseID.Text=_NewLicenseID.ToString();
            MessageBox.Show("Licensed Renewed Successfully With ID : "+_NewLicenseID.ToString(), "Licensed Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);


            btnRenewLicense.Enabled=false;
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            lbllShowNewLicenseInfo.Enabled=true;

        }

        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDrivingLicenseWithFilter1.TxtLicenseFocus();
        }

        private void lbllShowNewLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();

        }

        private void lbllShowLicenseHistroy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("This Form is Not Completed","Information",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }
    }
}
