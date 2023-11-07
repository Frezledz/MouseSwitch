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
using System.Windows.Forms;
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
            KeyConverter kc = new KeyConverter();
            Key testt;

            //http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/wpf/src/Base/System/Windows/Input/KeyConverter@cs/1305600/KeyConverter@cs
            testt = (Key)kc.ConvertFromString("CONTROL");
            List<Key> keys = new List<Key> {
                Key.S,
                testt
                
                
            };

            GlobalHotkey saveHotkey = new GlobalHotkey(modifiers,keys, MouseChange);
            HotkeyManager.AddHotkey(saveHotkey);
            System.Windows.Application.Current.MainWindow.Hide();
        }

        int speed = 15;
        public void MouseChange()
        {
            Debug.WriteLine("ads");
            speed = (speed+10)%20;
            Mouseswitcher.SystemParametersInfo(Mouseswitcher.SPI_SETMOUSESPEED, 0, uint.Parse(speed.ToString()), 0);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeyManager.ShutdownSystemHook();
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string[] Keystr = (ShortSTR.Text).Split(",");
            List<Key> keys = new List<Key>();
            foreach (string k in Keystr) {
                KeyConverter kc = new KeyConverter();
                Key kk = (Key)kc.ConvertFromString(k);

            }

           
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
