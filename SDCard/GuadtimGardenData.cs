using System;
using CommonInterfaces;
using Microsoft.SPOT;

namespace SDCard
{
	public class GuadtimGardenData : IGuadtimGardenData
	{
		private const string DateTimeFormatString = "yyyy-MM-dd hh:mm:ss";
		public const string DoubleFormatString = "N2";

		
		private readonly DateTime _createdDateTime;
		private readonly bool _heatOn;
		private readonly bool _reservoirEmpty;
		private readonly bool _growPodEmpty;
		private readonly bool _growPodFull;
		private readonly double _humidity;
		private readonly double _temperature;
		private readonly bool _pumpOn;

		public GuadtimGardenData()
		{
			
		}

		public GuadtimGardenData(bool pumpOn, DateTime createdDateTime, double temperature, double humidity, bool heatOn, bool reservoirEmpty,bool growPodEmpty, bool growPodFull)
		{
			_pumpOn = pumpOn;
			_createdDateTime = createdDateTime;
			_heatOn= heatOn;
			_reservoirEmpty = reservoirEmpty;
			_growPodEmpty = growPodEmpty;
			_growPodFull = growPodFull;
			_humidity = humidity;
			_temperature = temperature;
		}

		public DateTime GetTime()
		{
			return DateTime.Now;
		}

		public bool PumpOn { get { return _pumpOn; } }
		//public DateTime CreatedDateTime { get; }
		public DateTime CreatedDateTime { get { return _createdDateTime; } }
		public double Temperature { get { return _temperature; } }
		public double Humidity { get { return _humidity; } }
		public bool GrowPodFull { get {return _growPodFull;} }
		public bool GrowPodEmpty { get {return _growPodEmpty;} }
		public bool ReservoirEmpty { get {return _reservoirEmpty;} }
		public bool HeaterOn { get { return _heatOn; } }
		public string CreateDataLine()
		{
			double truncatedTemperature = System.Math.Truncate(_temperature * 100) / 100;
			double truncatedHumidity= System.Math.Truncate(_humidity* 100) / 100;

			return _createdDateTime.ToString(DateTimeFormatString) + ", " +
			       truncatedTemperature.ToString(DoubleFormatString) + ", " +
			       truncatedHumidity.ToString(DoubleFormatString) + ", " +
			       HeaterOn + ", " +
			       PumpOn + ", " +
				   ReservoirEmpty + ", " +
				   GrowPodEmpty + ", " +
				   GrowPodFull ;
		}

		
	}
}
