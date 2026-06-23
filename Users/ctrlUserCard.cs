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

namespace DVLD_With_MY_teatcher.Users
{
    public partial class ctrlUserCard: UserControl
    {
        clsUser _User;
        int _UserID;
        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        
        public void LoadUserInf(int UserID)
        {
            _UserID=UserID;
            _User=clsUser.FindUserByUserID(UserID);

            if (_User==null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _FillUserInfo();
        }

        private void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonId);

            lbUserID.Text=_User.UserId.ToString();
            lbUserName.Text=_User.UserName;

            if (_User.IsActive) 

                lbIsActive.Text="Yas";

            else

                lbIsActive.Text="No";
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void _ResetUserInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();
            lbUserID.Text="[?????]";
            lbUserName.Text="[?????]";
            lbIsActive.Text="[?????]";
        }
    }
}
