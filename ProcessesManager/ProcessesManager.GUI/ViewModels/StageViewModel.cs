﻿using Newtonsoft.Json;

namespace ProcessesManager.GUI.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StageViewModel : ViewModel
    {
        [JsonProperty]
        public int Time
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        protected override bool Validate()
        {
            var res = true;
            ValidationDetails.Clear();

            if (Time < 1)
            {
                res = false;
                ValidationDetails.Add("Время этапа должно быть больше 0");
            }

            return res;
        }
    }
}
