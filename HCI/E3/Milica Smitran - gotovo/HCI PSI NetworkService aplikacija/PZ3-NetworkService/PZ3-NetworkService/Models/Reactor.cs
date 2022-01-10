using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	[Serializable]
	public class Reactor : BindableBase, IEditableObject
	{
		private int id;
		private string name;
		private double temperature;
		private ReactorType reactorType;
		private string image;
		private bool isDragged;
		// Used for implementing IEditableObject
		private Reactor tempReactor = null;
		private bool r_Editing = false;

		public Reactor()
		{
			Temperature = 0;
			IsDragged = false;
		}

		public int Id
		{
			get { return id; }
			set
			{
				SetField(ref id, value);
			}
		}
		public string Name
		{
			get { return name; }
			set
			{
				SetField(ref name, value);
			}
		}
		public double Temperature
		{
			get { return temperature; }
			set
			{
				SetField(ref temperature, value);
			}
		}
		public ReactorType ReactorType
		{
			get { return reactorType; }
			set
			{
				SetField(ref reactorType, value);
			}
		}
		public string Image
		{
			get { return image; }
			set
			{
				SetField(ref image, value);
			}
		}
		public bool IsDragged
		{
			get { return isDragged; }
			set
			{
				SetField(ref isDragged, value);
			}
		}

		// Implementing IEditableObject interface
		public void BeginEdit()
		{
			if (!r_Editing)
			{
				tempReactor = this.MemberwiseClone() as Reactor;
				r_Editing = true;
			}
		}
		public void CancelEdit()
		{
			if (r_Editing)
			{
				this.Id = tempReactor.Id;
				this.Name = tempReactor.Name;
				this.Temperature = tempReactor.Temperature;
				this.ReactorType = tempReactor.ReactorType;
				this.Image = tempReactor.Image;
				this.IsDragged = tempReactor.IsDragged;
				r_Editing = false;
			}
		}
		public void EndEdit()
		{
			if (r_Editing)
			{
				tempReactor = null;
				r_Editing = false;
			}
		}

		public override string ToString()
		{
			return $"Reactor: {Name}";
		}
	}
}
