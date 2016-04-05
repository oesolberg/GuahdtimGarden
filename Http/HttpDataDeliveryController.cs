using System;
using System.IO;
using CommonInterfaces;
using Microsoft.SPOT;
using System.Net;
using System.Text;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Http
{
	public class HttpDataDeliveryController
	{
		private readonly string _webapiUrl;
		private readonly string _headerToken;
		private HttpSender _httpSender;

		//public HttpDataDeliveryController()
		//{
		//	_httpSender=new HttpSender();
		//}

		public HttpDataDeliveryController(string webapiUrl, string headerToken)
		{
			_webapiUrl = webapiUrl;
			_headerToken = headerToken;
			_httpSender = new HttpSender(webapiUrl, headerToken);
		}

		public void SendDataToWeb(IGuadtimGardenData dataPackage)
		{//deviceId = BytesToHexString(networkInterface.PhysicalAddress);
			SendHeaterData(dataPackage);
			SendTempAndHumidityData(dataPackage);
			SendWaterlevelsData(dataPackage);
		}

		public void SendPumpData(IGuadtimGardenData dataPackage)
		{
			var dataString = CreatePumpDataString(dataPackage);

			var urlPath = "/pump";
			_httpSender.DoInitializeAndSend(dataString, urlPath);

		}

		private string CreatePumpDataString(IGuadtimGardenData dataPackage)
		{
			var dataString = "CreatedDateTime=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
			dataString += "&PumpOn=" + dataPackage.PumpOn;
			return dataString;
		}

		public void SendHeaterData(IGuadtimGardenData dataPackage)
		{
			var dataString = CreateHeaterDataString(dataPackage);
			var urlPath = "/heater";
			_httpSender.DoInitializeAndSend(dataString,urlPath);
		}

		private string CreateHeaterDataString(IGuadtimGardenData dataPackage)
		{
			var dataString = "CreatedDateTime=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
			dataString += "&HeatOn=" + dataPackage.HeaterIsOn;
			return dataString;
		}
		//private string CreateDataString(IGuadtimGardenData dataPackage)
		//{
		//	var dataString = "CreatedDateTime=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
		//	var humidity = dataPackage.Humidity;
		//	dataString += "&humidity=" + humidity.ToString("f");
		//	dataString += "&temperature=" + dataPackage.Temperature.ToString("f");
		//	dataString += "&IsWarming=" + dataPackage.HeaterIsOn.ToString();
		//	dataString += "&IsWatering=" + dataPackage.PumpOn.ToString();

		//	return dataString;
		//}

		public void SendWaterlevelsData(IGuadtimGardenData dataPackage)
		{
			var dataString = CreateWaterlevelsDataString(dataPackage);
			var urlPath = "/waterlevels";
			_httpSender.DoInitializeAndSend(dataString, urlPath);
		}

		private string CreateWaterlevelsDataString(IGuadtimGardenData dataPackage)
		{
			var dataString = "CreatedDateTime=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
			dataString += "&ReservoirIsEmpty=" + dataPackage.ReservoirIsEmpty;
			dataString += "&GrowPodIsEmpty=" + dataPackage.GrowPodIsEmpty;
			dataString += "&GrowPodIsFull=" + dataPackage.GrowPodIsFull;
			return dataString;
		}

		public void SendTempAndHumidityData(IGuadtimGardenData dataPackage)
		{
			var dataString = CreateHumidityDataString(dataPackage);
			var urlPath = "/tempandhumidity";
			_httpSender.DoInitializeAndSend(dataString, urlPath);
		}

		private string CreateHumidityDataString(IGuadtimGardenData dataPackage)
		{
			var dataString = "CreatedDateTime=" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
			dataString += "&Humidity=" + dataPackage.Humidity.ToString("f");
			dataString += "&Temperature=" + dataPackage.Temperature.ToString("f");
			return dataString;
		}

		//		private void ThePreferredAproach(){
		//			using (var client = new HttpClient())
		//{
		//    var values = new Dictionary<string, string>
		//    {
		//       { "thing1", "hello" },
		//       { "thing2", "world" }
		//    };

		//    var content = new FormUrlEncodedContent(values);

		//    var response = await client.PostAsync("http://www.example.com/recepticle.aspx", content);

		//    var responseString = await response.Content.ReadAsStringAsync();
		//}


		//using (var client = new HttpClient())
		//{
		//    var responseString = client.GetStringAsync("http://www.example.com/recepticle.aspx");
		//}

		//		}

		//		private void AnotherTest()
		//		{
		//			var request = (HttpWebRequest)WebRequest.Create("http://www.example.com/recepticle.aspx");

		//var postData = "thing1=hello";
		//    postData += "&thing2=world";
		//var data = Encoding.ASCII.GetBytes(postData);

		//request.Method = "POST";
		//request.ContentType = "application/x-www-form-urlencoded";
		//request.ContentLength = data.Length;

		//using (var stream = request.GetRequestStream())
		//{
		//    stream.Write(data, 0, data.Length);
		//}

		//var response = (HttpWebResponse)request.GetResponse();

		//var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
		//GET

		//var request = (HttpWebRequest)WebRequest.Create("http://www.example.com/recepticle.aspx");

		//var response = (HttpWebResponse)request.GetResponse();

		//var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

		//		}
		
		

		private void DudeTest()
		{
			using (HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://requestb.in/1agwmmz1"))// eventHubAddressHttps + "/messages" + "?timeout=60" + ApiVersion))
			{
				request.Timeout = 2500;
				//request.Method = "POST";
				request.Method = "GET";

				// Enable these options to suit your environment
				//request.Proxy = new WebProxy("myproxy.myorganisation.com", true);
				//request.Credentials = new NetworkCredential("myusername", "mytopsecretpassword"); 

				//request.Headers.Add("Authorization", token);
				//request.Headers.Add("ContentType", "application/json;charset=utf-8");

				//byte[] buffer = Encoding.UTF8.GetBytes(messageBody);

				//request.ContentLength = buffer.Length;

				// request body
				//using (Stream stream = request.GetRequestStream())
				//{
				//	stream.Write(buffer, 0, buffer.Length);
				//}
				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						var test = response.GetResponseStream();
						Stream receiveStream = response.GetResponseStream();
						//Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
						// Pipes the stream to a higher level stream reader with the required encoding format. 
						StreamReader readStream = new StreamReader(receiveStream);//, encode);
						Debug.Print(("\r\nResponse stream received."));
						Char[] read = new Char[256];
						// Reads 256 characters at a time.    
						int count = readStream.Read(read, 0, 256);
						Debug.Print("HTML...\r\n");
						while (count > 0)
						{
							// Dumps the 256 characters on a string and displays the string to the console.
							String str = new String(read, 0, count);
							Debug.Print(str);
							count = readStream.Read(read, 0, 256);
						}
						Debug.Print("");
						// Releases the resources of the response.
						response.Close();
						// Releases the resources of the Stream.
						readStream.Close();

						Debug.Print("HTTP Status:" + response.StatusCode + " : " + response.StatusDescription);
						
					}

				}
				catch (Exception ex)
				{
					Debug.Print(ex.Message);
					throw;
				}
			}
		}


		private void EventHubSendMessage(string eventHubAddressHttps, string messageBody)
		{
			//string token = CreateSasToken(eventHubAddressHttps + "/messages", sasKeyName, sasKeyText);

			//try
			//{
			//	using (HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(eventHubAddressHttps + "/messages" + "?timeout=60" + ApiVersion))
			//	{
			//		request.Timeout = 2500;
			//		request.Method = "POST";

			//		// Enable these options to suit your environment
			//		//request.Proxy = new WebProxy("myproxy.myorganisation.com", true);
			//		//request.Credentials = new NetworkCredential("myusername", "mytopsecretpassword"); 

			//		request.Headers.Add("Authorization", token);
			//		request.Headers.Add("ContentType", "application/json;charset=utf-8");

			//		byte[] buffer = Encoding.UTF8.GetBytes(messageBody);

			//		request.ContentLength = buffer.Length;

			//		// request body
			//		using (Stream stream = request.GetRequestStream())
			//		{
			//			stream.Write(buffer, 0, buffer.Length);
			//		}

			//		using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			//		{
			//			Debug.Print("HTTP Status:" + response.StatusCode + " : " + response.StatusDescription);
			//		}
			//	}
			//}
			//catch (WebException we)
			//{
			//	Debug.Print(we.Message);
			//}
		}

		// Create a SAS token for a specified scope. SAS tokens are described in http://msdn.microsoft.com/en-us/library/windowsazure/dn170477.aspx.
		//private static string CreateSasToken(string uri, string keyName, string key)
		//{
		//	// Set token lifetime to 20 minutes. When supplying a device with a token, you might want to use a longer expiration time.
		//	uint tokenExpirationTime = GetExpiry(20 * 60);

		//	string stringToSign = HttpUtility.UrlEncode(uri) + "\n" + tokenExpirationTime;

		//	var hmac = SHA.computeHMAC_SHA256(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(stringToSign));
		//	string signature = Convert.ToBase64String(hmac);

		//	signature = Base64NetMf42ToRfc4648(signature);

		//	string token = "SharedAccessSignature sr=" + HttpUtility.UrlEncode(uri) + "&sig=" + HttpUtility.UrlEncode(signature) + "&se=" + tokenExpirationTime.ToString() + "&skn=" + keyName;

		//	return token;
		//}

		private static string Base64NetMf42ToRfc4648(string base64netMf)
		{
			var base64Rfc = string.Empty;

			for (var i = 0; i < base64netMf.Length; i++)
			{
				if (base64netMf[i] == '!')
				{
					base64Rfc += '+';
				}
				else if (base64netMf[i] == '*')
				{
					base64Rfc += '/';
				}
				else
				{
					base64Rfc += base64netMf[i];
				}
			}
			return base64Rfc;
		}

		static uint GetExpiry(uint tokenLifetimeInSeconds)
		{
			const long ticksPerSecond = 1000000000 / 100; // 1 tick = 100 nano seconds

			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan diff = DateTime.Now.ToUniversalTime() - origin;

			return ((uint)(diff.Ticks / ticksPerSecond)) + tokenLifetimeInSeconds;
		}
	}
}
