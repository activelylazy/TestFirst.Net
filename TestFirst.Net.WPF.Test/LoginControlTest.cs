using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NUnit.Framework;
using TestFirst.Net.Extensions.NUnit;
using TestFirst.Net.WPF.Matcher;
using TestFirst.Net.Matcher;

namespace TestFirst.Net.WPF.Test
{
    [TestFixture]
    public class LoginControlTest : AbstractNUnitScenarioTest
    {
        [Test]
        public void FieldsInitiallyEmpty_OKButtonDisabled()
        {
             TestDispatcher.Run(() =>
                {
                    LoginDriver driver;

                    Scenario()
                        .Given(driver = new LoginDriver())

                        .Then(driver.Email, Is(ATextBox.With().Text("")))
                        .Then(driver.Password, Is(APasswordBox.With().Password("")))
                        .Then(driver.OKButton, Is(AButton.With().IsEnabled(false)))
                    ;
                });
        }
    }

    internal class LoginDriver
    {
        private ControlContainer<LoginControl, LoginViewModel> m_container;

        public LoginDriver()
        {
            m_container = ControlContainer.For<LoginControl>().With(new LoginViewModel());
        }

        public TextBox Email { get { return m_container.Control.Email; } }
        public PasswordBox Password { get { return m_container.Control.Password; } }
        public Button OKButton { get { return m_container.Control.OKButton; } }
    }
}
