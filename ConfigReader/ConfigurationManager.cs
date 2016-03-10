using System;
using System.Xml;
using System.Collections;
using System.IO;
using Microsoft.SPOT;

namespace ConfigReader
{

	public static class ConfigurationManager
	{

		private const string AppSettingsFilePath = @"\SD\Appsettings.secret.config";

		private const string APPSETTINGS_SECTION = "appSettings";
		private const string ADD = "add";
		private const string KEY = "key";
		private const string VALUE = "value";

		private static Hashtable _appSettings;

		static ConfigurationManager()
		{
			_appSettings = new Hashtable();
		}

		public static string GetAppSetting(string key)
		{
			if (_appSettings.Count == 0)
			{
				LoadConfigFile();
			}
			return GetAppSetting(key, null);
		}

		private static void LoadConfigFile()
		{
			using (Stream stream = new FileStream(AppSettingsFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				ConfigurationManager.Load(stream);
			}
		}

		public static string GetAppSetting(string key, string defaultValue)
		{
			if (!_appSettings.Contains(key))
				return defaultValue;
			return (string)_appSettings[key];
		}

		public static void Load(Stream xmlStream)
		{
			using (XmlReader reader = XmlReader.Create(xmlStream))
			{
				while (reader.Read())
				{
					switch (reader.Name)
					{
						case APPSETTINGS_SECTION:
							while (reader.Read())
							{
								if (reader.Name == APPSETTINGS_SECTION)
									break;

								if (reader.Name == ADD)
								{
									var key = reader.GetAttribute(KEY);
									var value = reader.GetAttribute(VALUE);

									//Debug.Print(key + "=" + value);
									_appSettings.Add(key, value);
								}
							}

							break;
					}
				}
			}
		}
	}
}
