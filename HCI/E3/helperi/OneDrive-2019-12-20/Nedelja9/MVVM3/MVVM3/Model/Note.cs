using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM3.Model
{
    public class Note : ValidationBase
    {
        private string title;
        private string description;

        public string Title
        {
            get { return title; }
            set
            {
                if(title!=value)
                {
                    title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        protected override void ValidateSelf()
        {
            if(string.IsNullOrWhiteSpace(this.title))
            {
                this.ValidationErrors["Title"] = "Title is required.";
            }
            if (string.IsNullOrWhiteSpace(this.description))
            {
                this.ValidationErrors["Description"] = "Description cannot be empty.";
            }
        }
    }
}
