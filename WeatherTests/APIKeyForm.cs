using System;
using System.Windows.Forms;
using Weather;

namespace WeatherTests
{
    public partial class APIKeyForm : Form
    {
        public APIKey APIKey { get; set; }

        public APIKeyForm()
        {
            InitializeComponent();
        }

        private void APIKeyForm_Load(object sender, EventArgs e)
        {
            Visible = true;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            APIKey.Value = textBox_apiKey.Text;

            Close();
        }
    }
}