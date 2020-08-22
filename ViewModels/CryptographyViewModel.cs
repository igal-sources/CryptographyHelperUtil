using CryptographyHelperUtil.Commands;
using CryptographyHelperUtil.Helpers;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace CryptographyHelperUtil.ViewModels
{
    public class CryptographyViewModel: ViewModelBase
    {
        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                if (_filePath == value) return;

                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        private string _decryptedText;
        public string DecryptedText
        {
            get { return _decryptedText; }
            set
            {
                if (_decryptedText == value) return;

                _decryptedText = value;
                OnPropertyChanged("DecryptedText");
            }
        }

        private string _decryptedSingleText = "Enter text to decrypt";
        public string DecryptedSingleText
        {
            get { return _decryptedSingleText; }
            set
            {
                if (_decryptedSingleText == value) return;

                _decryptedSingleText = value;
                OnPropertyChanged("DecryptedSingleText");
            }
        }

        private string _encryptSingleText = "Enter text to encrypt";
        public string EncryptSingleText
        {
            get { return _encryptSingleText; }
            set
            {
                if (_encryptSingleText == value) return;

                _encryptSingleText = value;
                OnPropertyChanged("EncryptSingleText");
            }
        }

        public CryptographyViewModel()
        {

        }

        private void OpenFileDialog()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".exe.config",
                Filter = "Config (.exe.config)|*.exe.config",
                AddExtension = true
            };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                var file = new FileInfo(dlg.FileName);
                FilePath = file.FullName;
            }
        }

        private void DecryptText()
        {
            DecryptedSingleText = CryptographyHelper.Decrypt(DecryptedSingleText);
        }

        private void EncryptText()
        {
            EncryptSingleText = CryptographyHelper.Encrypt(EncryptSingleText);
        }


        private void Decrypt()
        {
            if (!ValidateFile()) return;

            Configuration config = Utils.OpenConfigurationFile(_filePath);
            var appSettingsConnectionString = config.AppSettings.Settings["ConnectionString"];
            DecryptedText = CryptographyHelper.Decrypt(appSettingsConnectionString.Value);
        }

        private void Encrypt()
        {
            if (!ValidateFile()) return;

            Configuration config = Utils.OpenConfigurationFile(_filePath);
            var appSettingsConnectionString = config.AppSettings.Settings["ConnectionString"];

            var encryptedConnectionString = CryptographyHelper.Encrypt(appSettingsConnectionString.Value);
            appSettingsConnectionString.Value = encryptedConnectionString;

            foreach (ConnectionStringSettings connectionString in config.ConnectionStrings.ConnectionStrings)
            {
                encryptedConnectionString = CryptographyHelper.Encrypt(connectionString.ConnectionString);
                connectionString.ConnectionString = encryptedConnectionString;
            }

            config.Save(ConfigurationSaveMode.Full);
        }

        #region Commands

        private RelayCommand<object> _openFileDialogCommand;

        public ICommand OpenFileDialogCommand
        {
            get
            {
                if (_openFileDialogCommand == null)
                {
                    _openFileDialogCommand = new RelayCommand<object>(x => OpenFileDialog());
                }

                return _openFileDialogCommand;
            }
        }

        private RelayCommand<object> _decryptCommand;

        public ICommand DecryptCommand
        {
            get
            {
                if (_decryptCommand == null)
                {
                    _decryptCommand = new RelayCommand<object>(x => Decrypt());
                }

                return _decryptCommand;
            }
        }

        private RelayCommand<object> _decryptTextCommand;

        public ICommand DecryptTextCommand
        {
            get
            {
                if (_decryptTextCommand == null)
                {
                    _decryptTextCommand = new RelayCommand<object>(x => DecryptText());
                }

                return _decryptTextCommand;
            }
        }

        private RelayCommand<object> _encryptTextCommand;

        public ICommand EncryptTextCommand
        {
            get
            {
                if (_encryptTextCommand == null)
                {
                    _encryptTextCommand = new RelayCommand<object>(x => EncryptText());
                }

                return _encryptTextCommand;
            }
        }

        private RelayCommand<object> _encryptCommand;

        public ICommand EncryptCommand
        {
            get
            {
                if (_encryptCommand == null)
                {
                    _encryptCommand = new RelayCommand<object>(x => Encrypt());
                }

                return _encryptCommand;
            }
        }

        #endregion

        private bool ValidateFile()
        {
            if (string.IsNullOrEmpty(_filePath)) return false;

            if (!File.Exists(_filePath))
            {
                MessageBox.Show(string.Format("File {0} does not exist.", _filePath));
                return false;
            }

            return true;
        }
    }
}
