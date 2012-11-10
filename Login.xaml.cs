/*
 * Created by SharpDevelop. Licensed under Apache License 2.0, see license.txt for more information.
 * User: Miku Laitinen
 * Date: 9.11.2012
 * Time: 12:45
 */
﻿using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MessageBox = System.Windows.Forms.MessageBox;

namespace QvSvnLogConnector
{
	/// <summary>
	/// Interaction logic for Login.xaml
	/// </summary>
	public partial class Login : Window
	{
		public Login()
		{
			InitializeComponent();
		}
		
		private void testButton_Click(object sender, RoutedEventArgs e)
        {
            PingServer(serverTextBox.Text);
        }

        public string GetServer()
        {
            return serverTextBox.Text;
        }

        public string GetUsername()
        {
            return userTextBox.Text;
        }

        public string GetPassword()
        {
            return passwordBox.Password;
        }

        private void PingServer(string server)
        {
            string result = "Test failed!";

            var pingSender = new Ping();
            var options = new PingOptions();
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            const string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            const int timeout = 120;
            try
            {
                var reply = pingSender.Send(server, timeout, buffer, options);
                if (reply != null && reply.Status == IPStatus.Success)
                {
                    result = String.Format("Address: {0}\n", reply.Address.ToString());
                    result += String.Format("RoundTrip time: {0}\n", reply.RoundtripTime);
                    result += String.Format("Buffer size: {0}\n", reply.Buffer.Length);
                    result += Environment.NewLine;
                    result += "Test successful!\n";
                }
            }
            catch (Exception) { }

            MessageBox.Show(result, "Test Result");
        }

        private void okBbutton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
	}
}