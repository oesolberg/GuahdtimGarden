using System;
using System.IO;
using System.Threading;
using CommonInterfaces;
using Microsoft.SPOT;
using Microsoft.SPOT.IO;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace SDCard
{
	public class SdCardController
	{



		private bool _oneFolderPerDay;

		private readonly OutputPort _confirm = new OutputPort(Pins.ONBOARD_LED, false);
		private readonly string _driveBase = @"\SD\";
		readonly string _dateFormatString = "yyyy-MM-dd";
		private DateTime _dateTime;

		public SdCardController( bool oneFolderPerday = false)
		{
			_oneFolderPerDay = oneFolderPerday;
		}
		//Check if there is a SD card

		//Create folder?

		//Create file

		//Append data to existing file

		public bool DoDataWrite(IGuadtimGardenData data)
		{
			_dateTime = DateTime.Now;
			if (DriveExists())
			{
				var filePath=PrepareFolderAndCreateFileName();
				return WriteDataToFile(filePath, data);
			}
			return false;
		}
		
		private bool DriveExists()
		{
			DirectoryInfo rootDirectory = new DirectoryInfo(_driveBase);
			
			if (rootDirectory.Exists)
			{
				return true;
			}
			SignalError();
			return false;
		}

		private string PrepareFolderAndCreateFileName()
		{
			var filePath = _driveBase;
			if (_oneFolderPerDay) filePath = CreateDayFolderIfItDoesNotExist(filePath);
			filePath = Path.Combine(filePath,CreateFileName());
			return filePath;
		}

		private string CreateDayFolderIfItDoesNotExist(string rootDirectory)
		{
			var folderName = _dateTime.Date.ToString(_dateFormatString );
			var fullPathToFolder = Path.Combine(rootDirectory, folderName);
			if(!Directory.Exists(fullPathToFolder))
				Directory.CreateDirectory(fullPathToFolder);
			return fullPathToFolder;
		}
		
		private string CreateFileName()
		{
			return _dateTime.Date.ToString(_dateFormatString) + ".txt";
		}

		private bool WriteDataToFile(string filePath, IGuadtimGardenData data)
		{
			using (StreamWriter streamWriter = new StreamWriter(filePath, true))
			{
				try
				{
					streamWriter.WriteLine(data.CreateDataLine());
					streamWriter.Flush();
					SignalWrittenFile();
				}
				catch (Exception)
				{
					SignalError();
					return false;
				}
				finally
				{
					streamWriter.Flush();
					streamWriter.Close();
					Debug.Print("File written");
				}
				
			}
			return true;
		}

		private void SignalError()
		{
			for (int i = 0; i < 10; i++)
			{
				_confirm.Write(true);
				Thread.Sleep(20);
				_confirm.Write(false);
				Thread.Sleep(20);
			}
		}

		void SignalWrittenFile()
		{
			for (int i = 0; i < 1; i++)
			{
				_confirm.Write(true);
				Thread.Sleep(50);
				_confirm.Write(false);
				Thread.Sleep(50);
			}
		}

		public DateTime GetTime()
		{
			return DateTime.Now;
		}
	}
}