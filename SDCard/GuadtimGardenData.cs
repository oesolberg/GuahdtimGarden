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
		private readonly bool _heatIsOn;
		private readonly bool _reservoirIsEmpty;
		private readonly bool _growPodIsEmpty;
		private readonly bool _growPodIsFull;
		private readonly double _humidity;
		private readonly double _temperature;
		private readonly bool _pumpOn;

		public GuadtimGardenData()
		{
			
		}

		public GuadtimGardenData(bool pumpOn, DateTime createdDateTime, double temperature, double humidity, bool heatIsOn, bool reservoirIsEmpty,bool growPodIsEmpty, bool growPodIsFull)
		{
			_pumpOn = pumpOn;
			_createdDateTime = createdDateTime;
			_heatIsOn= heatIsOn;
			_reservoirIsEmpty = reservoirIsEmpty;
			_growPodIsEmpty = growPodIsEmpty;
			_growPodIsFull = growPodIsFull;
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
		public bool GrowPodIsFull { get {return _growPodIsFull;} }
		public bool GrowPodIsEmpty { get {return _growPodIsEmpty;} }
		public bool ReservoirIsEmpty { get {return _reservoirIsEmpty;} }
		public bool HeaterIsOn { get { return _heatIsOn; } }
		public string CreateDataLine()
		{
			double truncatedTemperature = System.Math.Truncate(_temperature * 100) / 100;
			double truncatedHumidity= System.Math.Truncate(_humidity* 100) / 100;

			return _createdDateTime.ToString(DateTimeFormatString) + ", " +
			       truncatedTemperature.ToString(DoubleFormatString) + ", " +
			       truncatedHumidity.ToString(DoubleFormatString) + ", " +
			       HeaterIsOn + ", " +
			       PumpOn + ", " +
				   ReservoirIsEmpty + ", " +
				   GrowPodIsEmpty + ", " +
				   GrowPodIsFull ;
		}

	    public void UpdateWaterLevelData(IWaterLevelData waterLevelData)
	    {
	        throw new NotImplementedException();
	    }
	}
}
