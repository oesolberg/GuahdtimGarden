using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ElzeKool.io;
using ElzeKool.io.sht11_io;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SDCard;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace GuahdtimGarden
{
	public class Program
	{
		private const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";

		private static SDCard.SdCardController _sdCardController;
		private static SensirionSHT11 _tempAndHumiditySensor;

		public static void Main()
		{
			SetupTime();
			SetupMicroSdDrive();
			SetupHumidityAndTemperatureSensor();
			StartControllerLoop();

		}

		

		private static void SetupTime()
		{
			var result=Ntp.Ntp.UpdateTimeFromNtpServer(numberOfRetries: 5);
			Debug.Print("Timeupdate result: "+ result.ToString());
			Debug.Print("DateTime after update: "+DateTime.Now.ToString(DateTimeFormatString));
		}

		private static void SetupMicroSdDrive()
		{
			_sdCardController = new SDCard.SdCardController();
			Debug.Print("DateTime in SD Card Controller: "+_sdCardController.GetTime().ToString(DateTimeFormatString));
			Debug.Print("SD Card initialized");
		}

		private static void SetupHumidityAndTemperatureSensor()
		{
			//Todo - handle errors and try to redo?
			 var ioProvider = new SHT11_GPIO_IOProvider(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);
			_tempAndHumiditySensor=new SensirionSHT11(ioProvider);
			_tempAndHumiditySensor.SoftReset();
			_tempAndHumiditySensor.SetSensorToAccurate();
		}

		private static void StartControllerLoop()
		{
			var numberOfRuns = 2000000;
			do
			{
				var humidity = GetHumidity();
				var temperature = GetTemperature();
				var dataPackage = CreateNewDataPackage(humidity, temperature);
				_sdCardController.DoDataWrite(dataPackage);
				Thread.Sleep(10000);
				numberOfRuns--;
			} while (numberOfRuns>0);
			
			
		}

		private static double GetHumidity()
		{
			// Read Humidity with SHT11 VDD = +/- 3.5V 
			double humidity = _tempAndHumiditySensor.ReadRelativeHumidity(SensirionSHT11.SHT11VDD_Voltages.VDD_3_5V);
			Debug.Print("Humidity in percent: " + humidity.ToString());
			return humidity;
		}
		
		private static double GetTemperature()
		{
			double temperature = _tempAndHumiditySensor.ReadTemperature(SensirionSHT11.SHT11VDD_Voltages.VDD_3_5V, SensirionSHT11.SHT11TemperatureUnits.Celcius);
			Debug.Print("Temperature Celcius: " + temperature.ToString());
			return temperature;
		}

		private static IGuadtimGardenData CreateNewDataPackage(double humidity, double temperature)
		{
			return new SDCard.GuadtimGardenData(true, DateTime.Now,temperature,humidity, false);
		}

	

	}
}
