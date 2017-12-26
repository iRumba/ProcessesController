using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StageViewModel : ViewModel
    {
        [JsonProperty]
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
