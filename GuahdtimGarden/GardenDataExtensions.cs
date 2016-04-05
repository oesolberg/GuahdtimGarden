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
			gardenData=new GuadtimGardenData(gardenData.PumpOn,DateTime.Now, gardenData.Temperature,gardenData.Humidity,true,gardenData.ReservoirIsEmpty,gardenData.GrowPodIsEmpty,gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData AddHeaterOff(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, false, gardenData.ReservoirIsEmpty, gardenData.GrowPodIsEmpty, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData ReservoirEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, true, gardenData.GrowPodIsEmpty, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData ReservoirNotEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, false, gardenData.GrowPodIsEmpty, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, true, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodNotEmpty(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, false, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodFull(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, gardenData.GrowPodIsEmpty, true);
			return gardenData;
		}

		public static IGuadtimGardenData SetWaterLevelData(this IGuadtimGardenData gardenData, IGuadtimGardenData gardenDataToUpdateFrom)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenDataToUpdateFrom.ReservoirIsEmpty, gardenDataToUpdateFrom.GrowPodIsEmpty, gardenDataToUpdateFrom.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData GrowPodNotFull(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(gardenData.PumpOn, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, gardenData.GrowPodIsEmpty, false);
			return gardenData;
		}

		public static IGuadtimGardenData PumpOn(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(true, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, gardenData.GrowPodIsEmpty, gardenData.GrowPodIsFull);
			return gardenData;
		}

		public static IGuadtimGardenData PumpOff(this IGuadtimGardenData gardenData)
		{
			gardenData = new GuadtimGardenData(false, DateTime.Now, gardenData.Temperature, gardenData.Humidity, gardenData.HeaterIsOn, gardenData.ReservoirIsEmpty, gardenData.GrowPodIsEmpty, gardenData.GrowPodIsFull);
			return gardenData;
		}

	}
}