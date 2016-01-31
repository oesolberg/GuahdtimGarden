using System;

namespace SDCard
{
	public interface IGuadtimGardenData
	{
		bool IsWatering { get; }
		DateTime LoggedDateTime { get; }
		double Temperature { get; }
		double Humidity { get; }
		bool IsWarming { get; }
		string CreateDataLine();

	}
}