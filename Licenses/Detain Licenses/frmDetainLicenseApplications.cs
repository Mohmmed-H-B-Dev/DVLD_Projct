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

namespace DVLD_With_MY_teatcher.Licenses.Detain_Licenses
{
    public partial class frmDetainLicenseApplications: Form
    {
        int _DetainID = -1;
        int _SelectedLicenseID = -1;

        public frmDetainLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                MessageBox.Show( "you have error put the mouse in red icon.", "Validate", MessageBoxButtons.OK, MessageBoxIcon.Error) ;
                return;
            }
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            

            _DetainID=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text.Trim()), clsGlobal.CurrentUser.UserId);
            if (_DetainID == -1)
            {
                MessageBox.Show("Faild to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }


            lblDetainID.Text=_DetainID.ToString();
            MessageBox.Show("License Detained Successfully with ID=" + _DetainID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnDetain.Enabled=false;
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            txtFineFees.Enabled=false;
            llShowLicenseInfo.Enabled=true;


        }

        private void frmDetainLicenseApplications_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text=clsFormat.ToShort( DateTime.Now);
            lblCreatedByUser.Text=clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDrivingLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID=obj;
            if (_SelectedLicenseID==-1)
            {
                return;
            }
            lblLicenseID.Text=_SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled=(_SelectedLicenseID!=-1);

            if (ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("The License is already detained , choose an other one.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDetain.Enabled=true;
            txtFineFees.Focus();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            }
            ;


            if (!clsValidatoin.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            }
            ; if (string.IsNullOrEmpty(txtFineFees.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtFineFees, "Fees con not by empty");

            }
            else
            {
                e.Cancel=false;
                errorProvider1.SetError(txtFineFees, "");

            }
            if (!clsValidatoin.IsNumber(txtFineFees.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtFineFees, "Invalid Number");

            }
            else
            {
                e.Cancel=false;
                errorProvider1.SetError(txtFineFees, "");

            }
        }

        private void frmDetainLicenseApplications_Activated(object sender, EventArgs e)
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
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
