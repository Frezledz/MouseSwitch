using MouseSwitch.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Wpfbgtest1.Hotkeys;




namespace MouseSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            HotkeyManager.SetupSystemHook();
            List<ModifierKeys> modifiers = new List<ModifierKeys>
            {
                ModifierKeys.Control
            };

            List<Key> keys = new List<Key> {
                Key.S
            };

            GlobalHotkey saveHotkey = new GlobalHotkey(modifiers,keys, MouseChange);
            HotkeyManager.AddHotkey(saveHotkey);
            Application.Current.MainWindow.Hide();
        }

        int speed = 15;
        public void MouseChange()
        {
            speed = (speed+10)%20;
            Mouseswitcher.SystemParametersInfo(Mouseswitcher.SPI_SETMOUSESPEED, 0, uint.Parse(speed.ToString()), 0);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeyManager.ShutdownSystemHook();
        }
    }
}
