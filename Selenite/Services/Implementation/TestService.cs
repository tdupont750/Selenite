using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Selenite.Browsers;
using Selenite.Models;
using OpenQA.Selenium;

namespace Selenite.Services.Implementation
{
    public class TestService : ITestService
    {
        private readonly IConfigurationService _configurationService;

        public const string AboutBlank = "about:blank";
        private const string ScreenshotPath = ".\\Screenshots";
        private const string ScreenshotFilenameFormat = "{0}-{1}-{2}.png";

        public TestService(IConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        private void CaptureScreenshot(IWebDriver driver, TestResult testResult)
        {
            var screenshotDriver = driver as ITakesScreenshot;

            if (screenshotDriver != null)
            {
                try
                {
                    var ssFilename = string.Format(ScreenshotFilenameFormat,
                        testResult.CollectionName,
                        testResult.TestName,
                        testResult.DriverType)
                        .Replace("/", "_")
                        .Replace("\\", "_");

                    var path = Path.GetFullPath(ScreenshotPath);
                    var ssPath = Path.Combine(path, ssFilename);

                    testResult.ScreenshotPath = ssPath;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    var screenshot = screenshotDriver.GetScreenshot();
                    screenshot.SaveAsFile(
                        ssPath,
                        ImageFormat.Png);
                }
                catch (Exception screenshotEx)
                {
                    throw new InvalidOperationException(string.Format("Unable to write screenshot to: {0}.", testResult.ScreenshotPath), screenshotEx);
                }
            }            
        }

        public void ExecuteTest(BrowserBase browser, Test test)
        {
            var testResult = new TestResult
                {
                    TestName = test.Name,
                    TestDescription = test.Description,
                    CollectionName = test.TestCollection.File,
                    CollectionDescription = test.TestCollection.Description,
                    Url = test.TestUrl,
                    DriverType = browser.DriverType,
                };

            var traceResult = new StringBuilder();
            traceResult.AppendLine("Collection: " + test.TestCollection.File);
            traceResult.AppendLine("Name: " + test.Name);
            traceResult.AppendLine("Url: " + test.TestUrl);

            try
            {
                var limit = 10;
                while (browser.Driver.Url == AboutBlank)
                {
                    limit--;
                    if (limit <= 0)
                    {
                        throw new InvalidOperationException("Unable to navigate the driver to the given url: " + test.TestUrl);
                    }
                    browser.Driver.Url = test.TestUrl;
                }

                dynamic context = new ExpandoObject();
                context.DriverType = browser.DriverType;

                for (var i = 0; i < test.Commands.Count; i++) 
                {
                    var command = test.Commands[i];
                    Thread.Sleep(100);

                    try
                    {
                        command.Execute(browser.Driver, context);
                    }
                    catch (Exception ex)
                    {
                        CaptureScreenshot(browser.Driver, testResult);

                        string commandJson;
                        try
                        {
                            commandJson = JsonConvert.SerializeObject(
                                command,
                                Formatting.Indented,
                                new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore }
                            );
                        }
                        catch(Exception serializeEx)
                        {
                            commandJson = "Unable to serialize command: " + serializeEx.Message;
                        }
                        var message = String.Format("Test: {0}, Command #{1}: {2}{3}Command Config:{3}{4}{3}{5}",
                            test.Name,
                            i + 1, 
                            command.Name, 
                            Environment.NewLine, 
                            commandJson, 
                            ex.Message); 
                        throw new Exception(message, ex);
                    }
                }

                testResult.Status = ResultStatus.Passed;
                traceResult.AppendLine("Success");
            }
            catch
            {
                CaptureScreenshot(browser.Driver, testResult);

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