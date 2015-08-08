﻿namespace Project.DvbIpTv.Setup.UpdateWolf424
{
    partial class WizardWelcomeDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WizardWelcomeDialog));
            this.pictureWelcome = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.labelWelcomeTitle = new System.Windows.Forms.Label();
            this.labelWelcomeText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWelcome)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureWelcome
            // 
            this.pictureWelcome.BackgroundImage = global::Project.DvbIpTv.Setup.UpdateWolf424.Properties.Resources.WizardSidePattern;
            this.pictureWelcome.Image = global::Project.DvbIpTv.Setup.UpdateWolf424.Properties.Resources.WizardSide;
            resources.ApplyResources(this.pictureWelcome, "pictureWelcome");
            this.pictureWelcome.Name = "pictureWelcome";
            this.pictureWelcome.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.buttonBack);
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.buttonCancel);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // buttonBack
            // 
            this.buttonBack.DialogResult = System.Windows.Forms.DialogResult.No;
            resources.ApplyResources(this.buttonBack, "buttonBack");
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.UseVisualStyleBackColor = true;
            // 
            // buttonNext
            // 
            this.buttonNext.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonNext, "buttonNext");
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // labelWelcomeTitle
            // 
            resources.ApplyResources(this.labelWelcomeTitle, "labelWelcomeTitle");
            this.labelWelcomeTitle.Name = "labelWelcomeTitle";
            // 
            // labelWelcomeText
            // 
            resources.ApplyResources(this.labelWelcomeText, "labelWelcomeText");
            this.labelWelcomeText.Name = "labelWelcomeText";
            // 
            // WizardWelcomeDialog
            // 
            this.AcceptButton = this.buttonCancel;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.buttonNext;
            this.Controls.Add(this.labelWelcomeTitle);
            this.Controls.Add(this.labelWelcomeText);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pictureWelcome);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WizardWelcomeDialog";
            this.Load += new System.EventHandler(this.WizardWelcomeDialog_Load);
            this.Shown += new System.EventHandler(this.WizardWelcomeDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureWelcome)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureWelcome;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelWelcomeTitle;
        private System.Windows.Forms.Label labelWelcomeText;
        private System.Windows.Forms.Button buttonBack;
    }
}