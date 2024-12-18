using System;
using System.Windows.Forms;

namespace TriggerBotApp
{
    public partial class KeyCaptureForm : Form
    {
        private Action<Keys> _keyCapturedCallback;

        public KeyCaptureForm(Action<Keys> keyCapturedCallback)
        {
            InitializeComponent();
            _keyCapturedCallback = keyCapturedCallback;
        }

        private void KeyCaptureForm_KeyDown(object sender, KeyEventArgs e)
        {
            _keyCapturedCallback?.Invoke(e.KeyCode);
            this.Close();
        }

        private void KeyCaptureForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // KeyCaptureForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "KeyCaptureForm";
            this.Load += new System.EventHandler(this.KeyCaptureForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyCaptureForm_KeyDown);
            this.ResumeLayout(false);

        }
    }
}
