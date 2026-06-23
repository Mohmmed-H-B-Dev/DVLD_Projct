using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Licenses
{
    public partial class frmShowPersonLicensesHistory: Form
    {

        int _PersonID=-1;
        public frmShowPersonLicensesHistory(int personID)
        {
            InitializeComponent();
            _PersonID=personID;
        }

        private void ctrlDriverLicenses1_Load(object sender, EventArgs e)
        {
            if (_PersonID!=-1)
            {
                ctrlPersonCardWithFilter1.FilterEnabled=false;
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlDriverLicenses1.LoadInfoWithPersonID(_PersonID);
            }else
            {
                ctrlPersonCardWithFilter1.FilterEnabled=true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnSelectedPerson(int obj)
        {
            _PersonID=obj;
            if (_PersonID==-1)
            {
                ctrlDriverLicenses1.Clear();
            }
            else
            {
                ctrlDriverLicenses1.LoadInfoWithPersonID(_PersonID);
            }
        }
    }
}
