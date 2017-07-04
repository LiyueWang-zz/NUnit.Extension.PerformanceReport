using System.IO;
using NUnit.Engine;
using NUnit.Extension.PerformanceReport.TestEvents;

namespace NUnit.Extension.PerformanceReport {

	internal class BetterTestEventListenerAdapter : ITestEventListener {

		private readonly IBetterTestEventListener _listener;

		public BetterTestEventListenerAdapter( IBetterTestEventListener listener ) {
			_listener = listener;
		}

		void ITestEventListener.OnTestEvent( string report ) {

			var message = XmlMessageParser.Parse( report );
			if( message == null ) {
				return;
			}

			switch( message.EventType ) {

				case TestEventType.StartSuite:
					OnStartSuite( (StartSuiteEvent)message );
					break;

				case TestEventType.TestSuite:
					OnTestSuite( (TestSuiteEvent)message );
					break;

				case TestEventType.StartTest:
					OnStartTest( (StartTestEvent)message );
					break;

				case TestEventType.TestCase:
					OnTestCase( (TestCaseEvent)message );
					break;
			}
		}

		private void OnStartSuite( StartSuiteEvent msg ) {
			string assemblyName;
			if( TryGetAssembly( msg.FullName, out assemblyName ) ) {
				_listener.AssemblyStarted( msg );
			} else {
				_listener.SuiteStarted( msg );
			}
		}

		private void OnTestSuite( TestSuiteEvent msg ) {
			string assemblyName;
			if( TryGetAssembly( msg.FullName, out assemblyName ) ) {
				_listener.AssemblyFinished( msg );
			} else {
				_listener.SuiteFinished( msg );
			}
		}

		private void OnStartTest( StartTestEvent msg ) {
			_listener.TestStarted( msg );
		}

		private void OnTestCase( TestCaseEvent msg ) {
			_listener.TestFinished( msg );
		}

		private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

		internal static bool TryGetAssembly( string fullName, out string assembly ) {
			
			if( fullName.IndexOfAny( InvalidPathChars ) >= 0 ) {
				assembly = null;
				return false;
			}

			if( !Path.IsPathRooted( fullName ) ) {
				assembly = null;
				return false;
			}

			assembly = Path.GetFileName( fullName );
			return true;
		}
	}

}
