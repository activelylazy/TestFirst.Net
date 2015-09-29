using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestFirst.Net.WPF.Test
{
    public interface ILoginViewModel
    {
        string Email { get; set; }
        ICommand OKCommand { get; }
    }

    internal class LoginViewModel : ILoginViewModel
    {
        public string Email { get; set; }
        public ICommand OKCommand { get; private set; }

        public LoginViewModel()
        {
            OKCommand = new OKCommand(this);
        }
    }

    internal class OKCommand : ICommand
    {
        private LoginViewModel m_viewModel;

        public OKCommand(LoginViewModel viewModel)
        {
            m_viewModel = viewModel;
        }

        public void Execute(object param)
        {

        }

        public bool CanExecute(object param)
        {
            return false;
        }

        public event EventHandler CanExecuteChanged = delegate { };
    }
}
