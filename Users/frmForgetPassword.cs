using DVLD_Buisness;
using DVLD_Project.Global__Classes.Messages_Service;
using DVLD_With_MY_teatcher.Global__Classes.Messages_Service;
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
    public partial class frmForgetPassword : Form
    {
        EmailService emailService = new EmailService();

        SMSMessage smsMessageService = new SMSMessage();
        clsUser _User;
        clsPerson _Person;
        public frmForgetPassword()
        {
            InitializeComponent();
            _Person=new clsPerson();
            _User=new clsUser();
        }

        private void SendMessage(string Message)
        {
            if(txtUserNameOrNationalNo.Text.Trim().Length==10)
            {
              //  SMSMessage.
            }
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            _User=clsUser.ResetPasswordAndfindUser(txtUserNameOrNationalNo.Text.Trim());
            if(_User==null)
            {
                MessageBox.Show("No user found with this username or national number.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
               
                return;
            }
            if (rbtnEmail.Checked)
            {
                emailService.SendMessage(_Person.Email, " We work to renew  your password... " );
            }else if (rbtnSMS.Checked)
            {
             
                _Person=clsPerson.Find(_User.PersonId);
                smsMessageService.SendMessage(_Person.Phone, " We work to renew  your password... ");
            }

            btnCheck.Enabled=true;
          


        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (_User!=null&&(smsMessageService.IsSendSuccessful||emailService.IsSendSuccessful))
            {

                if(txtCode.Text.Trim()!="1234")
                {
                    MessageBox.Show("Invalid code. Please enter the correct code sent to your email or phone.", "Invalid Code", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //When we forget password and we want to change it
                //we need to open the change password form and pass the user information to it and then
                //we can change the password and update it in the database
                //and mast send the (f_ch) to the change password and we say i forget Password
                frmChangePassword frmChangePassword = new frmChangePassword(_User, "f_ch");

                frmChangePassword.ShowDialog();

                if (frmChangePassword.IsSuccessfulPasswordChanged)
                {
                    MessageBox.Show("Your password has been changed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clsGlobal.CurrentUser = null;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Password change was not successful. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void rbtnEmail_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnEmail.Checked)
            {
                rbtnSMS.Checked = false;
                btnSend.Enabled = true;
            }
        }

        private void rbtnSMS_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnSMS.Checked)
            {
                rbtnEmail.Checked = false;
                btnSend.Enabled = true;
            }

            
        }
    }
}
