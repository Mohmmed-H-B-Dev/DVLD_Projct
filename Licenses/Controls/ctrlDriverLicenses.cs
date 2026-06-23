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
using DVLD_With_MY_teatcher.Licenses.Local_Licenses;
using DVLD_With_MY_teatcher.Licenses.international_licenses;

namespace DVLD_With_MY_teatcher.Licenses.Controls
{
    public partial class ctrlDriverLicenses: UserControl
    {
        int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtInternationalLicensesHistory;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }
        private void _LoadLocalLicensesInfo()
        {
            _dtDriverLocalLicensesHistory=clsDriver.GetLocalLicenses(_DriverID);
            dgvLocalLicensesHistory.DataSource=_dtDriverLocalLicensesHistory;
            lblLocalLicensesRecords.Text=dgvLocalLicensesHistory.Rows.Count.ToString();


            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 110;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 110;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 270;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;

            }
        }
      
        private void _LoadInternationalLicenseInfo()
        {

            _dtInternationalLicensesHistory = clsDriver.GetInternationalLicenses(_DriverID);


            dgvInternationalLicensesHistory.DataSource = _dtInternationalLicensesHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();

            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 110;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 110;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 110;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 110;

            }
        }
        public void LoadInfo(int DriverID)
        {
            _Driver=clsDriver.FindByDriverID(DriverID);
            
            if (_Driver==null)
            {
                MessageBox.Show("There is not driver with Id : "+ DriverID.ToString(), "No Driver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID=DriverID;
            _LoadLocalLicensesInfo();
            _LoadInternationalLicenseInfo();
        }
        public void LoadInfoWithPersonID(int PersonID)
        {
            _Driver=clsDriver.FindByPersonID(PersonID);
           
            if (_Driver==null)
            {
                MessageBox.Show("There is not driver Linked with Person with Id : "+ _DriverID.ToString(), "No Driver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID=_Driver.DriverID;
            _LoadLocalLicensesInfo( );
            _LoadInternationalLicenseInfo( );
        }

        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtInternationalLicensesHistory.Clear();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();

        }

        private void InternationalLicenseHistorytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}

