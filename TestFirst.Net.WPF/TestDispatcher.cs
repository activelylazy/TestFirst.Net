using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Threading;

namespace TestFirst.Net.WPF
{
    public class TestDispatcher
    {
        private readonly IList<string> m_errors = new List<string>();
        private readonly Dispatcher m_dispatcher;

        public TestDispatcher()
        {
            m_dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Stop()
        {
            if (!TestInteractively.Enabled)
                m_dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(Stop_UI));
            else
                m_dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() => {/* wait */}));
        }

        public void CheckForErrors()
        {
            if (m_errors.Any())
                throw new Exception(string.Format("There were binding errors during the test: {0}",
                                                  string.Join("\n", m_errors)));
        }

        public void Run()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new TestTraceListener(this));
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            Dispatcher.Run();
        }

        internal void AddError(string message)
        {
            m_errors.Add(message);
        }

        private void Stop_UI()
        {
            m_dispatcher.InvokeShutdown();
        }
    }

    public class TestTraceListener : TraceListener
    {
        private readonly TestDispatcher m_testDispatcher;

        public TestTraceListener(TestDispatcher testDispatcher)
        {
            m_testDispatcher = testDispatcher;
        }

        public override void Write(string message)
        {
            m_testDispatcher.AddError(message);
        }

        public override void WriteLine(string message)
        {
            m_testDispatcher.AddError(message);
        }
    }
}
