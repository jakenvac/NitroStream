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
        public Options()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.NitroStreamViewModel vm = this.DataContext as ViewModel.NitroStreamViewModel;
            if (vm != null)
            {
                if (string.IsNullOrEmpty(vm.ViewSettings.IPAddress) == false)
                {
                    Model.ViewSettings.Save(vm.configPath, vm.ViewSettings);
                    vm.InitiateRemotePlay();
                }
                else
                {
                    vm.WriteToLog("IP Address can't be empty");
                }
            }
        }

        private void TextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (string.IsNullOrEmpty(t.Text))
            {
                ViewModel.NitroStreamViewModel vm = this.DataContext as ViewModel.NitroStreamViewModel;
                if (vm != null)
                {
                    vm.WriteToLog("ERR: All fields must have a value.");
                    t.Text = "1";
                }
            }

        }
    }
}
