using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CommonInterfaces;
using ElzeKool.io;
using ElzeKool.io.sht11_io;
using Http;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Relays;
using SDCard;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using WaterLevel;

namespace GuahdtimGarden
{
	public class Program
	{

		private const string WebApiUrlKey = "WebApiUrl";
		private const string TokenKey = "GuahtdimBearerToken";

		private const int NumberOfSecondsToSleep = 600;
		private const double MinimumTemperature = 23;
		private const double MaxTemperature = 28.5;


		private const string DateTimeFormatString = "yyyy-MM-dd HH:mm:ss";

		private static SDCard.SdCardController _sdCardController;
		private static SensirionSHT11 _tempAndHumiditySensor;
		private static HttpDataDeliveryController _httpDataDeliverer;

		private static Relays.RelayController _relays;
		private static WaterLevel.WaterLevelSensor _waterLevelSensorController;

		public static void Main()
		{
			SetupTime();
			SetupMicroSdDrive();
			SetupHumidityAndTemperatureSensor();
			SetupInternetDataSending();
			SetupWaterLevelSensors();
			SetupRelays();


			StartControllerLoop();

		}

		private static void SetupWaterLevelSensors()
		{
			Debug.Print("Setting up water level sensors");
			/*
				Should work as follows:
				Give a dataobject back telling what the water levels are in the 2 pools
				Pool1 (growpod) - Empty and filled
				Pool2 (reservoir) - Empty
			*/
			_waterLevelSensorController = new WaterLevel.WaterLevelSensor();
		}

		private static void SetupRelays()
		{
			Debug.Print("Setting up relays");
			_relays = new Relays.RelayController();
			/* Function: An object with 2-3 methods
			*		Heat-relay on
			*		Heat-relay off
			*
			*		Pump-relay on for x number of milliseconds
			*/
		}

		private static void SetupTime()
		{
			var result = Ntp.Ntp.UpdateTimeFromNtpServer(numberOfRetries: 10);
			Debug.Print("Timeupdate result: " + result.ToString());
			Debug.Print("DateTime after update: " + DateTime.Now.ToString(DateTimeFormatString));
		}

		private static void SetupMicroSdDrive()
		{
			_sdCardController = new SDCard.SdCardController();
			Debug.Print("DateTime in SD Card Controller: " + _sdCardController.GetTime().ToString(DateTimeFormatString));
			Debug.Print("SD Card initialized");
		}

		private static void SetupHumidityAndTemperatureSensor()
		{
			//Todo - handle errors and try to redo?
			var ioProvider = new SHT11_GPIO_IOProvider(Pins.GPIO_PIN_D0, Pins.GPIO_PIN_D1);
			_tempAndHumiditySensor = new SensirionSHT11(ioProvider);
			_tempAndHumiditySensor.SoftReset();
			_tempAndHumiditySensor.SetSensorToAccurate();
		}

		private static void SetupInternetDataSending()
		{

			//var webApiUrl=GetWebApiUrlFromConfig();
			//var headerInfo=GetWebApiHeaderInfo();
			var webApiUrl = "http://guahtdimwebapi.azurewebsites.net";


			var headerInfo = "69f248c8-0605-4bbb-95d0-4217bdd4858a";

			_httpDataDeliverer = new Http.HttpDataDeliveryController(webApiUrl, headerInfo);
		}

		private static string GetWebApiUrlFromConfig()
		{

			var webUrl = ConfigReader.ConfigurationManager.GetAppSetting(WebApiUrlKey);
			return webUrl;
		}

		private static string GetWebApiHeaderInfo()
		{
			var token = ConfigReader.ConfigurationManager.GetAppSetting(TokenKey);
			return token;
		}

		private static void StartControllerLoop()
		{
			//var numberOfRuns = 2000000;
			do
			{
				var humidity = GetHumidity();
				var temperature = GetTemperature();
				//var humidity = 20.0;
				//var temperature = 20.0;

				var waterLevelData = _waterLevelSensorController.GetWaterLevelStatus();


				var heaterStatus = _relays.GetHeaterStatus();


				var dataPackage = CreateNewDataPackage(humidity, temperature, waterLevelData, heaterStatus);

				_sdCardController.DoDataWrite(dataPackage);

				_httpDataDeliverer.SendDataToWeb(dataPackage);

				DoLogicBasedOnTemperature(temperature,heaterStatus);

				DoLogicBasedOnWaterLevels(waterLevelData);

				DoLogicBasedOnHumidity(humidity);



				SleepForGivenSeconds(NumberOfSecondsToSleep);
				//numberOfRuns--;
			} while (true);
			//}while(numberOfRuns>0);


		}

		private static void DoLogicBasedOnTemperature(double temperature,bool heatOn)
		{
			if (temperature > MaxTemperature && heatOn)
			{
				_relays.Heater(HeaterStatus.Off);
				_httpDataDeliverer.SendHeaterData(new GuadtimGardenData().AddHeaterOff());

			}
			if (temperature < MinimumTemperature && !heatOn)
			{
				_relays.Heater(HeaterStatus.On);
				_httpDataDeliverer.SendHeaterData(new GuadtimGardenData().AddHeaterOn());
			}

			//Send some message with chosen action to webserver?
		}

		private static void DoLogicBasedOnWaterLevels(WaterLevelData waterLevelData)
		{
			if (!waterLevelData.ReservoirEmpty)
			{
				if (waterLevelData.GrowPoolEmpty)
				{
					_httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOn());
					while (!waterLevelData.ReservoirEmpty && !waterLevelData.GrowPoolFull)
					{

						_relays.RunPump(1500);
						//Send some message about pumping for a second?
						waterLevelData = _waterLevelSensorController.GetWaterLevelStatus();


					}
					_httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOff());
					_httpDataDeliverer.SendWaterlevelsData(new GuadtimGardenData().SetWaterLevelData(waterLevelData));

				}
			}
			else
			{
				//Send some message about the reservoir being empty
				_httpDataDeliverer.SendWaterlevelsData(new GuadtimGardenData().SetWaterLevelData(waterLevelData));
			}
		}

		private static void DoLogicBasedOnHumidity(double humidity)
		{
			//At some later time create a web method to post help for water if it is really dry!
		}

		private static void SleepForGivenSeconds(int numberOfSeconds)
		{
			Thread.Sleep(numberOfSeconds * 1000);
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

		private static IGuadtimGardenData CreateNewDataPackage(double humidity, double temperature, WaterLevel.WaterLevelData waterLevelData, bool heaterStatus, bool pumpStatus = false)
		{
			return new SDCard.GuadtimGardenData(pumpStatus, DateTime.Now, temperature, humidity, heaterStatus, waterLevelData.ReservoirEmpty, waterLevelData.GrowPoolEmpty, waterLevelData.GrowPoolFull);
		}



	}
}
