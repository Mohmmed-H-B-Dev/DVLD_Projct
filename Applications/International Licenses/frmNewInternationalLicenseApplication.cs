using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
using DVLD_With_MY_teatcher.Globle;
using DVLD_With_MY_teatcher.Licenses;
using DVLD_With_MY_teatcher.Licenses.international_licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.International_Licenses
{
    public partial class frmNewInternationalLicenseApplication: Form
    {
        int _InternationalLicenseID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDrivingLicenseWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            llShowLicenseHistory.Enabled=(SelectedLicenseID!=-1);

            if (SelectedLicenseID==-1)
            {
                return;
            }
            lblLocalLicenseID.Text=SelectedLicenseID.ToString();
            if (ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.LicenseClass!=3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int ActiveInternationalLincense = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverID);

            if (ActiveInternationalLincense!=-1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternationalLincense.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled=true;
                _InternationalLicenseID=ActiveInternationalLincense;
                btnIssueLicense.Enabled=false;
                return;
            }


            btnIssueLicense.Enabled=true;
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text=clsFormat.ToShort(DateTime.Now);
            lblIssueDate.Text=clsFormat.ToShort(DateTime.Now);
            lblExpirationDate.Text=clsFormat.ToShort(DateTime.Now.AddYears(1));
            lblFees.Text=clsApplicationType.GetApplicationTypeInfoByID
                ((int)clsApplication.enApplicationType.NewInternationalLicense).Fees
                .ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }

            clsInternationalLicense internationalLicense = new clsInternationalLicense();

            internationalLicense.ApplicantPersonID=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            internationalLicense.ApplicationDate=DateTime.Now;
            internationalLicense.ApplicationStatus=clsApplication.enApplicationStatus.Completed;
            internationalLicense.LastStatusDate=DateTime.Now;
            internationalLicense.PaidFees=clsApplicationType.GetApplicationTypeInfoByID((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            internationalLicense.CreatedByUserID=clsGlobal.CurrentUser.UserId;


            internationalLicense.DriverID=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverID;
            internationalLicense.IssuedUsingLocalLicenseID=ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.LicenseID;
            internationalLicense.IssueDate=DateTime.Now;
            internationalLicense.ExpirationDate=DateTime.Now.AddYears(1);
            internationalLicense.CreatedByUserID=clsGlobal.CurrentUser.UserId;

            if (!internationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text=internationalLicense.ApplicationID.ToString();
            _InternationalLicenseID=internationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text=_InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + internationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueLicense.Enabled=false;
            ctrlDrivingLicenseWithFilter1.FilterEnabled=false;
            llShowLicenseInfo.Enabled=true;



        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm =
              new frmShowPersonLicensesHistory(ctrlDrivingLicenseWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void frmNewInternationalLicenseApplication_AutoValidateChanged(object sender, EventArgs e)
        {

        }

        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDrivingLicenseWithFilter1.TxtLicenseFocus();
        }
    }
}
