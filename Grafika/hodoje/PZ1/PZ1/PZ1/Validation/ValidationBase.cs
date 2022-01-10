using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PZ1.Validation
{
    public abstract class ValidationBase : BindableBase
    {
        [XmlIgnore]
        public ValidationErrors ValidationErrors { get; set; }
        [XmlIgnore]
        public bool IsValid { get; private set; }

        protected ValidationBase()
        {
            this.ValidationErrors = new ValidationErrors();
        }

        protected abstract void ValidateSelf();

        public void Validate()
        {
            this.ValidationErrors.Clear();
            this.ValidateSelf();
            this.IsValid = this.ValidationErrors.IsValid;
            this.OnPropertyChanged(nameof(IsValid));
            this.OnPropertyChanged(nameof(ValidationErrors));
        }
    }
}
