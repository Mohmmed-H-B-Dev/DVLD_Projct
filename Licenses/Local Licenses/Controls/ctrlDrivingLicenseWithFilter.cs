using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DVLD_Buisness;

namespace DVLD_With_MY_teatcher.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDrivingLicenseWithFilter: UserControl
    {
        public event Action<int> OnLicenseSelected;

        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> Handler = OnLicenseSelected;
            if (Handler!=null)
                Handler(LicenseID);
        }
        public ctrlDrivingLicenseWithFilter()
        {
            InitializeComponent();
        }

        bool _FilterEnabled = true;
        public bool FilterEnabled { 
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled= value;
                gbFilters.Enabled=_FilterEnabled;
            } 
        }

        int _LicenseID = -1;
       
        public int LicenseID { get { return ctrlDrivingLicenseInfo1.LicenseID; } }
        public clsLicense SelectedLicenseInfo { get { return ctrlDrivingLicenseInfo1.SelectedLicenseInfo; } }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text=LicenseID.ToString();
            ctrlDrivingLicenseInfo1.LoadInfo(ref LicenseID);
            _LicenseID=ctrlDrivingLicenseInfo1.LicenseID;
            if (OnLicenseSelected!=null && FilterEnabled)
                OnLicenseSelected(LicenseID);
           
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if(e.KeyChar == (int)Keys.Enter)
            {
                btnFind.PerformClick();
            }
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtLicenseID.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtLicenseID, "This Filed is required.");
            }else
            {
                
                errorProvider1.SetError(txtLicenseID,null);

            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we do not continue because the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                return;

            }
            _LicenseID=int.Parse( txtLicenseID.Text);
            LoadLicenseInfo(_LicenseID);

        }

        public void TxtLicenseFocus()
        {
            txtLicenseID.Focus();
        }
    }
}
