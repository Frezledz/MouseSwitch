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
                //これ別にkeyクラスにCTRLとかあるしいらなくない??
            };

            List<Key> keys = new List<Key> {
                Key.S,
                Key.LeftCtrl,
                Key.LeftShift
            };

            GlobalHotkey saveHotkey = new GlobalHotkey(modifiers,keys, MouseChange);
            HotkeyManager.AddHotkey(saveHotkey);
        }
        public void MouseChange()
        {
            Mouseswitcher.SystemParametersInfo(Mouseswitcher.SPI_SETMOUSESPEED, 0, uint.Parse("15"), 0);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeyManager.ShutdownSystemHook();
        }
    }
}
