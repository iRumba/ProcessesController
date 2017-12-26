using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProcessesManager.GUI.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        Dictionary<string, object> _values;
        Dictionary<string, string[]> _dependencies;

        public event PropertyChangedEventHandler PropertyChanged;

        protected ViewModel()
        {
            _values = new Dictionary<string, object>();
            _dependencies = new Dictionary<string, string[]>();
            var props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach(var prop in props)
            {
                var dependencyAttr = prop.GetCustomAttribute<DependenciesAttribute>();
                if (dependencyAttr != null)
                    _dependencies[prop.Name] = dependencyAttr.DependentProperties;
            }
        }

        protected T GetValue<T>([CallerMemberName]string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return default(T);

            if (GetValue(name, out var res))
                return (T)res;
            else
                return default(T);
        }

        protected void SetValue(object value, [CallerMemberName]string name = "")
        {
            if (string.IsNullOrEmpty(name))
                throw new InvalidOperationException("SetValue error");

            if (!GetValue(name, out var current) || current != value)
                UnsafeSetValue(name, value);
        }

        bool GetValue(string name, out object value)
        {
            if (_values.ContainsKey(name))
            {
                value = _values[name];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        void UnsafeSetValue(string name, object value)
        {
            _values[name] = value;
            OnPropertyChanged(name);
            if (_dependencies.ContainsKey(name))
                foreach (var depName in _dependencies[name])
                    OnPropertyChanged(depName);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public sealed class DependenciesAttribute : Attribute
    {
        public string[] DependentProperties { get; }
        public DependenciesAttribute(params string[] names)
        {
            DependentProperties = names;
        }
    }
}
