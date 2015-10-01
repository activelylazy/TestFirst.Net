using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using NUnit.Framework;
using TestFirst.Net.Extensions.Moq;
using TestFirst.Net.WPF;
using TestFirst.Net.WPF.Matcher;
using TestFirst.Net.Matcher;

namespace TestFirst.Net.WPF.Test
{
    [TestFixture]
    public class LoginControlTest : AbstractWPFScenarioTest
    {
        [Test]
        [RequiresSTA]
        public void FieldsInitiallyEmpty_OKButtonDisabled()
        {
            LoginDriver driver;

            Scenario()
                .Given(driver = new LoginDriver(new LoginViewModel()))

                .Then(driver.Email, Is(ATextBox.With().Text("")))
                .Then(driver.Password, Is(APasswordBox.With().Password("")))
                .Then(driver.OKButton, Is(AButton.With().IsEnabled(false)))
            ;
        }

        [Test]
        [RequiresSTA]
        public void UserCanEnterEmail()
        {
            LoginDriver driver;
            ILoginViewModel viewModel;
            ICommand okCommand;

            Scenario()
                .Given(okCommand = AMock<ICommand>()
                    .WhereMethod(c => c.CanExecute(null)).Returns(false)
                    .Instance)
                .Given(viewModel = AMock<ILoginViewModel>()
                    .WhereGet(v => v.Email).Returns("")
                    .WhereSet(v => v.Email = "fred@example.com")
                    .WhereGet(v => v.OKCommand).Returns(okCommand)
                    .Instance)
                .Given(driver = new LoginDriver(viewModel))

                .When(() => driver.SetEmail("fred@example.com"))

                .Then(ExpectNoMocksFailed())
            ;
        }
    }

    internal class LoginDriver
    {
        private ControlContainer<LoginControl, ILoginViewModel> m_container;

        public LoginDriver(ILoginViewModel viewModel)
        {
            m_container = ControlContainer.For<LoginControl>().With(viewModel);
        }

        public TextBox Email { get { return m_container.Control.Email; } }
        public PasswordBox Password { get { return m_container.Control.Password; } }
        public Button OKButton { get { return m_container.Control.OKButton; } }

        public void SetEmail(string email)
        {
            KeyboardDriver.Focus(Email);
            KeyboardDriver.SendText(email);
            KeyboardDriver.SendTab();
        }
    }
}
