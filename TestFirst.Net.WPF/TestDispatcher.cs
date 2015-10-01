using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using TestFirst.Net;

namespace TestFirst.Net.WPF
{
    public class TestDispatcher
    {
        private readonly IList<string> m_errors = new List<string>();

        public Dispatcher Dispatcher { get; private set; }

        public TestDispatcher()
        {
            Dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void Stop()
        {
            if (!TestInteractively.Enabled)
                Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(Stop_UI));
            else
                Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() => {/* wait */}));
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
            Dispatcher.InvokeShutdown();
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
