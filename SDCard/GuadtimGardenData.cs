using System;
using Microsoft.SPOT;

namespace SDCard
{
	public class GuadtimGardenData : IGuadtimGardenData
	{
		private const string DateTimeFormatString = "yyyy-MM-dd hh:mm:ss";
		public const string DoubleFormatString = "N2";

		private readonly bool _isWatering;
		private readonly DateTime _loggedDateTime;
		private readonly bool _isWarming;
		private readonly double _humidity;
		private readonly double _temperature;

		public GuadtimGardenData(bool isWatering, DateTime loggedDateTime, double temperature, double humidity, bool isWarming)
		{
			_isWatering = isWatering;
			_loggedDateTime = loggedDateTime;
			_isWarming = isWarming;
			_humidity = humidity;
			_temperature = temperature;
		}

		public DateTime GetTime()
		{
			return DateTime.Now;
		}

		public bool IsWatering { get { return _isWatering; } }
		public DateTime LoggedDateTime { get { return _loggedDateTime; } }
		public double Temperature { get { return _temperature; } }
		public double Humidity { get { return _humidity; } }
		public bool IsWarming { get { return _isWarming; } }
		public string CreateDataLine()
		{
			double truncatedTemperature = System.Math.Truncate(_temperature * 100) / 100;
			double truncatedHumidity= System.Math.Truncate(_humidity* 100) / 100;

			return _loggedDateTime.ToString(DateTimeFormatString) + ", " +
			       truncatedTemperature.ToString(DoubleFormatString) + ", " +
			       truncatedHumidity.ToString(DoubleFormatString) + ", " +
			       IsWarming + ", " +
			       IsWatering;
		}

		
	}
}
