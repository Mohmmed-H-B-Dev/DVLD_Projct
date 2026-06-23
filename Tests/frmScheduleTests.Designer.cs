namespace DVLD_With_MY_teatcher.Tests
{
    partial class frmScheduleTests
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctrlSchedule_Test1 = new DVLD_With_MY_teatcher.Tests.Controls.ctrlSchedule_Test();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ctrlSchedule_Test1
            // 
            this.ctrlSchedule_Test1.Location = new System.Drawing.Point(2, 0);
            this.ctrlSchedule_Test1.Name = "ctrlSchedule_Test1";
            this.ctrlSchedule_Test1.Size = new System.Drawing.Size(539, 709);
            this.ctrlSchedule_Test1.TabIndex = 0;
            this.ctrlSchedule_Test1.TestTypeID = DVLD_Buisness.clsTestType.enTestType.WrittenTest;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnClose.Image = global::DVLD_With_MY_teatcher.Properties.Resources.Close_32;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.Location = new System.Drawing.Point(15, 663);
            this.btnClose.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 36);
            this.btnClose.TabIndex = 167;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmScheduleTests
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 716);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.ctrlSchedule_Test1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "frmScheduleTests";
            this.Text = "frmScheduleTests";
            this.Load += new System.EventHandler(this.frmScheduleTests_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.ctrlSchedule_Test ctrlSchedule_Test1;
        private System.Windows.Forms.Button btnClose;
    }
}