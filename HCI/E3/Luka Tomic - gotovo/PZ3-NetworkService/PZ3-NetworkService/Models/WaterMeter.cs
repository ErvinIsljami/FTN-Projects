using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PZ3_NetworkService.Models
{
	[Serializable]
	public class WaterMeter : BindableBase
	{
		private int id;
		private string name;
		private double consumption;
		private WaterMeterType waterMeterType;
		private string image;
		private bool isDragged;

		public WaterMeter() { }

		public WaterMeter(int id, string name, WaterMeterType waterMeterType, string image)
		{
			Id = id;
			Name = name;
			WaterMeterType = waterMeterType;
			Image = image;
			Consumption = 0;
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
		public double Consumption
		{
			get { return consumption; }
			set
			{
				SetField(ref consumption, value);
			}
		}
		public WaterMeterType WaterMeterType
		{
			get { return waterMeterType; }
			set
			{
				SetField(ref waterMeterType, value);
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
			return $"WaterMeter: {Name}";
		}
	}
}
