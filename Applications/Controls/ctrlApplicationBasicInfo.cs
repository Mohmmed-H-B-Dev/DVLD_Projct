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
using DVLD_With_MY_teatcher.Globle;
using DVLD_With_MY_teatcher.Global__Classes;
using DVLD_With_MY_teatcher.People;

namespace DVLD_With_MY_teatcher.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo: UserControl
    {

        clsApplication _Application;
        int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }

        public void LoadApplicationInfo(int ApplicationID)
        {
            _Application=clsApplication.FindBaseApplication(ApplicationID);
            if (_Application==null)
            {

                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
                _FillApplicationType();
        }

        private void _FillApplicationType()
        {
            _ApplicationID=_Application.ApplicationID;
            lblApplicationID.Text=_ApplicationID.ToString();
            lblStatus.Text=_Application.StatusText;
            lblType.Text=_Application.ApplicationTypeInfo.ApplicationTitle;
            lblFees.Text=_Application.PaidFees.ToString();
            lblApplicant.Text=_Application.ApplicantFullName;
            lblDate.Text=clsFormat.ToShort(_Application.ApplicationDate);
            lblStatusDate.Text=clsFormat.ToShort(_Application.LastStatusDate);
            lblCreatedByUser.Text=_Application.CreatedByUserInfo.UserName;
        }
        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";

        }
        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(_Application.ApplicantPersonID);
            frm.ShowDialog();


            LoadApplicationInfo(_ApplicationID);
        }
    }
}
