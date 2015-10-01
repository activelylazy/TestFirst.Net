using System.Windows.Controls;
using TestFirst.Net.Matcher;

namespace TestFirst.Net.WPF.Matcher
{
    public class APasswordBox : PropertyMatcher<PasswordBox>
    {
        private static readonly PasswordBox PropertyNames = null;

        private APasswordBox()
        { }

        public static APasswordBox With()
        {
            return new APasswordBox();
        }

        public APasswordBox Password(string password)
        {
            return Password(AString.EqualTo(password));
        }

        public APasswordBox Password(IMatcher<string> password)
        {
            WithProperty(() => PropertyNames.Password, password);
            return this;
        }
    }
}
