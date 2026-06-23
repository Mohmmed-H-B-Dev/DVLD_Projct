using DVLD_Buisness;
using DVLD_With_MY_teatcher.Tests.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Tests
{
    public partial class frmScheduleTests: Form
    {

        int _LocalDrivingLicenseApplicationID = -1;
        int _AppointmentTestID = -1;
        clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;

        public frmScheduleTests(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID, int AppointmentTestID=-1)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID=LocalDrivingLicenseApplicationID;
            _AppointmentTestID=AppointmentTestID;
            _TestTypeID=TestTypeID;
        }

        private void frmScheduleTests_Load(object sender, EventArgs e)
        {
            ctrlSchedule_Test1.TestTypeID=_TestTypeID;
            ctrlSchedule_Test1.LoadInfo(_LocalDrivingLicenseApplicationID, _AppointmentTestID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
