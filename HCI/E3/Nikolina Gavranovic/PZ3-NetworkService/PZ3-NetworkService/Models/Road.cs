using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	[Serializable]
	public class Road : BindableBase
	{
		private int id;
		private string name;
		private double traffic;
		private RoadType roadType;
		private string image;
		private bool isDragged;

		public Road() { }

		public Road(int id, string name, RoadType roadType, string image)
		{
			Id = id;
			Name = name;
			RoadType = roadType;
			Image = image;
			Traffic = 0;
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
		public double Traffic
		{
			get { return traffic; }
			set
			{
				SetField(ref traffic, value);
			}
		}
		public RoadType RoadType
		{
			get { return roadType; }
			set
			{
				SetField(ref roadType, value);
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
			return $"Road: {Name}";
		}
	}
}
