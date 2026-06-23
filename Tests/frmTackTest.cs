using DVLD_Buisness;
using DVLD_With_MY_teatcher.Globle;
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
    public partial class frmTackTest: Form
    {



        private int _AppointmentID;
        private clsTestType.enTestType _TestType=clsTestType.enTestType.VisionTest;

        private int _TestID = -1;
        private clsTest _Test;
        public frmTackTest(int AppointmentID,clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _TestType=TestType;
            _AppointmentID=AppointmentID;


        }

        private void frmTackTest_Load(object sender, EventArgs e)
        {
            ctrlSecheduledTest1.TestTypeID=_TestType;
            ctrlSecheduledTest1.LoadInfo(_AppointmentID);

            if (ctrlSecheduledTest1.TestAppointmentID==-1)
                btnSave.Enabled=false;
            else
                btnSave.Enabled=true;

            int TestID = ctrlSecheduledTest1.TestID;
            if (TestID!=-1)
            {
                _Test=clsTest.Find(TestID);
                if (_Test.TestResult)
                    rbPass.Checked=true;
                else
                    rbFail.Checked=true;

                txtNotes.Text=_Test.Notes;

                lblUserMessage.Enabled=true;
                rbFail.Enabled=false;
                rbPass.Enabled=false;
                btnSave.Enabled=false;
                txtNotes.Enabled=false;

            }else
                _Test = new clsTest();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                      "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.No
             )
            {
                return;
            }

            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID=clsGlobal.CurrentUser.UserId;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    
}
