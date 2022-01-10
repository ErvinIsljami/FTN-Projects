using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkService.Models
{
    // INotifyPropertyChanged interface is used to notify clients, typically binding clients, that a property value has changed
    [Serializable]
    public class Road : INotifyPropertyChanged
    {
        private int _id;
        private string _label;
        private Type _type;
        private double _value;
        private string _shouldWarn;

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("Id");
                }
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (_label != value)
                {
                    _label = value;
                    RaisePropertyChanged("Label");
                }
            }
        }

        public Type Type
        {
            get { return _type; }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    RaisePropertyChanged("Type");
                }
            }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    // Call RaisePropertyChanged when the property is updated
                    _value = value;
                    RaisePropertyChanged("Value");
                }
            }
        }

        public string ShouldWarn
        {
            get { return _shouldWarn; }
            set
            {
                if (_shouldWarn != value)
                {
                    _shouldWarn = value;
                    RaisePropertyChanged("ShouldWarn");
                }
            }
        }

        public Road() { }

        public Road(int id, string label, Type type)
        {
            _id = id;
            _label = label;
            _type = type;
            _value = -1;
            _shouldWarn = "Transparent";
        }

        // Declare the PropertyChanged event
        // Method that will handle the PropertyChanged event raised when a property is changed on a component
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                // PropertyChangedEventArgs provide data for the PropertyChanged event
                // A PropertyChanged event is raised when a property is changed on a component
                // A PropertyChangedEventArgs object specifies the name of the property that changed 
                // (it provides the PropertyName property to get the name of the property that changed)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public override string ToString()
        {
            return $"ID: {Id}, L: {Label}";
        }
    }
}
