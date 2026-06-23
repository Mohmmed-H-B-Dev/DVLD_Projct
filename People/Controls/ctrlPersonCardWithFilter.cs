using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.People.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnSelectedPerson;
        public virtual void SelectedPerson(int personId)
        {
            if(OnSelectedPerson != null) OnSelectedPerson(personId);
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        private bool _ShowAddNewPerson= true;
        public bool ShowAddNewPerson
        {
            get{ return _ShowAddNewPerson; }
            set
            {
                _ShowAddNewPerson = value;
                btnAddNewPerson.Visible=_ShowAddNewPerson;
            }
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        int _PersonId;
        public int PersonId
        {
            get { return ctrlPersonCard1.PersonId; }
        }
        public void LoadPersonInfo(int PersonId)
        {
            cmbPersonFilter.SelectedIndex = 1;
            txtFilterText.Text=PersonId.ToString();
            _FindNew();


        }
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNew_Update frm=new frmAddNew_Update();
            frm.DataBack+=DataBackEvent;
            frm.ShowDialog();
        }

        public void DataBackEvent(object sender,int PersonId)
        {
            cmbPersonFilter.SelectedIndex=1;
            txtFilterText.Text=PersonId.ToString();
            _FindNew();
        }

        void _FindNew()
        {
            switch (cmbPersonFilter.Text)
            {

                case "Person ID":

                ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterText.Text));

                break;
            case "National No":

                ctrlPersonCard1.LoadPersonInfo(txtFilterText.Text);

                break;
            }
           
            if (OnSelectedPerson !=null && FilterEnabled)
                OnSelectedPerson(ctrlPersonCard1.PersonId);
        }
        private void btnSearchPerson_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we don`t continue because the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _FindNew();
        }

        private void txtFilterText_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterText.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterText, "This field is required");

            }
            else
            {
                errorProvider1.SetError(txtFilterText,null);

            }
        }

        private void txtFilterText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar ==(char)Keys.Enter)
            {
                btnSearchPerson.PerformClick();

            }

            if (e.KeyChar ==(char)Keys.Delete)
            {
                txtFilterText.Clear();

            }

            if(cmbPersonFilter.Text=="Person ID")
                e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterText.Text = "";
            txtFilterText.Focus();
        }
        public void FilterFocus()
        {
            txtFilterText.Focus();
        }
        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cmbPersonFilter.SelectedIndex=0;
            txtFilterText.Focus();

        }
    }
}
