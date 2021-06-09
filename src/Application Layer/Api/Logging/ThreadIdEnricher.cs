using System.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Rtl.TvMaze.Api.Logging
{
	/// <summary>
	/// Example of log enricher
	/// </summary>
	public class ThreadIdEnricher : ILogEventEnricher
	{
		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
			"ThreadId", Thread.CurrentThread.ManagedThreadId));
		}
	}
}
