using CryptographyHelperUtil.ViewModels;
using System.Windows;

namespace CryptographyHelperUtil
{
    public partial class MainWindow : Window
    {
        private CryptographyViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new CryptographyViewModel();
            this.DataContext = _viewModel;
        }
    }
}
