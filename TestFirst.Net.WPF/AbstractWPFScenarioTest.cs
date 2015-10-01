using TestFirst.Net.Extensions.Moq;

namespace TestFirst.Net.WPF
{
    public abstract class AbstractWPFScenarioTest : AbstractNUnitMoqScenarioTest
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
