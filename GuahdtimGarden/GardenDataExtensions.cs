using System;
using CommonInterfaces;
using SDCard;
using WaterLevel;

namespace GuahdtimGarden
{
	public static class GardenDataExtensions
	{
		public static IGuadtimGardenData AddHeaterOn(this IGuadtimGardenData gardenData)
		{
			gardenData=new GuadtimGardenData(gardenData.PumpOn,DateTime.Now, gardenData.Temperature,gardenData.Humidity,true,gardenData.ReservoirEmpty,gardenData.GrowPodEmpty,gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData AddHeaterOff(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, false, gardenData.ReservoirEmpty, gardenData.GrowPodEmpty, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData ReservoirEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, true, gardenData.GrowPodEmpty, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData ReservoirNotEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, false, gardenData.GrowPodEmpty, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, true, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodNotEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, false, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodFull(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, gardenData.GrowPodEmpty, true);
			return gardenData;
		}

		public static IGuadtimGardenData SetWaterLevelData(this IGuadtimGardenData gardenData,WaterLevelData waterLevelData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, waterLevelData.ReservoirEmpty, waterLevelData.GrowPoolEmpty, waterLevelData.GrowPoolFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodNotFull(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, gardenData.GrowPodEmpty, false);
			return gardenData;
		}

		public static IGuadtimGardenData PumpOn(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(true, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, gardenData.GrowPodEmpty, gardenData.GrowPodFull);
			return gardenData;
		}

		public static IGuadtimGardenData PumpOff(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(false, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterOn, gardenData.ReservoirEmpty, gardenData.GrowPodEmpty, gardenData.GrowPodFull);
			return gardenData;
		}

	}
}