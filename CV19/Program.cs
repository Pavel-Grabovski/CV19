﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CV19
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }
    }
}
