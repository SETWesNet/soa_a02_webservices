﻿namespace WebServiceInterface
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
            this.grpBoxMethodDocumentation = new System.Windows.Forms.GroupBox();
            this.txtBoxMethodDescription = new System.Windows.Forms.RichTextBox();
            this.txtbrdStatus = new WebServiceInterface.TextBoard();
            this.grdviewResponse = new System.Windows.Forms.DataGridView();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblReturnValue = new System.Windows.Forms.Label();
            this.grpboxParamters = new System.Windows.Forms.GroupBox();
            this.flwParameters = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMethods = new System.Windows.Forms.Label();
            this.drpdwnMethods = new System.Windows.Forms.ComboBox();
            this.lblWebService = new System.Windows.Forms.Label();
            this.grpboxMain.SuspendLayout();
            this.grpBoxMethodDocumentation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdviewResponse)).BeginInit();
            this.grpboxParamters.SuspendLayout();
            this.SuspendLayout();
            // 
            // drpdwnWebServices
            // 
            this.drpdwnWebServices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpdwnWebServices.FormattingEnabled = true;
            this.drpdwnWebServices.Location = new System.Drawing.Point(10, 45);
            this.drpdwnWebServices.Name = "drpdwnWebServices";
            this.drpdwnWebServices.Size = new System.Drawing.Size(350, 28);
            this.drpdwnWebServices.TabIndex = 0;
            this.drpdwnWebServices.SelectionChangeCommitted += new System.EventHandler(this.drpdwnWebServices_SelectionChangeCommitted);
            // 
            // grpboxMain
            // 
            this.grpboxMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxMain.Controls.Add(this.grpBoxMethodDocumentation);
            this.grpboxMain.Controls.Add(this.txtbrdStatus);
            this.grpboxMain.Controls.Add(this.grdviewResponse);
            this.grpboxMain.Controls.Add(this.btnSend);
            this.grpboxMain.Controls.Add(this.lblReturnValue);
            this.grpboxMain.Controls.Add(this.grpboxParamters);
            this.grpboxMain.Controls.Add(this.lblMethods);
            this.grpboxMain.Controls.Add(this.drpdwnMethods);
            this.grpboxMain.Controls.Add(this.lblWebService);
            this.grpboxMain.Controls.Add(this.drpdwnWebServices);
            this.grpboxMain.Location = new System.Drawing.Point(14, 5);
            this.grpboxMain.Name = "grpboxMain";
            this.grpboxMain.Size = new System.Drawing.Size(744, 948);
            this.grpboxMain.TabIndex = 1;
            this.grpboxMain.TabStop = false;
            // 
            // grpBoxMethodDocumentation
            // 
            this.grpBoxMethodDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBoxMethodDocumentation.Controls.Add(this.txtBoxMethodDescription);
            this.grpBoxMethodDocumentation.Location = new System.Drawing.Point(0, 85);
            this.grpBoxMethodDocumentation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpBoxMethodDocumentation.Name = "grpBoxMethodDocumentation";
            this.grpBoxMethodDocumentation.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpBoxMethodDocumentation.Size = new System.Drawing.Size(732, 131);
            this.grpBoxMethodDocumentation.TabIndex = 11;
            this.grpBoxMethodDocumentation.TabStop = false;
            this.grpBoxMethodDocumentation.Text = "Method Documentation";
            // 
            // txtBoxMethodDescription
            // 
            this.txtBoxMethodDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxMethodDescription.Location = new System.Drawing.Point(10, 31);
            this.txtBoxMethodDescription.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBoxMethodDescription.Name = "txtBoxMethodDescription";
            this.txtBoxMethodDescription.ReadOnly = true;
            this.txtBoxMethodDescription.Size = new System.Drawing.Size(714, 90);
            this.txtBoxMethodDescription.TabIndex = 0;
            this.txtBoxMethodDescription.Text = "";
            // 
            // txtbrdStatus
            // 
            this.txtbrdStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtbrdStatus.Location = new System.Drawing.Point(213, 626);
            this.txtbrdStatus.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtbrdStatus.Name = "txtbrdStatus";
            this.txtbrdStatus.Size = new System.Drawing.Size(310, 122);
            this.txtbrdStatus.TabIndex = 10;
            this.txtbrdStatus.Text = "Retrieving Response, Please Wait...";
            this.txtbrdStatus.Visible = false;
            // 
            // grdviewResponse
            // 
            this.grdviewResponse.AllowUserToAddRows = false;
            this.grdviewResponse.AllowUserToDeleteRows = false;
            this.grdviewResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdviewResponse.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdviewResponse.Location = new System.Drawing.Point(10, 517);
            this.grdviewResponse.Name = "grdviewResponse";
            this.grdviewResponse.ReadOnly = true;
            this.grdviewResponse.RowTemplate.Height = 28;
            this.grdviewResponse.Size = new System.Drawing.Size(716, 349);
            this.grdviewResponse.TabIndex = 9;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(10, 871);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(728, 72);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblReturnValue
            // 
            this.lblReturnValue.AutoSize = true;
            this.lblReturnValue.Location = new System.Drawing.Point(6, 494);
            this.lblReturnValue.Name = "lblReturnValue";
            this.lblReturnValue.Size = new System.Drawing.Size(121, 20);
            this.lblReturnValue.TabIndex = 7;
            this.lblReturnValue.Text = "Return Value(s)";
            // 
            // grpboxParamters
            // 
            this.grpboxParamters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpboxParamters.Controls.Add(this.flwParameters);
            this.grpboxParamters.Location = new System.Drawing.Point(0, 224);
            this.grpboxParamters.Name = "grpboxParamters";
            this.grpboxParamters.Size = new System.Drawing.Size(732, 275);
            this.grpboxParamters.TabIndex = 5;
            this.grpboxParamters.TabStop = false;
            this.grpboxParamters.Text = "Parameters";
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
            this.flwParameters.Size = new System.Drawing.Size(720, 242);
            this.flwParameters.TabIndex = 4;
            // 
            // lblMethods
            // 
            this.lblMethods.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMethods.AutoSize = true;
            this.lblMethods.Location = new System.Drawing.Point(372, 22);
            this.lblMethods.Name = "lblMethods";
            this.lblMethods.Size = new System.Drawing.Size(71, 20);
            this.lblMethods.TabIndex = 3;
            this.lblMethods.Text = "Methods";
            // 
            // drpdwnMethods
            // 
            this.drpdwnMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpdwnMethods.FormattingEnabled = true;
            this.drpdwnMethods.Location = new System.Drawing.Point(376, 45);
            this.drpdwnMethods.Name = "drpdwnMethods";
            this.drpdwnMethods.Size = new System.Drawing.Size(350, 28);
            this.drpdwnMethods.TabIndex = 2;
            this.drpdwnMethods.SelectedIndexChanged += new System.EventHandler(this.drpdwnMethods_SelectedIndexChanged);
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 965);
            this.Controls.Add(this.grpboxMain);
            this.Name = "MainForm";
            this.Text = "Web Service Interface";
            this.grpboxMain.ResumeLayout(false);
            this.grpboxMain.PerformLayout();
            this.grpBoxMethodDocumentation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdviewResponse)).EndInit();
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
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.DataGridView grdviewResponse;
        private TextBoard txtbrdStatus;
        private System.Windows.Forms.GroupBox grpBoxMethodDocumentation;
        private System.Windows.Forms.RichTextBox txtBoxMethodDescription;
    }
}

