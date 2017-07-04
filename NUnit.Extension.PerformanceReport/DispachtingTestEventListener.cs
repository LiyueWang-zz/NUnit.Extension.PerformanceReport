using System;
using System.Collections.Concurrent;
using NUnit.Extension.PerformanceReport.TestEvents;

namespace NUnit.Extension.PerformanceReport {

	internal sealed class DispachtingTestEventListener : IBetterTestEventListener {

		private readonly ConcurrentDictionary<int, IBetterTestEventListener> _listeners = new ConcurrentDictionary<int, IBetterTestEventListener>();
		private readonly Func<IBetterTestEventListener> _factory;

		public DispachtingTestEventListener( Func<IBetterTestEventListener> factory ) {
			_factory = factory;
		}

		void IBetterTestEventListener.AssemblyStarted( StartSuiteEvent msg ) {
			var listener = GetListener( msg );
			listener.AssemblyStarted( msg );
		}

		void IBetterTestEventListener.AssemblyFinished( TestSuiteEvent msg ) {

			int parallelizationId = GetParallelizationId( msg );

			IBetterTestEventListener listener;
			if( _listeners.TryRemove( parallelizationId, out listener ) ) {
				listener.AssemblyFinished( msg );
			}
		}

		void IBetterTestEventListener.SuiteStarted( StartSuiteEvent msg ) {
			var listener = GetListener( msg );
			listener.SuiteStarted( msg );
		}

		void IBetterTestEventListener.SuiteFinished( TestSuiteEvent msg ) {
			var listener = GetListener( msg );
			listener.SuiteFinished( msg );
		}

		void IBetterTestEventListener.TestStarted( StartTestEvent msg ) {
			var listener = GetListener( msg );
			listener.TestStarted( msg );
		}

		void IBetterTestEventListener.TestFinished( TestCaseEvent msg ) {
			var listener = GetListener( msg );
			listener.TestFinished( msg );
		}

		private IBetterTestEventListener GetListener( TestEvent e ) {

			int parallelizationId = GetParallelizationId( e );
			return _listeners.GetOrAdd( parallelizationId, i => _factory() );
		}

		private static int GetParallelizationId( TestEvent e ) {
			return GetParallelizationId( e.Id );
		}

		internal static int GetParallelizationId( string testEventId ) {

			if( testEventId == null ) {
				throw new ArgumentNullException( nameof( testEventId ), "Test event id cannot be null" );
			}

			var parts = testEventId.Split( '-' );
			if( parts.Length != 2 ) {
				throw new FormatException( $"Invalid test event id: { testEventId }" );
			}

			int parallelizationId;
			if( !int.TryParse( parts[ 0 ], out parallelizationId ) ) {
				throw new FormatException( $"Invalid test event id: { testEventId }" );
			}

			return parallelizationId;
		}
	}

}
