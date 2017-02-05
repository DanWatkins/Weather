namespace Weather.Client.Desktop.Windows.ViewModel
{
    internal class SettingsViewModel
    {
        public string ZipCode
        {
            get
            {
                return Properties.Settings.Default.ZipCode;
            }
            set
            {
                Properties.Settings.Default.ZipCode = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}