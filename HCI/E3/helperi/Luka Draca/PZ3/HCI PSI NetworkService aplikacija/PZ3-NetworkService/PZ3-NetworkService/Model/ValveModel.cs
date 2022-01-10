using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Model
{
    public class ValveModel : ValidationBase
    {
        private int id;
        private string name;
        private double value;
        private Type type=new Model.Type(null,null);
        private DateTime time = new DateTime();

        public ValveModel()
        {

        }

        public ValveModel(ValveModel vm)
        {
            id = vm.Id;
            name = vm.Name;
            value = vm.Value;
            time = vm.Time;
            type = new Type(vm.Type.Name, vm.Type.ImgUri);
        }
        
        public int Id
        {
            get => id;
            set
            {
                if(id!=value)
                {
                    id = value;
                    base.OnPropertyChanged("Id");
                }
            }

        }

        public string Name
        {
            get => name;
            set
            {
                if(name!=value)
                {
                    name = value;
                    base.OnPropertyChanged("Name");
                }
            }
        }

        public double Value
        {
            get => value;
            set
            {
                if(this.value!=value)
                {
                    this.value = value;
                    base.OnPropertyChanged("Value");
                }
            }
        }

        public Type Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    base.OnPropertyChanged("Type");
                }
            }
        }

        public DateTime Time
        {
            get => time;
            set
            {
                if (time != value)
                {
                    time = value;
                    base.OnPropertyChanged("Time");
                }
            }

        }

        protected override void ValidateSelf()
        {
            if (string.IsNullOrEmpty(name))
            {
                this.ValidationErrors["Name"] = "Name cannot be null/empty!";
            }
            if(id < 0 )
            {
                
                this.ValidationErrors["Id"] = "Id must be greater than 0!";

            }
            else
            {
                if (DataBase.Valve_MainStorage.ContainsKey(id))
                    this.ValidationErrors["Id"] = "Id already exists!";
            }
            
            if(type.ImgUri == null)
            {
                this.ValidationErrors["Value"] = "Must select Type!";
            }
        }
    }
}
