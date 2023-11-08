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
        int index = 0;
        GlobalHotkey saveHotkey;
        List<int> speeds = new List<int> { 5, 15 };
        public MainWindow()
        {
            InitializeComponent();
            
            HotkeyManager.SetupSystemHook();
            List<ModifierKeys> modifiers = new List<ModifierKeys>
            {
                //これ別にkeyクラスにCTRLとかあるしいらなくない??
            };
            KeyConverter kc = new KeyConverter();

            //http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/wpf/src/Base/System/Windows/Input/KeyConverter@cs/1305600/KeyConverter@cs
            List<Key> keys = new List<Key> {
                Key.S,
                Key.LeftAlt
                
                
            };

            saveHotkey = new GlobalHotkey(modifiers,keys, MouseChange);
            HotkeyManager.AddHotkey(saveHotkey);
            System.Windows.Application.Current.MainWindow.Hide();
        }

        public void MouseChange()
        {
            index = (index+ 1)%speeds.Count;
            Mouseswitcher.SystemParametersInfo(Mouseswitcher.SPI_SETMOUSESPEED, 0, uint.Parse(speeds[index].ToString()), 0);
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HotkeyManager.ShutdownSystemHook();
        }


        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            
            string[] Keystr = (ShortSTR.Text).Split(",");
            KeyConverter kc = new KeyConverter();
            bool sucs = true;
            List<Key> keys = new List<Key>();
            List<ModifierKeys> modifiers = new List<ModifierKeys>
            {
                //これ別にkeyクラスにCTRLとかあるしいらなくない??
            };

            HotkeyManager.RemoveHotkey(saveHotkey);

            foreach (string key in Keystr)
            {

                Key kk;
                try
                {
                    kk = (Key)kc.ConvertFromString(key);
                    GlobalHotkey saveHotkey = new GlobalHotkey(modifiers, keys, MouseChange);
                    keys.Add(kk);
                }
                catch
                {
                    System.Windows.MessageBox.Show($"Your input {key} is invalid. Please make sure you don't have any misspelling.","What", MessageBoxButton.OK,MessageBoxImage.Error);
                    sucs = false;
                    break;
                }
            }
            saveHotkey = new GlobalHotkey(modifiers, keys, MouseChange);
            HotkeyManager.AddHotkey(saveHotkey);
            System.Windows.MessageBox.Show($"Your Hotkey has been changed to {ShortSTR.Text}","Wow",MessageBoxButton.OK,MessageBoxImage.Information);


        }


        private void ValueButton_Click(object sender, RoutedEventArgs e)
        {
            string[] Keystr = (ValueSTR.Text).Split(",");
            if(Keystr.Length == 0 ) {
                System.Windows.MessageBox.Show("You need to have at least 1 parameter.", "You lazy kid", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            bool sucs = true;
            List<int> tmpsp = new List<int>();

            foreach (string key in Keystr)
            {
                int val;
                if(int.TryParse(key, out val)) {
                    if (val<21&&val>0)
                    {
                        tmpsp.Add(val);
                    }
                    else
                    {
                        sucs = false;
                        System.Windows.MessageBox.Show($"Your input {key} is out of range. it needs to be greater than 0 and less than 21.", "Get better", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                }
                else
                {
                    sucs = false;
                    System.Windows.MessageBox.Show($"Your input {key} is invalid. It only supports integer (1 to 20).", "I don't understand", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
                }
            }
            if (sucs)
            {
                speeds.Clear();
                foreach(int vv in tmpsp)
                {
                    speeds.Add(vv);
                }
                System.Windows.MessageBox.Show($"Modify succeeded.", "success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
