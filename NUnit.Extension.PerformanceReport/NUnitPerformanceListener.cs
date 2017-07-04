using System;
using System.Diagnostics;
using System.IO;
using NUnit.Extension.PerformanceReport.TestEvents;

namespace NUnit.Extension.PerformanceReport {

	internal sealed class NUnitPerformanceListener : IBetterTestEventListener {

		private readonly string m_outputFilePath;

		public NUnitPerformanceListener(string outputFilePath) {
			m_outputFilePath = outputFilePath;
		}

		void IBetterTestEventListener.AssemblyFinished( TestSuiteEvent msg ) {

			if( msg.SuiteType == TestSuiteType.Assembly ) {

				if( msg.PID != null ) {
					Process p = Process.GetProcessById( (int)msg.PID );
					p.Refresh();
					long maxMemoryUsage = p.PeakWorkingSet64 / 1024 / 1024;
					WritePerformanceMetrics( msg.Name, maxMemoryUsage );
					Console.WriteLine( "[Assembly]: " + ( msg.Name ) + ", [MaxMemoryUsage]: " + ( maxMemoryUsage ) + " MB, [PID]: " + msg.PID );
				}
			}
		}

		void IBetterTestEventListener.AssemblyStarted( StartSuiteEvent msg ) {
			throw new NotImplementedException();
		}

		void IBetterTestEventListener.SuiteStarted( StartSuiteEvent msg ) {
			throw new NotImplementedException();
		}

		void IBetterTestEventListener.SuiteFinished( TestSuiteEvent msg ) {
			throw new NotImplementedException();
		}

		void IBetterTestEventListener.TestStarted( StartTestEvent msg ) {
			throw new NotImplementedException();
		}

		void IBetterTestEventListener.TestFinished( TestCaseEvent msg ) {
			throw new NotImplementedException();
		}

		private void WritePerformanceMetrics( string assemblyName, long maxMemoryUsage ) {

			var newLine = string.Format( "{0},{1}\n", assemblyName, maxMemoryUsage );
			File.AppendAllText( m_outputFilePath, newLine );
		}
	}
}
