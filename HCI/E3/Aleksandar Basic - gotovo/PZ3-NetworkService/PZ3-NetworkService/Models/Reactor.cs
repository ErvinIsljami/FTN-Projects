using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	[Serializable]
	public class Reactor : BindableBase
	{
		private int id;
		private string name;
		private double temperature;
		private ReactorType reactorType;
		private string image;
		private bool isDragged;

		public Reactor() { }

		public Reactor(int id, string name, ReactorType reactorType, string image)
		{
			Id = id;
			Name = name;
			ReactorType = reactorType;
			Image = image;
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

		public override string ToString()
		{
			return $"Reactor: {Name}";
		}
	}
}
