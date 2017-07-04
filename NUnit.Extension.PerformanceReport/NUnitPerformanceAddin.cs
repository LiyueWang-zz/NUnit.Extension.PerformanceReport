using System;
using NUnit.Engine;
using NUnit.Engine.Extensibility;

namespace NUnit.Extension.PerformanceReport {

	[Extension( Enabled = true, Description = "Sends performance metrics." )]
	public sealed class NUnitPerformanceAddin : ITestEventListener {

		private readonly ITestEventListener _innerListener;
		private readonly string outputFilePath = @"C:\AssemblyByMemoryUsage.csv";

		public NUnitPerformanceAddin() {

			_innerListener = new BetterTestEventListenerAdapter(
				new DispachtingTestEventListener(
					() => new NUnitPerformanceListener(
							outputFilePath
						)
				)
			);

			Console.WriteLine(
					"Publishing performance metrics to {0}",
					outputFilePath
				);
		}

		void ITestEventListener.OnTestEvent( string report ) {

			if( _innerListener == null ) {
				return;
			}

			try {
				_innerListener.OnTestEvent( report );
			} catch( Exception err ) {
				Console.Error.WriteLine( "Unhandled exception in performance extension: {0}", err );
			}
		}
	}
}
