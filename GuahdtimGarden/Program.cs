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
            do
            {
                //var numberOfRuns = 2000000;

                AlwaysFullTank();

                //HumidityFocusedFilling();



            } while (true);

        }

        private static void HumidityFocusedFilling()
        {
            var dataPackage = GetAllDataAndCreateDatapackage();

            StoreAndSendInitialData(dataPackage);

            _httpDataDeliverer.SendDataToWeb(dataPackage);

            DoLogicBasedOnTemperature(dataPackage);

            DoLogicBasedOnWaterLevelsWithHumidityFocus(dataPackage);

            DoLogicBasedOnHumidityWithHumidityFocus(dataPackage);

            SleepForGivenSeconds(NumberOfSecondsToSleep);
            //numberOfRuns--;

            //}while(numberOfRuns>0);
        }

        
        private static void DoLogicBasedOnWaterLevelsWithHumidityFocus(IGuadtimGardenData dataPackage)
        {
            //Do we need to do anything?
        }

        private static void DoLogicBasedOnHumidityWithHumidityFocus(IGuadtimGardenData dataPackage)
        {
            if ((dataPackage.Humidity <= 40) && (dataPackage.GrowPodIsEmpty) && (!dataPackage.ReservoirIsEmpty))
            {
                _httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOn());
                while (!dataPackage.ReservoirIsEmpty && !dataPackage.GrowPodIsFull && dataPackage.GrowPodIsEmpty)
                {
                    _relays.RunPump(1500);
                    //Send some message about pumping for a second?
                    var waterLevelData = _waterLevelSensorController.GetWaterLevelStatus();
                   dataPackage.UpdateWaterLevelData(waterLevelData);
                }
                _httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOff());
                _httpDataDeliverer.SendWaterlevelsData(new GuadtimGardenData().SetWaterLevelData(dataPackage));

            }
        }


        private static void StoreAndSendInitialData(IGuadtimGardenData dataPackage)
        {
            _sdCardController.DoDataWrite(dataPackage);

            _httpDataDeliverer.SendDataToWeb(dataPackage);
        }

        private static IGuadtimGardenData GetAllDataAndCreateDatapackage()
        {
            var humidity = GetHumidity();
            var temperature = GetTemperature();
            var waterLevelData = _waterLevelSensorController.GetWaterLevelStatus();
            var heaterStatus = _relays.GetHeaterStatus();
            var dataPackage = CreateNewDataPackage(humidity, temperature, waterLevelData, heaterStatus);
            
            return dataPackage;
        }

        private static void AlwaysFullTank()
        {
            var dataPackage = GetAllDataAndCreateDatapackage();


            StoreAndSendInitialData(dataPackage);

            DoLogicBasedOnTemperature(dataPackage);

            DoLogicBasedOnWaterLevels(dataPackage);

            SleepForGivenSeconds(NumberOfSecondsToSleep);
         
        }

        private static void DoLogicBasedOnTemperature(IGuadtimGardenData dataPackage)
        {
            var temperature=dataPackage.Temperature;
            var heatIsOn = dataPackage.HeaterIsOn;
            if (temperature > MaxTemperature && heatIsOn)
            {
                _relays.Heater(HeaterStatus.Off);
                _httpDataDeliverer.SendHeaterData(new GuadtimGardenData().AddHeaterOff());

            }
            if (temperature < MinimumTemperature && !heatIsOn)
            {
                _relays.Heater(HeaterStatus.On);
                _httpDataDeliverer.SendHeaterData(new GuadtimGardenData().AddHeaterOn());
            }

            //Send some message with chosen action to webserver?
        }

        private static void DoLogicBasedOnWaterLevels(IGuadtimGardenData dataPackage)
        {
            
            if (!dataPackage.ReservoirIsEmpty)
            {
                if (dataPackage.GrowPodIsEmpty)
                {
                    _httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOn());
                    while (!dataPackage.ReservoirIsEmpty && !dataPackage.GrowPodIsFull)
                    {

                        _relays.RunPump(1500);
                        //Send some message about pumping for a second?
                        var waterLevelData = _waterLevelSensorController.GetWaterLevelStatus();
                        dataPackage.UpdateWaterLevelData(waterLevelData);


                    }
                    _httpDataDeliverer.SendPumpData(new GuadtimGardenData().PumpOff());
                    _httpDataDeliverer.SendWaterlevelsData(new GuadtimGardenData().SetWaterLevelData(dataPackage));

                }
            }
            else
            {
                //Send some message about the reservoir being empty
                _httpDataDeliverer.SendWaterlevelsData(new GuadtimGardenData().SetWaterLevelData(dataPackage));
            }
        }

        private static void DoLogicBasedOnHumidity(IGuadtimGardenData dataPackage)
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

        private static IGuadtimGardenData CreateNewDataPackage(double humidity, double temperature, IWaterLevelData waterLevelData, bool heaterStatus, bool pumpStatus = false)
        {
            return new SDCard.GuadtimGardenData(pumpStatus, DateTime.Now, temperature, humidity, heaterStatus, waterLevelData.ReservoirEmpty, waterLevelData.GrowPoolEmpty, waterLevelData.GrowPoolFull);
        }



    }
}
