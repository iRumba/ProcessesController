﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProcessesManager.GUI.ViewModels
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        Dictionary<string, object> _values;
        Dictionary<string, string[]> _dependencies;

        public ObservableCollection<string> ValidationDetails { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsValid => Validate();

        //public string ValidationDetails => ValidationDetails();

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

        protected bool CanExecuteTrue(object parameter)
        {
            return true;
        }

        protected virtual bool Validate()
        {
            return true;
        }

        public string ValidationDetailsString
        {
            get
            {
                var message = string.Empty;
                foreach(var det in ValidationDetails)
                    message = $"{message}{det}\n";
                return message;
            }
        }

        protected void ShowValidationDetailsMessage()
        {
            MessageBox.Show(ValidationDetailsString, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
