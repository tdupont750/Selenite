using System;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using Selenite.Browsers;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class TestService : ITestService
    {
        public const string AboutBlank = "about:blank";

        public void ExecuteTest(BrowserBase browser, Test test)
        {
            Trace.WriteLine("Collection: " + test.CollectionName);
            Trace.WriteLine("Name: " + test.Name);
            Trace.WriteLine("Url: " + test.DomainUrl);

            try
            {
                browser.Driver.Url = test.DomainUrl;

                // Hack, sometimes the set does not work?
                if (browser.Driver.Url == AboutBlank)
                    browser.Driver.Url = test.DomainUrl;

                dynamic context = new ExpandoObject();
                context.DriverType = browser.DriverType;

                foreach (var command in test.Commands)
                {
                    Thread.Sleep(100);

                    try
                    {
                        command.Execute(browser.Driver, context);
                    }
                    catch (Exception ex)
                    {
                        var message = String.Format("Test: {0}, Command: {1}{2}{3}", test.Name, command.Name, Environment.NewLine, ex.Message);
                        throw new Exception(message, ex);
                    }
                }

                Trace.WriteLine("Success");
            }
            catch
            {
                Trace.WriteLine(String.Empty);
                Trace.WriteLine("***** FAILURE *****");
                throw;
            }
            finally
            {
                Trace.WriteLine(String.Empty);
                Trace.WriteLine("------------------------------------------------------------------------");
            }
        }
    }
}