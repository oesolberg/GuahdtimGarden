using System;

namespace CommonInterfaces
{
	public interface IGuadtimGardenData
	{
		bool PumpOn { get; }
		DateTime CreatedDateTime { get; }
		double Temperature { get; }
		double Humidity { get; }
		bool HeaterOn{ get; }

		bool GrowPodFull { get; }
		bool GrowPodEmpty { get; }
		bool ReservoirEmpty { get; }

		string CreateDataLine();


	}
}