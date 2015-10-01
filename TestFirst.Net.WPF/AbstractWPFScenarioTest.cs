using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestFirst.Net.Extensions.Moq;

namespace TestFirst.Net.WPF
{
    public class AbstractWPFScenarioTest : AbstractNUnitMoqScenarioTest
    {
        private TestDispatcher m_dispatcher;

        public override void BeforeTest()
        {
            m_dispatcher = new TestDispatcher();
            base.BeforeTest();
        }

        public override void AfterTest()
        {
            m_dispatcher.Stop();
            m_dispatcher.Run();
            m_dispatcher.CheckForErrors();
            base.AfterTest();
        }
    }
}
