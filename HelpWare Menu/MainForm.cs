using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TriggerBotApp
{
    public partial class MainForm : Form
    {
        private bool _triggerBotActive = false;
        private int _tapTime = 60; // Default Tap Time in milliseconds

        private System.Windows.Forms.Timer _timer;
        private int _r = 255, _g = 111, _b = 255; // Purple
        private int _rYellow = 255, _gYellow = 255, _bYellow = 85; // Yellow
        private int _colorTolerance = 80; // Default color tolerance

        private const int ZoneSize = 3;
        private Rectangle _grabZone;

        // Overlay form to show status
        private StatusOverlayForm _statusOverlay;

        // Socket variables
        private string _host = "localhost"; // Host for the socket connection
        private int _port = 65422; // Port number
        private TcpClient _client;
        private NetworkStream _stream;

        // Import keybd_event function from user32.dll
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const int KEYEVENTF_KEYUP = 0x0002;

        // Import functions for global hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const int HOTKEY_ID = 1;
        private const uint MOD_NONE = 0x0000; // No modifier keys
        private const uint VK_J = 0x4A; // Virtual Key for "J"

        // Import functions for window handling
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // GUI elements for mode selection and color tolerance
        private RadioButton rbtnNormalMode;
        private RadioButton rbtnBoostMode;
        private TrackBar trackBarColorTolerance; // TrackBar for color tolerance
        private Label lblColorToleranceValue;
        private Label lblHotkeyInfo;

        private ComboBox cmbColorSelection; // ComboBox for color selection

        private Label lblConnectionStatus;

        public MainForm()
        {
            InitializeComponent();

            // Initialisierung des Verbindungsstatus-Labels
            lblConnectionStatus = new Label
            {
                Text = "Warten auf Verbindung...",
                AutoSize = true,
                Location = new Point(10, 120),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblConnectionStatus);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 2; // Interval of 2ms
            _timer.Tick += Timer_Tick;

            // Initialize the status overlay form
            _statusOverlay = new StatusOverlayForm();
            _statusOverlay.Location = new Point(0, 0);
            _statusOverlay.Show();

            CheckBox chkToggleOverlay = new CheckBox
            {
                Text = "Show Status",
                Location = new Point(365, 90),
                AutoSize = true,
                Checked = true
            };
            chkToggleOverlay.CheckedChanged += ChkToggleOverlay_CheckedChanged;
            this.Controls.Add(chkToggleOverlay);

            // Initialize and configure the color selection ComboBox
            cmbColorSelection = new ComboBox
            {
                Location = new Point(100, 30),
                Size = new Size(75, 21)
            };

            // Add color options to ComboBox
            cmbColorSelection.Items.Add("Purple");
            cmbColorSelection.Items.Add("Yellow");
            cmbColorSelection.SelectedIndex = 0; // Default selection

            cmbColorSelection.SelectedIndexChanged += CmbColorSelection_SelectedIndexChanged;

            this.Controls.Add(cmbColorSelection);

            // Set the initial grab zone
            _grabZone = new Rectangle(
                (Screen.PrimaryScreen.Bounds.Width / 2 - ZoneSize),
                (Screen.PrimaryScreen.Bounds.Height / 2 - ZoneSize),
                ZoneSize * 2,
                ZoneSize * 2
            );

            // Register global hotkey
            RegisterGlobalHotKey();

            // Initialize and configure the hotkey info label
            lblHotkeyInfo = new Label
            {
                Text = "Toggle bot \"J\"",
                AutoSize = true,
                Location = new Point(10, 75),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblHotkeyInfo);

            // Initialize mode selection radio buttons
            rbtnNormalMode = new RadioButton
            {
                Text = "Normal Mode [ext]", // gesammter bildschirm
                Location = new Point(10, 100),
                AutoSize = true,
                Checked = true // Default mode
            };
            rbtnBoostMode = new RadioButton
            {
                Text = "Boost Mode [game]", // nur game fenster
                Location = new Point(10, 130),
                AutoSize = true
            };
            this.Controls.Add(rbtnNormalMode);
            this.Controls.Add(rbtnBoostMode);

            // Initialize and configure color tolerance trackbar
            trackBarColorTolerance = new TrackBar
            {
                Minimum = 20,
                Maximum = 205,
                Value = _colorTolerance,
                TickFrequency = 20,
                LargeChange = 1,
                SmallChange = 1,
                Location = new Point(10, 181),
                Size = new Size(200, 45)
            };
            trackBarColorTolerance.Scroll += TrackBarColorTolerance_Scroll;
            this.Controls.Add(trackBarColorTolerance);

            // Label to display the current color tolerance value
            lblColorToleranceValue = new Label
            {
                Text = $"Tolerance: {_colorTolerance}",
                AutoSize = true,
                Location = new Point(10, 160),
                ForeColor = Color.Black,
                Font = new Font("Arial", 10, FontStyle.Bold)
            };
            this.Controls.Add(lblColorToleranceValue);

            // Beim Start des Programms mit dem Server verbinden
            ConnectToServerAsync();
        }

        private async void ConnectToServerAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    _client = new TcpClient(_host, _port);
                    _stream = _client.GetStream();

                    // Erfolgreich verbunden, also GUI im Hauptthread aktualisieren
                    this.Invoke((Action)(() =>
                    {
                        lblConnectionStatus.Text = "Verbunden mit dem Server.";
                        lblConnectionStatus.ForeColor = Color.Green;
                    }));
                }
                catch (SocketException ex)
                {
                    // Fehler beim Verbinden, GUI im Hauptthread aktualisieren
                    this.Invoke((Action)(() =>
                    {
                        lblConnectionStatus.Text = $"Verbindung fehlgeschlagen: {ex.Message}";
                        lblConnectionStatus.ForeColor = Color.Red;
                    }));
                }
                catch (Exception ex)
                {
                    // Allgemeiner Fehler, GUI im Hauptthread aktualisieren
                    this.Invoke((Action)(() =>
                    {
                        lblConnectionStatus.Text = $"Fehler: {ex.Message}";
                        lblConnectionStatus.ForeColor = Color.Red;
                    }));
                }
            });
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_triggerBotActive)
            {
                SearchForColor();
            }
        }

        private void SearchForColor()
        {
            Bitmap bitmap = null;
            Rectangle searchZone;

            try
            {
                if (rbtnBoostMode.Checked)
                {
                    IntPtr hWnd = GetForegroundWindow();
                    RECT rect;

                    if (GetClientRect(hWnd, out rect))
                    {
                        Point point = new Point(rect.Left, rect.Top);
                        ClientToScreen(hWnd, ref point);

                        Rectangle clientRect = new Rectangle(point.X, point.Y, rect.Right - rect.Left, rect.Bottom - rect.Top);

                        if (clientRect.Width > 0 && clientRect.Height > 0)
                        {
                            bitmap = new Bitmap(clientRect.Width, clientRect.Height);

                            using (var g = Graphics.FromImage(bitmap))
                            {
                                g.CopyFromScreen(clientRect.Location, Point.Empty, clientRect.Size);
                            }

                            searchZone = new Rectangle(
                                clientRect.Width / 2 - _grabZone.Width / 2,
                                clientRect.Height / 2 - _grabZone.Height / 2,
                                _grabZone.Width,
                                _grabZone.Height
                            );
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Rectangle screenRect = Screen.PrimaryScreen.Bounds;

                    if (screenRect.Width > 0 && screenRect.Height > 0)
                    {
                        bitmap = new Bitmap(screenRect.Width, screenRect.Height);

                        using (var g = Graphics.FromImage(bitmap))
                        {
                            g.CopyFromScreen(screenRect.Location, Point.Empty, screenRect.Size);
                        }

                        searchZone = new Rectangle(
                            Screen.PrimaryScreen.Bounds.Width / 2 - _grabZone.Width / 2,
                            Screen.PrimaryScreen.Bounds.Height / 2 - _grabZone.Height / 2,
                            _grabZone.Width,
                            _grabZone.Height
                        );
                    }
                    else
                    {
                        return;
                    }
                }

                for (int x = searchZone.Left; x < searchZone.Right; x++)
                {
                    for (int y = searchZone.Top; y < searchZone.Bottom; y++)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);

                        if (IsColorMatch(pixelColor))
                        {
                            SendToServer("trigger");

                            string response = ReceiveFromServer();
                            if (response == "p")
                            {
                                MessageBox.Show("Signal empfangen: 'p'", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }

                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler bei der Farbsuche: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bitmap?.Dispose(); // Bitmap entsorgen um Speicher freizugeben
            }
        }

        private bool IsColorMatch(Color color)
        {
            int rDiff, gDiff, bDiff;
            double distance;

            if (_r == 255 && _g == 111 && _b == 255) // Purple
            {
                rDiff = color.R - _r;
                gDiff = color.G - _g;
                bDiff = color.B - _b;
                distance = Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
                return distance <= _colorTolerance;
            }
            else if (_r == 255 && _g == 255 && _b == 85) // Yellow
            {
                rDiff = color.R - _rYellow;
                gDiff = color.G - _gYellow;
                bDiff = color.B - _bYellow;
                distance = Math.Sqrt(rDiff * rDiff + gDiff * gDiff + bDiff * bDiff);
                return distance <= _colorTolerance;
            }

            return false;
        }

        private void CmbColorSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbColorSelection.SelectedItem.ToString())
            {
                case "Purple":
                    _r = 255;
                    _g = 111;
                    _b = 255;
                    break;

                case "Yellow":
                    _r = 255;
                    _g = 255;
                    _b = 85;
                    break;
            }
        }

        // Socket methods for server communication
        private void ConnectToServer()
        {
            try
            {
                _client = new TcpClient(_host, _port);
                _stream = _client.GetStream();
                MessageBox.Show("Verbunden mit dem Server.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Fehler beim Verbinden mit dem Server: {ex.Message} (Code: {ex.ErrorCode})", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblConnectionStatus.Text = "Verbindung zum Server fehlgeschlagen"; // Anzeigen des Verbindungsstatus in der GUI
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Allgemeiner Fehler: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblConnectionStatus.Text = "Allgemeiner Fehler bei der Verbindung"; // Anzeigen des Verbindungsstatus in der GUI
            }
        }


        private void SendToServer(string message)
        {
            if (_client?.Connected == true)
            {
                try
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    _stream.Write(data, 0, data.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Senden der Nachricht: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Keine Verbindung zum Server!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ReceiveFromServer()
        {
            if (_client?.Connected == true)
            {
                try
                {
                    byte[] buffer = new byte[256];
                    int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                    return Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Empfangen der Nachricht: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Keine Verbindung zum Server!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

        private void DisconnectFromServer()
        {
            try
            {
                if (_stream != null) _stream.Close();
                if (_client != null) _client.Close();
                lblConnectionStatus.Text = "Verbindung getrennt"; // Anzeige auf der GUI
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Trennen der Verbindung: {ex.Message}", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ToggleTriggerBot()
        {
            _triggerBotActive = !_triggerBotActive;
            btnToggle.Text = _triggerBotActive ? "Stop TriggerBot" : "Start TriggerBot";
            _statusOverlay.UpdateStatus(_triggerBotActive);
            if (_triggerBotActive)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        private void ChkToggleOverlay_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBox = sender as CheckBox;
            if (chkBox.Checked)
            {
                _statusOverlay.Show();
            }
            else
            {
                _statusOverlay.Hide();
            }
        }

        private void btnToggle_Click(object sender, EventArgs e)
        {
            ToggleTriggerBot();
        }

        private void TrackBarColorTolerance_Scroll(object sender, EventArgs e)
        {
            _colorTolerance = trackBarColorTolerance.Value;
            lblColorToleranceValue.Text = $"Tolerance: {_colorTolerance}";
        }

        private void RegisterGlobalHotKey()
        {
            RegisterHotKey(this.Handle, HOTKEY_ID, MOD_NONE, VK_J);
        }
    }
}
