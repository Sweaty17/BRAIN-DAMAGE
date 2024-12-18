namespace TriggerBotApp
{
    partial class KeySelectorForm
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
            this.SuspendLayout();
            // 
            // KeySelectorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(200, 100);
            this.Name = "KeySelectorForm";
            this.Text = "Select Hold Key";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeySelectorForm_KeyDown);
            this.ResumeLayout(false);
        }
    }
}
