/*
 * Created by SharpDevelop. Licensed under Apache License 2.0, see license.txt for more information.
 * User: Miku Laitinen
 * Date: 9.11.2012
 * Time: 11:29
 */
using System;
using System.Windows.Interop;
using QlikView.Qvx.QvxLibrary;

namespace QvSvnLogConnector
{
	/// <summary>
	/// Description of QVSvnLogServer.
	/// </summary>
	internal class QvSvnLogServer : QvxServer
	{
		public override QvxConnection CreateConnection()
		{
			return new QvSvnLogConnection();
		}
		
		public override string CreateConnectionString()
		{
			QvxLog.Log(QvxLogFacility.Application, QvxLogSeverity.Debug, "CreateConnectionString()");
            
			Login login = CreateLoginWindowHelper();
            login.ShowDialog();

            string connectionString = null;
            if (login.DialogResult.Equals(true))
            {
                connectionString = String.Format("Server={0};UserId={1};Password={2}",
                    login.GetServer(), login.GetUsername(), login.GetPassword());
            }

            return connectionString;
		}
		
		private Login CreateLoginWindowHelper()
        {
            // Since the owner of the loginWindow is a Win32 process we need to
            // use WindowInteropHelper to make it modal to its owner.

            Login login = new Login();
            WindowInteropHelper wih = new WindowInteropHelper(login);
            wih.Owner = MParentWindow;

            return login;
        }
	}
}
