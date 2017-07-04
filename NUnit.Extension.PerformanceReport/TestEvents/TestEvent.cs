using System;
using System.Xml;

namespace NUnit.Extension.PerformanceReport.TestEvents {

	internal sealed class StartSuiteEvent : TestEvent {
		public override TestEventType EventType => TestEventType.StartSuite;

		public StartSuiteEvent( XmlReader reader ) : base( reader ) { }
	}

	internal sealed class TestSuiteEvent : TestEvent {
		public override TestEventType EventType => TestEventType.TestSuite;

		public TestSuiteType? SuiteType { get; }
		public int? PID { get; }
		public TestSuiteEvent( XmlReader reader ) : base( reader ) {

			TestSuiteType suiteType;
			if( Enum.TryParse( reader.GetAttribute( "type" ), out suiteType ) ) {
				SuiteType = suiteType;

				if( suiteType == TestSuiteType.Assembly ) {
					reader.ReadToFollowing( "properties" );
					if( reader.ReadToDescendant( "property" ) ) {
						do {
							if( reader.GetAttribute( "name" ).Equals( "_PID" ) ) {
								PID = Int32.Parse( reader.GetAttribute( "value" ) );
							}

						} while( reader.ReadToNextSibling( "property" ) );
					}
				}
			}
		}
	}

	internal sealed class StartTestEvent : TestEvent {
		public override TestEventType EventType => TestEventType.StartTest;
		public StartTestEvent( XmlReader reader ) : base( reader ) { }
	}

	internal sealed class TestCaseEvent : TestEvent {
		public override TestEventType EventType => TestEventType.TestCase;
		public TestCaseEvent( XmlReader reader ) : base( reader ) { }
	}

	internal abstract class TestEvent {

		public string Id { get; }
		public string Name { get; }
		public string FullName { get; }
		public abstract TestEventType EventType { get; }

		public TestEvent( XmlReader reader ) {

			Id = reader.GetAttribute( "id" );
			Name = reader.GetAttribute( "name" );
			FullName = reader.GetAttribute( "fullname" );
		}
	}
}
