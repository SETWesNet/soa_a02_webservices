namespace WebServiceInterface
{
    partial class TextBoard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlMainPanel = new System.Windows.Forms.Panel();
            this.lblTextBoard = new System.Windows.Forms.Label();
            this.pnlMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMainPanel
            // 
            this.pnlMainPanel.Controls.Add(this.lblTextBoard);
            this.pnlMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainPanel.Location = new System.Drawing.Point(0, 0);
            this.pnlMainPanel.Name = "pnlMainPanel";
            this.pnlMainPanel.Size = new System.Drawing.Size(1001, 557);
            this.pnlMainPanel.TabIndex = 0;
            // 
            // lblTextBoard
            // 
            this.lblTextBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTextBoard.Location = new System.Drawing.Point(0, 0);
            this.lblTextBoard.Name = "lblTextBoard";
            this.lblTextBoard.Size = new System.Drawing.Size(1001, 557);
            this.lblTextBoard.TabIndex = 0;
            this.lblTextBoard.Text = "TextBoard";
            this.lblTextBoard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TextBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMainPanel);
            this.Name = "TextBoard";
            this.Size = new System.Drawing.Size(1001, 557);
            this.pnlMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlMainPanel;
        private System.Windows.Forms.Label lblTextBoard;
    }
}
