using System;

namespace CommonInterfaces
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