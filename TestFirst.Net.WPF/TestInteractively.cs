using System;

namespace TestFirst.Net.WPF
{
    public static class TestInteractively
    {
        private static bool _enabled;
        public static bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEAMCITY_VERSION")))
                    throw new Exception("Cannot test interactively on TeamCity - please remove Interactive=true from tests before commit");
                _enabled = value;
            }
        }
    }
}
