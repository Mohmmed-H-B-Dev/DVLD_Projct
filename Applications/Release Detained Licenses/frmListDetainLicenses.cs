using DVLD_Buisness;
using DVLD_With_MY_teatcher.Licenses;
using DVLD_With_MY_teatcher.Licenses.Detain_Licenses;
using DVLD_With_MY_teatcher.Licenses.Local_Licenses;
using DVLD_With_MY_teatcher.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.Release_Detained_Licenses
{
    public partial class frmListDetainLicenses: Form
    {

        DataTable _dtListDetainedLicenses;
        public frmListDetainLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainLicenses_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex=0;
            _dtListDetainedLicenses=clsDetainedLicense.GetAllDetainedLicenses();

            dgvDetainedLicenses.DataSource=_dtListDetainedLicenses;
            lblTotalRecords.Text=dgvDetainedLicenses.Rows.Count.ToString();
            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 80;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 90;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 160;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 110;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 110;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 80;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 330;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 120;

            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";


            switch (cbFilterBy.Text) 
            {
                case "Detain ID":
                    FilterColumn="DetainID";
                    break;

                case "Is Released":
                    FilterColumn="IsReleased";
                    break;
                case "National No.":
                    FilterColumn="NationalNo";
                    break;
                case "Full Name" :
                    FilterColumn="FullName";
                    break;
                case "Release Application ID":
                    FilterColumn="ReleaseApplicationID";
                    break;
                default:
                    FilterColumn="None";
                    break;


            }

            if(txtFilterValue.Text.Trim() ==""|| FilterColumn=="None")
            {
                _dtListDetainedLicenses.DefaultView.RowFilter="";
                lblTotalRecords.Text=dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterColumn=="DetainID"||FilterColumn=="ReleaseApplicationID")
                _dtListDetainedLicenses.DefaultView.RowFilter=string.Format("[{0}] ={1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtListDetainedLicenses.DefaultView.RowFilter=string.Format("[{0}] Like '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblTotalRecords.Text=dgvDetainedLicenses.Rows.Count.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if(cbFilterBy.Text=="Is Released")
            {
                cbIsReleased.Visible=true;
                txtFilterValue.Visible=false;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex=0;
            }else
            {
                txtFilterValue.Focus();
                txtFilterValue.Visible=(cbFilterBy.Text!="None");
                cbIsReleased.Visible=false;
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;


            switch (FilterValue) 
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;


            }

            if (FilterValue=="All")
                _dtListDetainedLicenses.DefaultView.RowFilter="";
            else
                _dtListDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblTotalRecords.Text=dgvDetainedLicenses.Rows.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text=="Release Application ID"||cbFilterBy.Text=="Detain ID")
                e.Handled= !char.IsDigit(e.KeyChar)&& !char.IsControl(e.KeyChar);

        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;
            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);

        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled=!(bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplications frm = new frmDetainLicenseApplications();
            frm.ShowDialog();
            frmListDetainLicenses_Load(null, null);

        }
    }
}
