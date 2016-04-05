using System;
using System.Threading;
using CommonInterfaces;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;


namespace WaterLevel
{
	public class WaterLevelSensor
	{
		private readonly AnalogInput _reservoirSensorEmpty;
		private readonly AnalogInput _growpoolSensorEmpty;
		private readonly AnalogInput _growpoolSensorFull;


		public WaterLevelSensor()
		{
			_reservoirSensorEmpty = new AnalogInput(AnalogChannels.ANALOG_PIN_A5);
			_growpoolSensorEmpty = new AnalogInput(AnalogChannels.ANALOG_PIN_A4);
			_growpoolSensorFull = new AnalogInput(AnalogChannels.ANALOG_PIN_A3);
		}

		public WaterLevelSensor(Cpu.AnalogChannel reservoirChannel, Cpu.AnalogChannel growPoolEmptyChannel, Cpu.AnalogChannel growPoolFullChannel)
		{
			_reservoirSensorEmpty = new AnalogInput(reservoirChannel);
			_growpoolSensorEmpty = new AnalogInput(growPoolEmptyChannel);
			_growpoolSensorFull = new AnalogInput(growPoolFullChannel);
		}

		//Read the waterlevel and return a WaterLevelDataStruct
		public IWaterLevelData GetWaterLevelStatus()
		{
			var reservoirValue = _reservoirSensorEmpty.Read();
			var growpoolSensorEmptyValue = _growpoolSensorEmpty.Read();
			var growpoolSensorFullValue = _growpoolSensorFull.Read();

			var waterLevelData = CreateWaterLevelData(reservoirValue, growpoolSensorEmptyValue, growpoolSensorFullValue);
			//var waterLevelData = new WaterLevelData(1, 1, 1);
			return waterLevelData;
		}

		private WaterLevelData CreateWaterLevelData(double reservoirValue, double growpoolSensorEmptyValue, double growpoolSensorFullValue)
		{
			return new WaterLevelData(reservoirValue, growpoolSensorEmptyValue, growpoolSensorFullValue);
		}
	}

	public class WaterLevelData : IWaterLevelData
    {
		private bool _growPoolEmpty;
		private bool _growPoolFull;
		private bool _reservoirEmpty;

		public WaterLevelData()
		{
			
		}
		public WaterLevelData(double reservoirValue, double growpoolSensorEmptyValue, double growpoolSensorFullValue)
		{
			if (reservoirValue < 0.1)
				_reservoirEmpty = true;
			else
			{
				_reservoirEmpty = false;
			}

			if (growpoolSensorEmptyValue < 0.1)
			{
				_growPoolEmpty = true;
			}
			else
			{
				_growPoolEmpty = false;
			}

			if (growpoolSensorFullValue < 0.1)
			{
				_growPoolFull = false;
			}
			else
			{
				_growPoolFull = true;
			}
		}

		public bool GrowPoolEmpty
		{
			get { return _growPoolEmpty; }
			set { _growPoolEmpty = value; }
		}

		public bool GrowPoolFull
		{
			get { return _growPoolFull; }
			set { _growPoolFull = value; }
		}

		public bool ReservoirEmpty
		{
			get { return _reservoirEmpty; }
			set { _reservoirEmpty = value; }
		}
	}
}
