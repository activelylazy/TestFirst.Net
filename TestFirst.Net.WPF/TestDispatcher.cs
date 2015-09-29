using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Threading;
using TestFirst.Net;

namespace TestFirst.Net.WPF
{
    public class TestDispatcher : IRunOnScenarioEnd
    {
        private readonly ManualResetEvent m_started = new ManualResetEvent(false);
        private readonly ManualResetEvent m_shutdown = new ManualResetEvent(false);
        private readonly bool m_interactive;
        private readonly Thread m_thread;
        private readonly IList<string> m_errors = new List<string>();

        public Dispatcher Dispatcher { get; private set; }

        public TestDispatcher()
        {
            m_interactive = TestInteractively.Enabled;
            m_thread = new Thread(Run);
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_started.WaitOne(5000);
            Dispatcher = Dispatcher.FromThread(m_thread);
        }

        public void OnScenarioEnd()
        {
            Stop();
        }

        public void Stop()
        {
            if (!m_interactive)
                Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(Stop_UI));
            else
                Dispatcher.Invoke(DispatcherPriority.ContextIdle, new Action(() => {/* wait */}));

            if (m_errors.Any())
                throw new Exception(string.Format("There were binding errors during the test: {0}",
                                                  string.Join("\n", m_errors)));
        }

        internal void AddError(string message)
        {
            m_errors.Add(message);
        }

        private void Run()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => m_started.Set()));
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Listeners.Add(new TestTraceListener(this));
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            Dispatcher.Run();
        }

        private void Stop_UI()
        {
            m_shutdown.Set();
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
