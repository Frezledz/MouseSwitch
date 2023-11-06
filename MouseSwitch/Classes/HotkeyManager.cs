using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MouseSwitch.Classes
{
    internal class HotkeyManager
    {
    
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private static LowLevelKeyboardProc LowLevelProc = HookCallback;


        private static List<GlobalHotkey> Hotkeys { get; set; }

        private const int WH_KEYBOARD_LL = 13;

        private static IntPtr HookID = IntPtr.Zero;
        public static bool IsHookSetup { get; set; }

        static HotkeyManager()
        {
            Hotkeys = new List<GlobalHotkey>();
        }
        public static void SetupSystemHook()
        {
            if (!IsHookSetup)
            {

                HookID =SetHook(LowLevelProc);
                IsHookSetup = true;
            }
        }
        public static void ShutdownSystemHook()
        {
            if (IsHookSetup)
            {
                UnhookWindowsHookEx(HookID);
                IsHookSetup = false;

            }
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule currentModule = currentProcess.MainModule)
                {
                    return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(currentModule.ModuleName), 0);
                }
            }
        }

        public static void AddHotkey(GlobalHotkey hotkey)
        {
            Hotkeys.Add(hotkey);
        }
        public static void RemoveHotkey(GlobalHotkey hotkey)
        {
            Hotkeys.Remove(hotkey);
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode>=0)
            {
                //check hotkeys
                //Use keyboard class
                foreach (GlobalHotkey hotkey in Hotkeys)
                {
                    bool executable = true;
                    //Check Modifier Keys
                    /*foreach (ModifierKeys modifier in hotkey.Modifier)
                    {
                        if (!(Keyboard.Modifiers==modifier&&hotkey.CanExecute))
                        {
                            executable=false;
                            break;
                        }
                    }*/
                    //Check keys
                    foreach (Key key in hotkey.Key)
                    {
                        if (!executable)
                        {
                            break;
                        }
                        if (!(Keyboard.IsKeyDown(key)&&hotkey.CanExecute))
                        {
                            executable=false; break;
                        }
                    }
                    //Invoke using bool executable
                    if (executable)
                    {
                        //Debug.WriteLine("succeeded");
                        hotkey.Callback?.Invoke();
                    }
                    else
                    {
                        //Debug.WriteLine("fail");
                    }

                }
            }

            //Hotkey will be scanned
            return CallNextHookEx(HookID, nCode, wParam, lParam);
            //I don't understand how this works bro
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}

