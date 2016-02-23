using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Relays
{
	public class RelayController
	{
		private readonly OutputPort _heaterRelay;
		private readonly OutputPort _pumpRelay;

		public RelayController()
		{
			//Set default pins
			_heaterRelay = new OutputPort(Pins.GPIO_PIN_D9, false);
			_pumpRelay = new OutputPort(Pins.GPIO_PIN_D8, false);
		}

		public RelayController(Cpu.Pin heaterRelayPin, Cpu.Pin pumpRelayPin)
		{
			//Set default pins
			_heaterRelay = new OutputPort(heaterRelayPin, false);
			_pumpRelay = new OutputPort(pumpRelayPin, false);
		}

		public void Heater(HeaterStatus heaterStatusStatus)
		{
			if(heaterStatusStatus==HeaterStatus.On)
				_heaterRelay.Write(true);
			if(heaterStatusStatus==HeaterStatus.Off)
				_heaterRelay.Write(false);
		}

		public void RunPump(int millisecondsToRun)
		{
			_pumpRelay.Write(true);
			Thread.Sleep(millisecondsToRun);
			_pumpRelay.Write(false);
		}

		public bool GetHeaterStatus()
		{
			return _heaterRelay.Read();
		}
	}
}
