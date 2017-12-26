using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    public class StageViewModel : ViewModel
    {
        public int Time
        {
            get
            {
                return GetValue<int>();
            }
            set
            {
                SetValue(value);
            }
        }
    }
}
