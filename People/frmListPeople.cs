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

namespace DVLD_With_MY_teatcher.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
        }
        private static   DataTable _dtAllPeople=clsPerson.GetAllPeople();
        //Only select column that you nead to show in dgvPeople.
        DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
        private void _RefreshPeopleList()
        {
            _dtAllPeople=clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");
            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text=dgvPeople.Rows.Count.ToString(); 

        }
        private void frmListPeople_Load(object sender, EventArgs e)
        {
dgvPeople.DataSource = _dtPeople;
            cmbListPeople.SelectedIndex = 0;
            lblRecordsCount.Text=dgvPeople.Rows.Count.ToString(); 
            if(dgvPeople.Rows.Count > 0)
            {
                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 100;


                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 100;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 150;

            }
        }

        private void txtFilterPeople_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //Map selected Filter to real Column Name
            switch (cmbListPeople.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset value in dgvPeople incase  List People is None ,,Or filter value is Empty
            if (txtFilterValue.Text.Trim()==""||cmbListPeople.Text=="None")
            {
                _dtPeople.DefaultView.RowFilter="";
                lblRecordsCount.Text=dgvPeople.Rows.Count.ToString();
                return;
            }
            if(FilterColumn =="PersonID")
            {
                //in this case we deal with integer not string.
                _dtPeople.DefaultView.RowFilter=string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            }else
            {
                _dtPeople.DefaultView.RowFilter=string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            }
          

            lblRecordsCount.Text=dgvPeople.Rows.Count.ToString();   
        }

        private void cmbListPeople_SelectedIndexChanged(object sender, EventArgs e)
        {
            //incase cmbListPeople is None we Make txtFilterPeople visible false.
            txtFilterValue.Visible=(cmbListPeople.Text!="None");
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text="";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterPeople_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cmbListPeople.Text== "Person ID")
                e.Handled= !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();   
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddNew_Update frm =new frmAddNew_Update();
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonId = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmAddNew_Update frm = new frmAddNew_Update(PersonId);
            frm.ShowDialog();
            _RefreshPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You sure you want to delete Person [ "+dgvPeople.CurrentRow.Cells[0].Value.ToString()+"].", "Cunfirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)==DialogResult.OK)
            {

                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Deleted Is Done in Successfully..", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeopleList();


                }
                else
                {
                    MessageBox.Show("Person was not deleted because t has data linked to it..", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet", "Not Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddNew_Update frm = new frmAddNew_Update();
            frm.ShowDialog();
            _RefreshPeopleList();
        }
    }
}
