﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PrintTool
{
	/// <summary>
	/// Interaction logic for Log.xaml
	/// </summary>
	public partial class Logger : UserControl
	{

		int lineCount = 0;
		public string fileName = "";
		public string fileLoc = @"Data\Logs\Temp\";
		public Logger(string fileName)
		{
			InitializeComponent();
			this.fileName = fileName;
		}


		public async Task Log(string result)
		{
			lineCount++;
			if (result is null or "" or "\n" or "\r" or "\r\n" or "\n\r") { return; } // removing empty lines and unsupported syntax
			result = Regex.Replace(result, "(\u001b\\[1;34m)", "");
			result = Regex.Replace(result, "(\u001b\\[1;36m)", "");
			result = Regex.Replace(result, "(\u001b\\[1;32minit)", "");
			result = Regex.Replace(result, "(\u001b\\[m)", "");
			result = Regex.Replace(result, "(\u001b\\[0m)", "");
			result = Regex.Replace(result, "(\\[0;0)", "");
			result = Regex.Replace(result, "(\\0)", "");
			result = Regex.Replace(result, "(\n\r)", "\r\n");






			LogBox.Dispatcher.Invoke(new Action(() =>
			{
				if (lineCount > 800)
				{
					LogBox.Text = "";
					lineCount = 0;
				}
				LogBox.AppendText(result);
				LogBox.ScrollToEnd();
			}));

			Scroller.Dispatcher.Invoke(new Action(() =>
			{
				Scroller.ScrollToBottom();
			}));

			await File.AppendAllTextAsync(fileLoc + "Log" + fileName + ".txt", result);
		}
	}
}