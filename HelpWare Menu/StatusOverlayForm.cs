using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TriggerBotApp
{
    public partial class StatusOverlayForm : Form
    {
        private Label lblStatus;

        // Importiere Funktionen von user32.dll
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x00080000;
        private const int WS_EX_TRANSPARENT = 0x00000020;
        private const int LWA_COLORKEY = 0x00000001;
        private const int LWA_ALPHA = 0x00000002;

        private struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        public StatusOverlayForm()
        {
            InitializeComponent();

            // Manuelle Initialisierung des Formulars
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.Manual;
            this.TopMost = true;
            this.ClientSize = new Size(200, 50); // Setze die Größe des Formulars

            // Setze das Formular auf transparent
            int style = GetWindowLong(this.Handle, GWL_EXSTYLE);
            SetWindowLong(this.Handle, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT);

            this.BackColor = Color.Black;
            this.TransparencyKey = Color.Black; // Setze die Transparenzfarbe auf Schwarz

            // Initialisiere das Status-Label
            lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.White,
                BackColor = Color.Transparent, // Hintergrund des Labels transparent
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(lblStatus);

            // Setze die Fenster-Transparenz
            SetTransparent();
        }

        private void SetTransparent()
        {
            BLENDFUNCTION blend = new BLENDFUNCTION
            {
                BlendOp = 0,
                BlendFlags = 0,
                SourceConstantAlpha = 255, // Vollständig undurchsichtig
                AlphaFormat = 1
            };

            Point ptSrc = new Point(0, 0);
            Point ptDst = new Point(0, 0);
            Size size = new Size(this.Width, this.Height);

            UpdateLayeredWindow(this.Handle, IntPtr.Zero, ref ptDst, ref size, IntPtr.Zero, ref ptSrc, 0, ref blend, LWA_COLORKEY | LWA_ALPHA);
        }

        public void UpdateStatus(bool isActive)
        {
            lblStatus.Text = $"TriggerBot: {(isActive ? "On" : "Off")}";
        }

        private void StatusOverlayForm_Load(object sender, EventArgs e)
        {

        }

        public void SetStatusText(string text, Color color)
        {
            if (lblStatus != null)
            {
                lblStatus.Text = text;
                lblStatus.ForeColor = color;
            }
        }

    }
}
