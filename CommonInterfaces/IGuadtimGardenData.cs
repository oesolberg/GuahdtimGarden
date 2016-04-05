using System;

namespace CommonInterfaces
{
	public interface IGuadtimGardenData
	{
		bool PumpOn { get; }
		DateTime CreatedDateTime { get; }
		double Temperature { get; }
		double Humidity { get; }
		bool HeaterIsOn{ get; }

		bool GrowPodIsFull { get; }
		bool GrowPodIsEmpty { get; }
		bool ReservoirIsEmpty { get; }

		string CreateDataLine();

	    void UpdateWaterLevelData(IWaterLevelData waterLevelData);
	}
}