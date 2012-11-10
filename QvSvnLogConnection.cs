/*
 * Created by SharpDevelop. Licensed under Apache License 2.0, see license.txt for more information.
 * User: Miku Laitinen
 * Date: 9.11.2012
 * Time: 11:33
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using QlikView.Qvx.QvxLibrary;
using SharpSvn;

namespace QvSvnLogConnector
{
	/// <summary>
	/// Defines the table structure and initializes the connection.
	/// </summary>
	public class QvSvnLogConnection : QvxConnection
	{
		public override void Init()
		{
			QvxLog.SetLogLevels(true, true);
            QvxLog.Log(QvxLogFacility.Application, QvxLogSeverity.Notice, "Init()");

            QvxField[] svnLogFields = new QvxField[]
            {
	            new QvxField("Author", 		QvxFieldType.QVX_TEXT, QvxNullRepresentation.QVX_NULL_FLAG_SUPPRESS_DATA, FieldAttrType.ASCII),
	            new QvxField("LogMessage", 	QvxFieldType.QVX_TEXT, QvxNullRepresentation.QVX_NULL_FLAG_SUPPRESS_DATA, FieldAttrType.ASCII),
	            new QvxField("Revision", 	QvxFieldType.QVX_SIGNED_INTEGER, QvxNullRepresentation.QVX_NULL_FLAG_SUPPRESS_DATA, FieldAttrType.INTEGER),
	            new QvxField("Time", 		QvxFieldType.QVX_IEEE_REAL, QvxNullRepresentation.QVX_NULL_FLAG_SUPPRESS_DATA, FieldAttrType.DATE)
            };

            MTables = new List<QvxTable> 
            {
				new QvxTable
				{
                	TableName = "SvnLogEntries",
					GetRows = GetSvnLogEntries,
					Fields = svnLogFields
                }
            };
		}
		
		/// <summary>
		/// Gets the svn log entries from the repository.
		/// </summary>
		/// <returns>An IEnumerable of the QvxDataRows.</returns>
		private IEnumerable<QvxDataRow> GetSvnLogEntries()
        {
            QvxLog.Log(QvxLogFacility.Application, QvxLogSeverity.Notice, "GetSvnLogEntries()");
			
            string username, password, server;
            this.MParameters.TryGetValue("UserId", out username);
            this.MParameters.TryGetValue("Password", out password);
            this.MParameters.TryGetValue("Server", out server);
            System.Net.NetworkCredential creds = new System.Net.NetworkCredential(username, password);
			
			SvnClient client = new SvnClient();
			client.Authentication.DefaultCredentials = creds;
			
			SvnUriTarget target = new SvnUriTarget(server);
			
			Collection<SvnLogEventArgs> logEntries = new Collection<SvnLogEventArgs>();
			bool result = client.GetLog(target.Uri, out logEntries);		
			
			if(!result)
			{
				throw new SvnClientException("Retrieving Subversion log failed");
			}
			
			foreach(SvnLogEventArgs entry in logEntries)
			{
				yield return MakeEntry(entry, FindTable("SvnLogEntries", MTables));
			}
        }
		
		
		/// <summary>
		/// Creates a Qvx data entry from an Svn log entry.
		/// </summary>
		/// <param name="item">The Svn log entry to be used as a source</param>
		/// <param name="table">The destination Qvx table</param>
		/// <returns>The resulting row</returns>
		private QvxDataRow MakeEntry(SvnLogEventArgs entry, QvxTable table)
		{
			QvxDataRow row = new QvxDataRow();
			
			row[table.Fields[0]] = entry.Author;
			row[table.Fields[1]] = entry.LogMessage;
			row[table.Fields[2]] = entry.Revision;
			row[table.Fields[3]] = entry.Time.ToOADate();
			
			return row;
		}
	}
}
