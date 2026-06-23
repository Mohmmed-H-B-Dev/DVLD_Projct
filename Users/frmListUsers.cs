using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Users
{
    public partial class frmListUsers: Form
    {
        private DataTable _dtAllUsers;
        public frmListUsers()
        {
            InitializeComponent();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog(Owner);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNew_UpdateUsers frm = new frmAddNew_UpdateUsers(Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
            frmUsersList_Load(null, null);
        }
       
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddNew_UpdateUsers frm = new frmAddNew_UpdateUsers();
            frm.ShowDialog(Owner);
            frmUsersList_Load(null, null);
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddNew_UpdateUsers frm = new frmAddNew_UpdateUsers();
            frm.ShowDialog(Owner);
            frmUsersList_Load(null, null);
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Screen is Not  implemented","Confirm",MessageBoxButtons.OK,MessageBoxIcon.Information);

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Screen is Not  implemented", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void frmUsersList_Load(object sender, EventArgs e)
        {
            _dtAllUsers=clsUser.GetAllUsers();
            dgvUsers.DataSource=_dtAllUsers;
            cbFilterBy.SelectedIndex=0;
            lblRecordsCount.Text=dgvUsers.Rows.Count.ToString();

            if (dgvUsers.Rows.Count>0)
            {

                dgvUsers.Columns[0].HeaderText="User ID";
                dgvUsers.Columns[0].Width=110;

                dgvUsers.Columns[1].HeaderText="Person ID";
                dgvUsers.Columns[1].Width=120;

                dgvUsers.Columns[2].HeaderText="Full Name";
                dgvUsers.Columns[2].Width=350;

                dgvUsers.Columns[3].HeaderText="User Name";
                dgvUsers.Columns[3].Width=120;

                dgvUsers.Columns[4].HeaderText="Is Active";
                dgvUsers.Columns[4].Width=120;
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsUser.DeleteUser(Convert.ToInt32(dgvUsers.CurrentRow.Cells[0].Value)))
        { MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmUsersList_Load(null, null);
        }

            else
                MessageBox.Show("User is not deleted due to data connected to it.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);





           
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text=="Is Active")
            {
                cbIsActive.Visible=true;
                txtFilterValue.Visible=false;
                cbIsActive.SelectedIndex=0;
                cbIsActive.Focus();
            }
            else
            {
                txtFilterValue.Visible=(cbFilterBy.Text!="None");
                cbIsActive.Visible=false;

                txtFilterValue.Text="";
                txtFilterValue.Focus();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
             string FilterValue = cbIsActive.Text.Trim();

          
            if(FilterValue=="Yas")
                FilterValue="1";
            else if (FilterValue=="No")
                FilterValue="0";

            if (FilterValue=="All")
                _dtAllUsers.DefaultView.RowFilter="";

           else if(!string.IsNullOrEmpty(FilterValue))
            {
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter=string.Format("[{0}]={1}", FilterColumn, FilterValue);
            }
            lblRecordsCount.Text=dgvUsers.Rows.Count.ToString();


        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            string FilterValue = txtFilterValue.Text.Trim();
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn="UserID";
                    break;

                case "Person ID":
                    FilterColumn="PersonID";
                    break;

                case "UserName":
                    FilterColumn="UserName";
                    break;

                case "Full Name":
                    FilterColumn="FullName";
                    break;

                case "Is Active":
                    FilterColumn="IsActive";
                    break;

                default:
                    FilterColumn="None";
                    break;

            }

            if (FilterValue==""||FilterColumn=="None")
            {
                _dtAllUsers.DefaultView.RowFilter="";
                lblRecordsCount.Text=dgvUsers.Rows.Count.ToString();
                return;

            }

            if (FilterColumn!="FullName"&& FilterColumn!="UserName"&&!string.IsNullOrEmpty(txtFilterValue.Text))
                _dtAllUsers.DefaultView.RowFilter=string.Format("[{0}]={1} ", FilterColumn, FilterValue);
            else
                _dtAllUsers.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'", FilterColumn, FilterValue);

            lblRecordsCount.Text=dgvUsers.Rows.Count.ToString();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text=="User ID"||cbFilterBy.Text=="Person ID")
                e.Handled=!char.IsDigit(e.KeyChar) &&!char.IsControl(e.KeyChar);
        }

    }
}
