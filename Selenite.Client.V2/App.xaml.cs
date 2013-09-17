﻿using System.Windows;

namespace Selenite.Client.V2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();

            bootstrapper.Run();
        }
    }
}