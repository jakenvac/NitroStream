using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Nitro_Stream.View
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : UserControl
    {

        private ViewModel.NitroStreamViewModel _Vm;

        public Options()
        {
            InitializeComponent();
            _Vm = this.DataContext as ViewModel.NitroStreamViewModel;
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_Vm != null)
            {
                if (string.IsNullOrEmpty(_Vm.ViewSettings.IPAddress) == false)
                {
                    Model.ViewSettings.Save(_Vm.configPath, _Vm.ViewSettings);
                    _Vm.InitiateRemotePlay();
                }
                else
                {
                    _Vm.WriteToLog("IP Address can't be empty");
                }
            }
        }

        private void TextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                if (_Vm != null)
                {
                    _Vm.WriteToLog("ERR: All fields must have a value.");
                    t.Text = "1";
                }
            }

        }

        private void SendMemPatch(object sender, RoutedEventArgs e)
        {
            if (_Vm != null)
            {
                _Vm.MemPatch();
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _Vm = this.DataContext as ViewModel.NitroStreamViewModel;
        }
    }
}
