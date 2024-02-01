using System.Windows;
using System.Windows.Controls;
using Forms = System.Windows.Forms;

namespace Flow.Launcher.Plugin.TempCleaner
{
    public partial class TempCleanerSettings : UserControl
    {
        private readonly Settings _settings;

        public TempCleanerSettings(Settings settings)
        {
            InitializeComponent();
            _settings = settings;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewTempFolderPath.Text = _settings.TempFolderPath;
        }

        private void btnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            using var folderBrowserDialog = new Forms.FolderBrowserDialog();
            var dialogResult = folderBrowserDialog.ShowDialog();
            if (dialogResult == Forms.DialogResult.OK)
            {
                viewTempFolderPath.Text = viewTempFolderPath.Text + ";" + folderBrowserDialog.SelectedPath;
                _settings.TempFolderPath = viewTempFolderPath.Text;
                _settings.Save();
            }
        }

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            var textBox = sender as TextBox;
            viewTempFolderPath.Text = textBox.Text;
            _settings.TempFolderPath = viewTempFolderPath.Text;
            _settings.Save();
        }
    }
}