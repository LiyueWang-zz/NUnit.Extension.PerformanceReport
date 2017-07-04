
using NUnit.Extension.PerformanceReport.TestEvents;

namespace NUnit.Extension.PerformanceReport {

	internal interface IBetterTestEventListener {

		void AssemblyStarted( StartSuiteEvent msg );
		void AssemblyFinished( TestSuiteEvent msg );

		void SuiteStarted( StartSuiteEvent msg );
		void SuiteFinished( TestSuiteEvent msg );

		void TestStarted( StartTestEvent msg );
		void TestFinished( TestCaseEvent msg );
	}
}
