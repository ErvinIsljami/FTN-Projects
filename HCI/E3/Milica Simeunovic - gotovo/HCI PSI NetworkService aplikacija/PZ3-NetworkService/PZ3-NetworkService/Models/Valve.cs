using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	[Serializable]
	public class Valve : BindableBase, IEditableObject
	{
		private int id;
		private string name;
		private double pressureInMp;
		private ValveType valveType;
		private string image;
		private bool isDragged;
		// Used for implementing IEditableObject
		private Valve tempValve = null;
		private bool v_Editing = false;

		public Valve() { }

		public Valve(int id, ValveType valveType, string name, string image)
		{
			Id = id;
			PressureInMp = 0;
			ValveType = valveType;
			Name = name;
			Image = image;
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
			set { SetField(ref name, value); }
		}
		public double PressureInMp
		{
			get { return pressureInMp; }
			set
			{
				SetField(ref pressureInMp, value);
			}
		}
		public ValveType ValveType
		{
			get { return valveType; }
			set
			{
				SetField(ref valveType, value);
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
			if (!v_Editing)
			{
				tempValve = this.MemberwiseClone() as Valve;
				v_Editing = true;
			}
		}
		public void CancelEdit()
		{
			if (v_Editing)
			{
				this.Id = tempValve.Id;
				this.Name = tempValve.Name;
				this.PressureInMp = tempValve.PressureInMp;
				this.ValveType = tempValve.ValveType;
				this.Image = tempValve.Image;
				this.IsDragged = tempValve.IsDragged;
				v_Editing = false;
			}
		}
		public void EndEdit()
		{
			if (v_Editing)
			{
				tempValve = null;
				v_Editing = false;
			}
		}

		public override string ToString()
		{
			return $"Valve: {Name}";
		}
	}
}
