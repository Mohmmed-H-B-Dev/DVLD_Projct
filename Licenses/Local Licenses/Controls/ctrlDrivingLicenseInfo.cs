using DVLD_Buisness;
using DVLD_With_MY_teatcher.Global__Classes;
using System.IO;
using System.Windows.Forms;

namespace DVLD_With_MY_teatcher.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDrivingLicenseInfo : UserControl
    {

        int _LicenseID = -1;
        clsLicense _License;
        public int LicenseID { get { return _LicenseID; } }

        public clsLicense SelectedLicenseInfo { get { return _License; } }

        public ctrlDrivingLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor==0)
                pbPersonImage.Image=DVLD_With_MY_teatcher.Properties.Resources.Male_512;
            else
                pbPersonImage.Image=DVLD_With_MY_teatcher.Properties.Resources.Female_512;


            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;
            if (ImagePath!="") 
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        public void LoadInfo(ref int LicenseID)
        {
            _LicenseID=LicenseID;
            _License =clsLicense.Find(_LicenseID);
            if (_License==null)
            {
                MessageBox.Show("Could Not Found License Id by : "+_LicenseID.ToString(), 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID=-1;
                LicenseID=-1;
                return;
            }
            lblLicenseID.Text=_License.LicenseID.ToString();
            lblClass.Text=_License.LicenseClassIfo.ClassName;
            lblFullName.Text=_License.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text=_License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text=_License.DriverInfo.PersonInfo.Gendor==0 ? "Male" : "Female";
            lblIssueDate.Text=clsFormat.ToShort(_License.IssueDate);
            lblIssueReason.Text=_License.IssueReasonText;
            lblNotes.Text= _License.Notes==""?"No Notes": _License.Notes;
            lblIsActive.Text=_License.IsActive ? "Yas" : "No";
            lblDateOfBirth.Text=clsFormat.ToShort( _License.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text=_License.DriverID.ToString();
            lblExpirationDate.Text=clsFormat.ToShort(_License.ExpirationDate);
            lblIsDetained.Text=_License.IsDetained ? "Yas" : "No";

            _LoadPersonImage();
        }
    }
}
