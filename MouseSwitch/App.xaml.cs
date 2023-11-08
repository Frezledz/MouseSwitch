using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace MouseSwitch
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        MainWindow Wnd = null;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //アイコンの取得
            var icon = GetResourceStream(new Uri("logo.ico", UriKind.Relative)).Stream;
            //右クリックメニューを追加する
            var menu = new System.Windows.Forms.ContextMenuStrip();
            menu.Items.Add("Settings", null, Setting_Click);
            menu.Items.Add("Exit",null,Exit_click);
            //タスクトレイにアイコンを追加する
            _notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Visible = true,
                Icon = new System.Drawing.Icon(icon),
                Text = "MouseSwitcher",
                ContextMenuStrip = menu
            };
            Task.Run(() => {
            });
        }

        private void ShowMainWindow()
        {
            if (Wnd==null)
            {
                Wnd = new MainWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                Wnd.Show();

                Wnd.Closing += (s, e) =>
                {
                    Wnd.Hide();
                    e.Cancel = true;
                };
            }
            else
            {
                Wnd.Show();
            }
        }


        //右クリックメニュー用の関数
        private void Setting_Click(object sender, EventArgs e)
        {
            ShowMainWindow();
        }
        private void Exit_click(object sender,EventArgs e)
        {
            Shutdown();
        }
    }
}
