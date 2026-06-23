using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Applications.Local_Driving_License
{
    public partial class frmLocalDrivingLicenseApplicationInfo: Form
    {
        int _localDrivingAppID = -1;

        public frmLocalDrivingLicenseApplicationInfo(int localDrivingAppID)
        {
            InitializeComponent();
            _localDrivingAppID=localDrivingAppID;
        }

        private void frmLocalDrivingLicenseApplicationInfo_Load(object sender, EventArgs e)
        {
            ctrlLocalDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_localDrivingAppID);
        }
    }
}
