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

		private readonly bool SetRelayLogicOn = false;
		private readonly bool SetRelayLogicOff = true;

		public RelayController()
		{
			//Set default pins
			_heaterRelay = new OutputPort(Pins.GPIO_PIN_D9,SetRelayLogicOff);
			_pumpRelay = new OutputPort(Pins.GPIO_PIN_D8, SetRelayLogicOff);
		}

		public RelayController(Cpu.Pin heaterRelayPin, Cpu.Pin pumpRelayPin)
		{
			//Set default pins
			_heaterRelay = new OutputPort(heaterRelayPin, SetRelayLogicOff);
			_pumpRelay = new OutputPort(pumpRelayPin, SetRelayLogicOff);
		}

		public void Heater(HeaterStatus heaterStatusStatus)
		{
			if(heaterStatusStatus==HeaterStatus.On)
				_heaterRelay.Write(SetRelayLogicOn);
			if(heaterStatusStatus==HeaterStatus.Off)
				_heaterRelay.Write(SetRelayLogicOff);
		}

		public void RunPump(int millisecondsToRun)
		{
			_pumpRelay.Write(SetRelayLogicOn);
			Thread.Sleep(millisecondsToRun);
			_pumpRelay.Write(SetRelayLogicOff);
		}

		public bool GetHeaterStatus()
		{
			var heaterRelayStatus=_heaterRelay.Read();
			if (heaterRelayStatus == SetRelayLogicOff)
				return false;
			return true;
		}
	}
}
