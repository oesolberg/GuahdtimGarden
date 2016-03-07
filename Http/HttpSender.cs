using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace Http
{
	public class HttpSender
	{
		private readonly string _headerToken;
			//"http://requestb.in/1agwmmz1"n;
		private readonly string _webapiUrl;

		public HttpSender(string webapiUrl, string headerToken)
		{
			this._webapiUrl = webapiUrl;
			this._headerToken = headerToken;
		}

		public void InitializeNetwork()
		{
			NetworkInterface networkInterface = NetworkInterface.GetAllNetworkInterfaces()[0];

			if (networkInterface.IsDhcpEnabled)
			{
				Debug.Print(" Waiting for IP address ");

				while (NetworkInterface.GetAllNetworkInterfaces()[0].IPAddress == IPAddress.Any.ToString()) ;
			}

			// Display network config for debugging
			Debug.Print("Network configuration");
			Debug.Print(" Network interface type: " + networkInterface.NetworkInterfaceType.ToString());
			//Debug.Print(" MAC Address: " + BytesToHexString(networkInterface.PhysicalAddress));
			Debug.Print(" DHCP enabled: " + networkInterface.IsDhcpEnabled.ToString());
			Debug.Print(" Dynamic DNS enabled: " + networkInterface.IsDynamicDnsEnabled.ToString());
			Debug.Print(" IP Address: " + networkInterface.IPAddress.ToString());
			Debug.Print(" Subnet Mask: " + networkInterface.SubnetMask.ToString());
			Debug.Print(" Gateway: " + networkInterface.GatewayAddress.ToString());

			foreach (string dnsAddress in networkInterface.DnsAddresses)
			{
				Debug.Print(" DNS Server: " + dnsAddress.ToString());
			}

		}

		public void SendRequest(string dataString)
		{
			HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(_webapiUrl);

			UTF8Encoding enc = new UTF8Encoding();
			byte[] data = UTF8Encoding.UTF8.GetBytes(dataString);
			var guahtdimBearer = GetGuahtdimBearerString();

			webReq.Headers.Add("Authentication", guahtdimBearer);
			webReq.Method = "POST";
			webReq.ContentType = "application/json";
			webReq.ContentLength = data.Length;
			webReq.Timeout = 10000;
			Stream dataStream = null;
			WebResponse response = null;
			try
			{
				dataStream = webReq.GetRequestStream();

				dataStream.Write(data, 0, data.Length);
				dataStream.Close();

				response = webReq.GetResponse();

				//HttpWebResponse resp = (HttpWebResponse)webReq.GetResponse();

				Debug.Print(((HttpWebResponse)response).StatusDescription);

			}
			catch (Exception ex)
			{

				Debug.Print(ex.Message);

			}
			finally
			{
				if (response != null)
					response.Close();
				if (dataStream != null)
					dataStream.Close();
			}
			
		}

		private string GetGuahtdimBearerString()
		{
			return "GuahtdimBearer " + _headerToken;// "69f248c8-0605-4bbb-95d0-4217bdd4858a";
		}

		public void DoInitializeAndSend(string dataString)
		{
			InitializeNetwork();
			SendRequest(dataString);
		}
	}
}