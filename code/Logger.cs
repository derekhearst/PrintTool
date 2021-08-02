﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace PrintTool
{
    class Logger
    {
        private List<string> logOutput = new();
        private string logType = "";
        private List<TextBox> textBoxes =new();
        private StreamReader streamReader;
        Task taskOutputReader;
        public Logger(string xlogType)
        {
            logType = xlogType;
        }
        public void Log(string log)
        {
            string output = "[" + DateTime.Now.ToShortTimeString() + "] " + log;
            logOutput.Add(output);
            foreach(TextBox textBox in textBoxes)
            {
                textBox.AppendText(output);
            }
        }

        public void SaveLog(string path)
        {
            var myFile = File.CreateText(path + logType);
            foreach(string log in logOutput)
            {
                myFile.WriteLineAsync(log);
            }
        }

        public void AddTextBox(TextBox textBox)
        {
            textBoxes.Add(textBox);
        }

        public void AddOutputStream(StreamReader output)
        {
            
            streamReader = output;
            taskOutputReader = new Task(UpdateOutputStream);
            
        }

        public async void UpdateOutputStream()
        {
            while(streamReader.EndOfStream == false)
            {
                Log(await streamReader.ReadLineAsync());
            }
        }
    }
}
