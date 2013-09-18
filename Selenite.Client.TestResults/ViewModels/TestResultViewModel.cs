using System;
using System.Windows;
using System.Windows.Input;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;
using Selenite.Models;

namespace Selenite.Client.TestResults.ViewModels
{
    public class TestResultViewModel : ViewModelBase
    {
        public TestResultViewModel()
        {
            OpenProcessCommand = new DelegateCommand<string>(parameter =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(parameter);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(
                            string.Format("Unable to open \"{0}\".", parameter.ToString()),
                            "Error");
                    }
                });
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string CollectionDescription { get; set; }
        public string Url { get; set; }
        public ResultStatus Status { get; set; }
        public string ResultOutput { get; set; }
        public string StackTrace { get; set; }
        public string Browser { get; set; }
        public string ScreenshotPath { get; set; }

        public ICommand OpenProcessCommand { get; set; }
    }
}
