using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using TestFirst.Net.Matcher;

namespace TestFirst.Net.WPF.Matcher
{
    public class AButton : PropertyMatcher<Button>
    {
        private static readonly Button PropertyNames = null;

        private AButton()
        { }

        public static AButton With()
        {
            return new AButton();
        }

        public AButton IsEnabled(bool isEnabled)
        {
            return IsEnabled(ABool.EqualTo(isEnabled));
        }

        public AButton IsEnabled(IMatcher<bool?> isEnabled)
        {
            WithProperty(() => PropertyNames.IsEnabled, isEnabled);
            return this;
        }
    }
}
