using DVLD_Buisness;
using DVLD_With_MY_teatcher.Globle;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Users
{
    
    
    public partial class frmChangePassword: Form
    {
        int _UserID;
        clsUser _User;
        string _TypeProcess;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID=UserID;
        }
        public frmChangePassword(clsUser user, string TypeProcess )
        {
            InitializeComponent();
            _TypeProcess=TypeProcess;
            _UserID =user.UserId;
            _User=user;
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtCurrentPassword, "Password Con`t by black..");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword,null);
            }
            //there is  need to compare the password with the database because we already have the user information in the
            //memory and we can compare it with the current password that the user entered
            if (clsUser.IsPasswordCorrect(_UserID, txtCurrentPassword.Text.Trim()))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password is Wrong..");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null); 
            }


        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text))
            {
                e.Cancel=true;
                errorProvider1.SetError(txtNewPassword, "New Password can`t be black..");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword,null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text!=txtNewPassword.Text)
            {
                e.Cancel=true;
                errorProvider1.SetError(txtConfirmPassword, " Password Confirmation does not match New Password..");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);

            }
        }

        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            if(_UserID<=0)
            {
                MessageBox.Show("Invalid User ID, can`t load user information.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            if(_User!=null&& _TypeProcess.ToLower()=="f_ch")
            {
                clsGlobal.CurrentUser=_User;
                txtCurrentPassword.Visible=false;
                label2.Visible=false;
                pictureBox5.Visible=false;
                txtCurrentPassword.Text = _User.Password;
            }
            clsUser  User= clsGlobal.CurrentUser;

            if (User == null)
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;

            }
            ctrlUserCard1.LoadUserInf(_UserID);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsUser _User = clsGlobal.CurrentUser;
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.Password=clsGlobal.ComputeHashing( txtNewPassword.Text.Trim());
            if (_User.ChangePassword())
                {
                    MessageBox.Show("Password Changed Successfully.",
                       "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clsGlobal.CurrentUser=_User;
                IsSuccessfulPasswordChanged=true;

                _ResetDefualtValues();
                this.Close();

            }
                else
                {
                    MessageBox.Show("An Erro Occured, Password did not change.",
                       "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

        }
        public bool IsSuccessfulPasswordChanged { get; private set; }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
