using System;
using System.Windows.Forms;

namespace TriggerBotApp
{
    public partial class KeySelectorForm : Form
    {
        public Keys SelectedKey { get; private set; }

        public KeySelectorForm()
        {
            InitializeComponent();
        }

        private void KeySelectorForm_KeyDown(object sender, KeyEventArgs e)
        {
            SelectedKey = e.KeyCode;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
