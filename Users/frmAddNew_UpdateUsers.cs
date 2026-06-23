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

namespace DVLD_With_MY_teatcher.Users
{
    public partial class frmAddNew_UpdateUsers: Form
    {
        public enum enMode { AddNew=1,Update=2 }
        enMode _Mode;
        clsUser _User;
        int _UserID;

        public frmAddNew_UpdateUsers()
        {
            InitializeComponent();
            _Mode=enMode.AddNew;
        }

        public frmAddNew_UpdateUsers(int UserID)
        {
            InitializeComponent();
            _Mode=enMode.Update;
            _UserID=UserID;
        }


        private void _ResetDefaultValues()
        {
            if (_Mode==enMode.AddNew)
            {
                lbTitle.Text="Add New User";
                this.Text="Add New User";
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lbTitle.Text="Update User";
                this.Text="Update User";
                tpLoginInfo.Enabled = true;
                btnSave.Enabled=true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;
        }
        private void _LoadDate()
        {
            _User=clsUser.FindUserByUserID(_UserID);
            ctrlPersonCardWithFilter1.FilterEnabled=false;
            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            lblUserID.Text=_UserID.ToString();
            txtUserName.Text=_User.UserName;
            txtPassword.Text=_User.Password;
            txtConfirmPassword.Text=_User.Password;
            chkIsActive.Checked=_User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonId);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                MessageBox.Show("Some files are vailed .put the mouse over the red icon(s) to see the error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                   );
                return;

            }

            _User.UserName=txtUserName.Text.Trim();
            _User.Password=clsGlobal.ComputeHashing( txtPassword.Text.Trim());
            _User.IsActive=chkIsActive.Checked;
            _User.PersonId=ctrlPersonCardWithFilter1.PersonId;

            if (_User.Save())
            {

                lblUserID.Text=_User.UserId.ToString();
                lbTitle.Text="Update User";
                this.Text="Update User";
                _Mode=enMode.Update;
                ctrlPersonCardWithFilter1.FilterEnabled=false;
                txtPassword.Enabled=false;
                txtConfirmPassword.Enabled=false;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddNew_UpdateUsers_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void frmAddNew_UpdateUsers_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (_Mode==enMode.Update)
                _LoadDate();

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            ;

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
            ;

        }
        private void frmAddNew_UpdateUsers_Activated(object sender, EventArgs e)
        {

            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }
            if (_Mode==enMode.AddNew)
            {

                if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel=true;
                    errorProvider1.SetError(txtUserName, "this username is used by anther user.");
                    return;
                }
                else
                {
                    errorProvider1.SetError(txtUserName,"");

                }


            }
            else
            {
                if (_User.UserName!=txtUserName.Text.Trim())
                {

                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel=true;
                        errorProvider1.SetError(txtUserName, "This username is used by anther user.");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, "");
                    }
                }
            }
        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode==enMode.Update)
            {
                btnSave.Enabled=true;
                tpLoginInfo.Enabled=true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                txtConfirmPassword.Enabled=false;
                txtPassword.Enabled=false;
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonId!=-1)
            {
                if (clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonId))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {

                    btnSave.Enabled=true;
                    tpLoginInfo.Enabled=true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];

                    
                }
                 

            }
            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }

        }
    }
}
