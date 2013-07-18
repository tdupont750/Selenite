using System;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Selenite.Browsers;
using Selenite.Models;

namespace Selenite.Services.Implementation
{
    public class TestService : ITestService
    {
        public const string AboutBlank = "about:blank";

        public void ExecuteTest(BrowserBase browser, Test test)
        {
            var testResult = new TestResult
                {
                    TestName = test.Name,
                    CollectionName = test.CollectionName,
                    Url = test.TestUrl,
                    DriverType = browser.DriverType,
                };

            var traceResult = new StringBuilder();
            traceResult.AppendLine("Collection: " + test.CollectionName);
            traceResult.AppendLine("Name: " + test.Name);
            traceResult.AppendLine("Url: " + test.TestUrl);

            try
            {
                browser.Driver.Url = test.TestUrl;

                // Hack, sometimes the set does not work?
                if (browser.Driver.Url == AboutBlank)
                    browser.Driver.Url = test.TestUrl;

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

                testResult.Status = ResultStatus.Passed;
                traceResult.AppendLine("Success");
            }
            catch
            {
                testResult.Status = ResultStatus.Failed;
                traceResult.AppendLine(String.Empty);
                traceResult.AppendLine("***** FAILURE *****");
                throw;
            }
            finally
            {
                traceResult.AppendLine(String.Empty);
                traceResult.AppendLine("------------------------------------------------------------------------");

                testResult.TraceResult = traceResult.ToString();

                var testResultJson = JsonConvert.SerializeObject(testResult);
                Trace.WriteLine(testResultJson);
            }
        }
    }
}