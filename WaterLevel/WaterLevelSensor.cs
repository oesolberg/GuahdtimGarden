using System;
using System.Threading;
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
		
		// Read the waterlevel and return a WaterLevelDataStruct
		public WaterLevelData GetWaterLevelStatus()
		{
			var reservoirValue = _reservoirSensorEmpty.Read();
			var growpoolSensorEmptyValue = _growpoolSensorEmpty.Read();
			var growpoolSensorFullValue = _growpoolSensorFull.Read();

			var waterLevelData = CreateWaterLevelData(reservoirValue, growpoolSensorEmptyValue, growpoolSensorFullValue);
			return waterLevelData;
		}

		private WaterLevelData CreateWaterLevelData(double reservoirValue, double growpoolSensorEmptyValue, double growpoolSensorFullValue)
		{
			return new WaterLevelData(reservoirValue, growpoolSensorEmptyValue, growpoolSensorFullValue);
		}
	}

	public struct WaterLevelData
	{
		public WaterLevelData(double reservoirValue, double growpoolSensorEmptyValue, double growpoolSensorFullValue)
		{
			if (reservoirValue < 0.1)
				ReservoirEmpty = true;
			else
			{
				ReservoirEmpty = false;
			}

			if (growpoolSensorEmptyValue<0.1)
			{
				GrowPoolEmpty = true;
			}
			else
			{
				GrowPoolEmpty = false;
			}

			if (growpoolSensorFullValue < 0.1)
			{
				GrowPoolFull = false;
			}
			else
			{
				GrowPoolFull = true;
			}
		}
		public bool GrowPoolEmpty { get; private set; }
		public bool GrowPoolFull{ get; private set; }
		public bool ReservoirEmpty { get; private set; }
	}
}
