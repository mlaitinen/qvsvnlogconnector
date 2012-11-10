/*
 * Created by SharpDevelop. Licensed under Apache License 2.0, see license.txt for more information.
 * User: Miku Laitinen
 * Date: 9.11.2012
 * Time: 11:19
 */
using System;
using System.Windows.Forms;
using SharpSvn;
using QlikView.Qvx.QvxLibrary;
using System.Collections.ObjectModel;

namespace QvSvnLogConnector
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program 
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			if (args != null && args.Length >= 2)
            {
                new QvSvnLogServer().Run(args[0], args[1]);
            }
		}
		
	}
}
