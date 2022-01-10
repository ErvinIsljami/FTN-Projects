using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ1.Models
{
    public class Image : BindableBase
    {
        private string _path;
        private string _title;
        private string _description;
        private string _timestampString;
        private string _owner;

        public Image() { }

        public Image(string path, string title, string description, string owner)
        {
            Path = path;
            Title = title;
            Description = description;
            TimestampString = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            Owner = owner;
        }

        public string Path
        {
            get { return _path; }
            set
            {
                if(_path != value)
                {
                    _path = value;
                    OnPropertyChanged(nameof(Path));
                }
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string TimestampString
        {
            get { return _timestampString; }
            set
            {
                if (_timestampString != value)
                {
                    _timestampString = value;
                    OnPropertyChanged(nameof(TimestampString));
                }
            }
        }

        public string Owner
        {
            get { return _owner; }
            set
            {
                if (_owner != value)
                {
                    _owner = value;
                    OnPropertyChanged(nameof(Owner));
                }
            }
        }
    }
}
