﻿using System;

namespace TestFirst.Net.Inject
{
    /// <summary>
    /// Use the real time
    /// </summary>
    public class SystemClock : IClock
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.Now;
        }
    }
}