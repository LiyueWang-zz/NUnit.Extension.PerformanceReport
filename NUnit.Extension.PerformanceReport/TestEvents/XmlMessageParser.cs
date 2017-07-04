using System.IO;
using System.Xml;

namespace NUnit.Extension.PerformanceReport.TestEvents {

	internal static class XmlMessageParser {

		internal static TestEvent Parse( string xml ) {

			using( var stream = new StringReader( xml ) )
			using( var reader = XmlReader.Create( stream ) ) {

				if( !reader.Read() ) {
					return null;
				}

				if( reader.NodeType != XmlNodeType.Element ) {
					return null;
				}

				var messageType = reader.Name;
				switch( messageType ) {

					case "start-suite":
						return new StartSuiteEvent( reader );

					case "test-suite":
						return new TestSuiteEvent( reader );

					case "start-test":
						return new StartTestEvent( reader );

					case "test-case":
						return new TestCaseEvent( reader );

					default:
						return null;
				}
			}
		}
	}
}
