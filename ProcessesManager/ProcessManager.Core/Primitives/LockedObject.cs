using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessManager.Core.Primitives
{
    public class LockedObject
    {
        bool _locked;

        Dictionary<string, object> _values = new Dictionary<string, object>();

        public bool Locked
        {
            get
            {
                return _locked;
            }
        }

        internal virtual void Lock()
        {
            _locked = true;
        }

        internal virtual void Unlock()
        {
            _locked = false;
        }

        protected void SetValue(string key, object value)
        {
            if (Locked)
                ThrowLocked();
            _values[key] = value;
        }

        protected object GetValue(string key)
        {
            return _values[key];
        }

        protected bool ContainsKey(string key)
        {
            return _values.ContainsKey(key);
        }

        protected Type TypeOfValue(string key)
        {
            return _values[key].GetType();
        }

        protected virtual void ThrowLocked()
        {
            throw new InvalidOperationException("Объект заблокирован");
        }
    }
}
