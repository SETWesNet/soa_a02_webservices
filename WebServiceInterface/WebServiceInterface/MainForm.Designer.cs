namespace WebServiceInterface
{
    partial class MainForm
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
            this.drpdwnWebServices = new System.Windows.Forms.ComboBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grpboxMain = new System.Windows.Forms.GroupBox();
            this.lblWebService = new System.Windows.Forms.Label();
            this.lblMethods = new System.Windows.Forms.Label();
            this.drpdwnMethods = new System.Windows.Forms.ComboBox();
            this.flwParameters = new System.Windows.Forms.FlowLayoutPanel();
            this.grpboxParamters = new System.Windows.Forms.GroupBox();
            this.richtxtReturnValue = new System.Windows.Forms.RichTextBox();
            this.lblReturnValue = new System.Windows.Forms.Label();
            this.grpboxMain.SuspendLayout();
            this.grpboxParamters.SuspendLayout();
            this.SuspendLayout();
            // 
            // drpdwnWebServices
            // 
            this.drpdwnWebServices.FormattingEnabled = true;
            this.drpdwnWebServices.Location = new System.Drawing.Point(10, 45);
            this.drpdwnWebServices.Name = "drpdwnWebServices";
            this.drpdwnWebServices.Size = new System.Drawing.Size(310, 28);
            this.drpdwnWebServices.TabIndex = 0;
            // 
            // grpboxMain
            // 
            this.grpboxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxMain.Controls.Add(this.lblReturnValue);
            this.grpboxMain.Controls.Add(this.richtxtReturnValue);
            this.grpboxMain.Controls.Add(this.grpboxParamters);
            this.grpboxMain.Controls.Add(this.lblMethods);
            this.grpboxMain.Controls.Add(this.drpdwnMethods);
            this.grpboxMain.Controls.Add(this.lblWebService);
            this.grpboxMain.Controls.Add(this.drpdwnWebServices);
            this.grpboxMain.Location = new System.Drawing.Point(13, 5);
            this.grpboxMain.Name = "grpboxMain";
            this.grpboxMain.Size = new System.Drawing.Size(648, 685);
            this.grpboxMain.TabIndex = 1;
            this.grpboxMain.TabStop = false;
            // 
            // lblWebService
            // 
            this.lblWebService.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebService.AutoSize = true;
            this.lblWebService.Location = new System.Drawing.Point(6, 22);
            this.lblWebService.Name = "lblWebService";
            this.lblWebService.Size = new System.Drawing.Size(98, 20);
            this.lblWebService.TabIndex = 1;
            this.lblWebService.Text = "Web Service";
            // 
            // lblMethods
            // 
            this.lblMethods.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMethods.AutoSize = true;
            this.lblMethods.Location = new System.Drawing.Point(322, 22);
            this.lblMethods.Name = "lblMethods";
            this.lblMethods.Size = new System.Drawing.Size(71, 20);
            this.lblMethods.TabIndex = 3;
            this.lblMethods.Text = "Methods";
            // 
            // drpdwnMethods
            // 
            this.drpdwnMethods.Enabled = false;
            this.drpdwnMethods.FormattingEnabled = true;
            this.drpdwnMethods.Location = new System.Drawing.Point(326, 45);
            this.drpdwnMethods.Name = "drpdwnMethods";
            this.drpdwnMethods.Size = new System.Drawing.Size(310, 28);
            this.drpdwnMethods.TabIndex = 2;
            // 
            // flwParameters
            // 
            this.flwParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flwParameters.AutoScroll = true;
            this.flwParameters.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwParameters.Location = new System.Drawing.Point(6, 25);
            this.flwParameters.Name = "flwParameters";
            this.flwParameters.Size = new System.Drawing.Size(614, 267);
            this.flwParameters.TabIndex = 4;
            this.flwParameters.WrapContents = false;
            // 
            // grpboxParamters
            // 
            this.grpboxParamters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxParamters.Controls.Add(this.flwParameters);
            this.grpboxParamters.Location = new System.Drawing.Point(10, 79);
            this.grpboxParamters.Name = "grpboxParamters";
            this.grpboxParamters.Size = new System.Drawing.Size(626, 298);
            this.grpboxParamters.TabIndex = 5;
            this.grpboxParamters.TabStop = false;
            this.grpboxParamters.Text = "Parameters";
            // 
            // richtxtReturnValue
            // 
            this.richtxtReturnValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richtxtReturnValue.Location = new System.Drawing.Point(10, 403);
            this.richtxtReturnValue.Name = "richtxtReturnValue";
            this.richtxtReturnValue.ReadOnly = true;
            this.richtxtReturnValue.Size = new System.Drawing.Size(626, 276);
            this.richtxtReturnValue.TabIndex = 6;
            this.richtxtReturnValue.Text = "Return Value Here";
            // 
            // lblReturnValue
            // 
            this.lblReturnValue.AutoSize = true;
            this.lblReturnValue.Location = new System.Drawing.Point(6, 380);
            this.lblReturnValue.Name = "lblReturnValue";
            this.lblReturnValue.Size = new System.Drawing.Size(121, 20);
            this.lblReturnValue.TabIndex = 7;
            this.lblReturnValue.Text = "Return Value(s)";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 702);
            this.Controls.Add(this.grpboxMain);
            this.Name = "MainForm";
            this.Text = "Web Service Interface";
            this.grpboxMain.ResumeLayout(false);
            this.grpboxMain.PerformLayout();
            this.grpboxParamters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox drpdwnWebServices;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox grpboxMain;
        private System.Windows.Forms.Label lblMethods;
        private System.Windows.Forms.ComboBox drpdwnMethods;
        private System.Windows.Forms.Label lblWebService;
        private System.Windows.Forms.FlowLayoutPanel flwParameters;
        private System.Windows.Forms.GroupBox grpboxParamters;
        private System.Windows.Forms.Label lblReturnValue;
        private System.Windows.Forms.RichTextBox richtxtReturnValue;
    }
}

