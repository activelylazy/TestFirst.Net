using System.Windows;
using System.Windows.Controls;

namespace TestFirst.Net.WPF
{
    public interface IDriver<in TControl>
    {
    }

    public static class ControlContainer
    {
        public static Builder<TControl> For<TControl>()
            where TControl : Control, new()
        {
            return new Builder<TControl>(new TControl());
        }

        public class Builder<TControl>
            where TControl : Control, new()
        {
            private readonly TControl m_control;

            public Builder(TControl control)
            {
                m_control = control;
            }

            public ControlContainer<TControl, TViewModel> With<TViewModel>(TViewModel viewModel)
                where TViewModel : class
            {
                return new ControlContainer<TControl, TViewModel>(m_control, viewModel);
            }
        }
    }

    public class ControlContainer<TControl, TViewModel>
        where TControl : Control, new()
        where TViewModel : class
    {
        private readonly Window m_window;
        private readonly TViewModel m_viewModel;
        private readonly TControl m_control;

        internal ControlContainer(TControl control, TViewModel viewModel)
        {
            var visible = TestInteractively.Enabled;

            m_window = new Window();
            m_window.DataContext = null;
            var grid = new Grid();
            m_control = control;
            grid.Children.Add(m_control);
            m_window.Content = grid;
            m_viewModel = viewModel;
            grid.DataContext = m_viewModel;
            m_window.SizeToContent = SizeToContent.WidthAndHeight;
            if (visible)
            {
                m_window.Top = 0;
                m_window.Left = 0;
                m_window.ShowInTaskbar = true;
            }
            else
            {
                m_window.Top = -1000;
                m_window.Left = -1000;
                m_window.ShowInTaskbar = false;
            }
            m_window.Show();
        }

        public TControl Control { get { return m_control; }}
    }
}
