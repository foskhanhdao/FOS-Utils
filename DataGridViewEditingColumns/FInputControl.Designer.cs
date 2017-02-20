namespace FOS_Utils
{
    partial class FInputControl
    {
        
        private System.ComponentModel.IContainer components = null;

       
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        

        
        private void InitializeComponent()
        {
            this.inputControl = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // inputControl
            // 
            this.inputControl.Location = new System.Drawing.Point(0, 0);
            this.inputControl.Margin = new System.Windows.Forms.Padding(4);
            this.inputControl.Name = "inputControl";
            this.inputControl.Size = new System.Drawing.Size(155, 22);
            this.inputControl.TabIndex = 0;
            this.inputControl.Enter += new System.EventHandler(this.inputControl_Enter);
            this.inputControl.ImeModeChanged += new System.EventHandler(this.inputControl_ImeModeChanged);
            this.inputControl.Leave += new System.EventHandler(this.inputControl_Leave);
            this.inputControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.inputControl_KeyPress);
            this.inputControl.SizeChanged += new System.EventHandler(this.InputControl_SizeChanged);
            // 
            // FInputControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.inputControl);
            this.Font = new System.Drawing.Font("MS UI Gothic", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FInputControl";
            this.Size = new System.Drawing.Size(200, 188);
            this.ImeModeChanged += new System.EventHandler(this.FInputControl_ImeModeChanged);
            this.SizeChanged += new System.EventHandler(this.FInputControl_SizeChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        private System.Windows.Forms.TextBox inputControl;
    }
}
