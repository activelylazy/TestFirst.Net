using System.Windows.Controls;
using TestFirst.Net.Matcher;

namespace TestFirst.Net.WPF.Matcher
{
    public class ATextBox : PropertyMatcher<TextBox>
    {
        private static readonly TextBox PropertyNames = null;

        private ATextBox()
        {}

        public static ATextBox With()
        {
            return new ATextBox();
        }

        public ATextBox Text(string text)
        {
            return Text(AString.EqualTo(text));
        }

        public ATextBox Text(IMatcher<string> text)
        {
            WithProperty(() => PropertyNames.Text, text);
            return this;
        }
    }
}
