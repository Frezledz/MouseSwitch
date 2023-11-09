using MouseSwitch.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
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
using MessageBox = System.Windows.MessageBox;
using MouseSwitch.Properties;

namespace MouseSwitch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private static List<int>? STRtospd(string kstr)//ユーザーの速度指定をint型のlistに変換する
        {
            string[] Keystr = (kstr).Split(",");
            if (Keystr.Length == 0)
            {
                MessageBox.Show("You need to have at least 1 parameter.", "You lazy kid", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            List<int> tmpsp = new List<int>();

            foreach (string key in Keystr)
            {
                int val;
                if (int.TryParse(key, out val))
                {
                    if (val<21&&val>0)
                    {
                        tmpsp.Add(val);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show($"Your input {key} is out of range. it needs to be greater than 0 and less than 21.", "Get better", MessageBoxButton.OK, MessageBoxImage.Error);
                        return null;
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show($"Your input {key} is invalid. It only supports integer (1 to 20).", "I don't understand", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            return tmpsp;
        }
        int index = 0;
        GlobalHotkey saveHotkey;
        List<int> speeds = STRtospd(Settings.Default.spd);
        public MainWindow()
        {
            InitializeComponent();
            ValueSTR.Text=Settings.Default.spd;


            HotkeyManager.SetupSystemHook();
            List<ModifierKeys> modifiers = new List<ModifierKeys>
            {
                //これ別にkeyクラスにCTRLとかあるしいらなくない??
            };
            KeyConverter kc = new KeyConverter();
            //http://www.dotnetframework.org/default.aspx/4@0/4@0/DEVDIV_TFS/Dev10/Releases/RTMRel/wpf/src/Base/System/Windows/Input/KeyConverter@cs/1305600/KeyConverter@cs
            List<Key> keys = HotkeyManager.STRtokeys(Settings.Default.Key);
            saveHotkey = new GlobalHotkey(modifiers,keys, MouseChange);
            HotkeyManager.SetHotkey(saveHotkey);
            ShortSTR.Text=Settings.Default.Key;
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


        private void SubmitButton_Click(object sender, RoutedEventArgs e)//ショートカットを変更
        {
            List<Key>? a = HotkeyManager.STRtokeys(ShortSTR.Text);
            if (a!=null)
            {
                List<ModifierKeys> modifiers = new List<ModifierKeys> { };
                saveHotkey = new GlobalHotkey(modifiers, a, MouseChange);
                HotkeyManager.SetHotkey(saveHotkey);
                MessageBox.Show($"Your Hotkey has been changed to {ShortSTR.Text}", "Wow", MessageBoxButton.OK, MessageBoxImage.Information);
                Settings.Default.Key=ShortSTR.Text;
                Settings.Default.Save();

            }
        }


        private void ValueButton_Click(object sender, RoutedEventArgs e)//速度を変更
        {
            List<int>? values = STRtospd(ValueSTR.Text);
            if (values!=null)
            {
                speeds.Clear();
                foreach(int vv in values)
                {
                    speeds.Add(vv);
                }
                MessageBox.Show("Modify succeeded.", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                Settings.Default.spd=ValueSTR.Text;
                Settings.Default.Save();
            }
        }
    }
}
