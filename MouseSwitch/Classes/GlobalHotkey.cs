using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MouseSwitch.Classes
{
    public class GlobalHotkey
    {
        public List<ModifierKeys> Modifier { get; set; }
        public List<Key> Key { get; set; }
        public Action Callback { get; set; }
        public bool CanExecute { get; set; }
        
        public GlobalHotkey(List<ModifierKeys> modifier, List<Key> key, Action callback, bool canExecute = true)
        {
            Modifier = modifier; Key = key; Callback = callback; CanExecute = canExecute;

        }
    }
}
