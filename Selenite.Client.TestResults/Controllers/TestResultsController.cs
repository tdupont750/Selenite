using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Common.Constants;
using Common.Events;
using Common.Services;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Selenite.Client.TestResults.ViewModels;
using Selenite.Client.TestResults.Views;
using Selenite.Enums;
using Selenite.Models;
using Xunit;

namespace Selenite.Client.TestResults.Controllers
{
    public class TestResultsController : ITestResultsController, ITestMethodRunnerCallback
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISettingsService _settingsService;

        private TestResultsViewModel _viewModel;
        private bool _isCancelRequested;
        private List<ICollectionView> _testResultsViews;

        public TestResultsController(IUnityContainer container, IRegionManager regionManager, IEventAggregator eventAggregator, ISettingsService settingsService)
        {
            _container = container;
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _settingsService = settingsService;
        }

        public void Initialize()
        {
            _viewModel = CreateViewModel();
            _container.RegisterInstance(_viewModel);

            var region = _regionManager.Regions[RegionNames.MainContent];
            var view = _container.Resolve<TestResultsView>();

            region.Add(view);
            region.Activate(view);

            _eventAggregator.GetEvent<ShowTestResultsEvent>().Subscribe(arg => region.Activate(view), ThreadOption.UIThread, true);
        }

        private TestResultsViewModel CreateViewModel()
        {
            var viewModel = new TestResultsViewModel
                {
                    ShowPassed = true,
                    ShowFailed = true,
                    IsRunning = false,
                };
            _testResultsViews = new List<ICollectionView>();

            List<DriverType> enabledBrowsers;

            try
            {
                // TODO: Create a settings service and store the client browser settings there.
                enabledBrowsers = JsonConvert.DeserializeObject<List<DriverType>>(_settingsService.GetEnabledBrowsers())
                    ?? new List<DriverType> { DriverType.PhantomJs };
            }
            catch
            {
                enabledBrowsers = new List<DriverType> { DriverType.PhantomJs };
            }

            foreach (var browser in enabledBrowsers)
            {
                switch (browser)
                {
                    case DriverType.PhantomJs:
                        viewModel.UsePhantomJs = true;
                        break;

                    case DriverType.Firefox:
                        viewModel.UseFirefox = true;
                        break;

                    case DriverType.Chrome:
                        viewModel.UseChrome = true;
                        break;

                    case DriverType.InternetExplorer:
                        viewModel.UseInternetExplorer = true;
                        break;
                }
            }

            viewModel.RunTestsCommand = new DelegateCommand(RunTests, () => AnyBrowserEnabled && !_viewModel.IsRunning);
            viewModel.CancelTestRunCommand = new DelegateCommand(CancelTestRun, () => viewModel.IsRunning && !_isCancelRequested);
            viewModel.ExportToClipboardCommand = new DelegateCommand(ExportToClipboard, () => !_viewModel.IsRunning);

            viewModel.EnabledBrowsersChangedCommand = new DelegateCommand(SaveEnabledBrowsers);
            viewModel.TestResultsFilterChangedCommand = new DelegateCommand(UpdateFilters);

            return viewModel;
        }

        private bool AnyBrowserEnabled
        {
            get
            {
                return _viewModel != null &&
                       (_viewModel.UseFirefox
                        || _viewModel.UseChrome
                        || _viewModel.UseInternetExplorer
                        || _viewModel.UsePhantomJs);
            }
        }

        private void RunTests()
        {
            _viewModel.TimeElapsed = 0;
            _viewModel.PassedTests = 0;
            _viewModel.SkippedTests = 0;
            _viewModel.FailedTests = 0;

            _viewModel.IsRunning = true;
            _viewModel.TestResults.Clear();
            _viewModel.SelectedTestResult = null;

            _testResultsViews.Clear();

            Task.Factory.StartNew(DoRunTests);
        }

        private void CancelTestRun()
        {
            _isCancelRequested = true;
        }

        private void ExportToClipboard()
        {
            var export = new StringBuilder();

            foreach (var collection in _viewModel.TestResults)
            {
                export.Append("Test Collection: ");
                export.AppendLine(collection.Name);

                foreach (var container in collection.TestContainers)
                {
                    export.Append("  ");
                    export.AppendLine(container.Name);

                    foreach (TestResultViewModel result in container.TestResults.SourceCollection)
                    {
                        // TODO: Respect visible filters?
                        export.Append("    Browser: ");
                        export.Append(result.Browser);

                        if (result.Status == ResultStatus.Passed)
                        {
                            export.AppendLine(" - Passed");
                        }
                        else if (result.Status == ResultStatus.Failed)
                        {
                            export.AppendLine(" ***** FAILED *****");
                        }
                        else
                        {
                            export.AppendLine(" // Skipped \\");
                        }

                        export.Append("    Url: ");
                        export.AppendLine(result.Url);

                        export.Append("    Output: ");
                        export.AppendLine(result.ResultOutput);

                        if (!string.IsNullOrWhiteSpace(result.StackTrace))
                        {
                            export.Append("    Stack Trace: ");
                            export.AppendLine(result.StackTrace);
                        }
                    }
                }
            }

            Clipboard.SetText(export.ToString());
        }

        private void UpdateFilters()
        {
            foreach (var view in _testResultsViews)
            {
                view.Filter = TestResultsFilter;
            }
        }

        public bool TestResultsFilter(object param)
        {
            var viewModel = param as TestResultViewModel;

            if (viewModel == null)
                return false;

            return (_viewModel.ShowPassed && viewModel.Status == ResultStatus.Passed)
                   || (_viewModel.ShowFailed && viewModel.Status == ResultStatus.Failed)
                   || (_viewModel.ShowSkipped && viewModel.Status == ResultStatus.Skipped);
        }

        private void AddResult(string collectionName, TestResultViewModel testResultViewModel)
        {
            switch (testResultViewModel.Status)
            {
                case ResultStatus.Failed:
                    _viewModel.FailedTests++;
                    break;

                case ResultStatus.Passed:
                    _viewModel.PassedTests++;
                    break;

                case ResultStatus.Skipped:
                    _viewModel.SkippedTests++;
                    break;
            }

            var collection = _viewModel.TestResults.FirstOrDefault(c => c.Name == collectionName);

            if (collection != null)
            {
                var container = collection.TestContainers.FirstOrDefault(c => c.Name == testResultViewModel.Name);

                if (container != null)
                {
                    var sourceCollection = container.TestResults.SourceCollection as ObservableCollection<TestResultViewModel>;
                    if (sourceCollection != null)
                        sourceCollection.Add(testResultViewModel);
                }
                else
                {
                    collection.TestContainers.Add(new TestResultContainerViewModel
                    {
                        Name = testResultViewModel.Name,
                        TestResults = GetTestView(testResultViewModel),
                        Description = testResultViewModel.Description
                    });
                }
            }
            else
            {
                // Test Collection didn't exist so add it.
                _viewModel.TestResults.Add(new TestResultCollectionViewModel
                {
                    Name = collectionName,
                    Description = testResultViewModel.CollectionDescription,
                    TestContainers = new ObservableCollection<TestResultContainerViewModel>
                            {
                                new TestResultContainerViewModel
                                    {
                                        Name = testResultViewModel.Name,
                                        Description = testResultViewModel.Description,
                                        TestResults = GetTestView(testResultViewModel),
                                    }
                            }
                });
            }
        }

        private ICollectionView GetTestView(TestResultViewModel testResult)
        {
            var testResults = new ObservableCollection<TestResultViewModel>
                {
                    testResult
                };

            var testResultsView = CollectionViewSource.GetDefaultView(testResults);
            testResultsView.Filter = TestResultsFilter;

            _testResultsViews.Add(testResultsView);

            return testResultsView;
        }

        private void SaveEnabledBrowsers()
        {
            ((DelegateCommand)_viewModel.RunTestsCommand).RaiseCanExecuteChanged();

            var browsers = new List<DriverType>();

            if (_viewModel.UseFirefox)
            {
                browsers.Add(DriverType.Firefox);
            }

            if (_viewModel.UseChrome)
            {
                browsers.Add(DriverType.Chrome);
            }

            if (_viewModel.UseInternetExplorer)
            {
                browsers.Add(DriverType.InternetExplorer);
            }

            if (_viewModel.UsePhantomJs)
            {
                browsers.Add(DriverType.PhantomJs);
            }

            _settingsService.SetEnabledBrowsers(JsonConvert.SerializeObject(browsers));
        }

        #region Test Runner Helpers

        public void DoRunTests()
        {
            _isCancelRequested = false;

            if (Directory.Exists("./Screenshots"))
            {
                var screenshotDirectory = new DirectoryInfo("./Screenshots");
                foreach (var file in screenshotDirectory.GetFiles("*.png"))
                {
                    file.Delete();
                }
            }

            var executingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            try
            {
                using (var wrapper = new ExecutorWrapper(executingAssembly.Location, null, true))
                {
                    var assembly = TestAssemblyBuilder.Build(wrapper);
                    var methods = assembly
                        .EnumerateTestMethods()
                        .ToList();

                    var activeMethods = methods
                        .Where(m =>
                               _viewModel.UseFirefox && m.TestClass.TypeName.Contains("Firefox") ||
                               _viewModel.UseChrome && m.TestClass.TypeName.Contains("Chrome") ||
                               _viewModel.UseInternetExplorer && m.TestClass.TypeName.Contains("Internet") ||
                               _viewModel.UsePhantomJs && m.TestClass.TypeName.Contains("Phantom")
                        )
                        .ToList();

                    if (activeMethods.Count > 0)
                    {
                        assembly.Run(activeMethods, this);
                    }
                    else
                    {
                        _viewModel.IsRunning = false;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                Application.Current.Dispatcher.BeginInvoke((Action)(DoDone));
            }
        }

        private void DoProcessTest(TestMethod testMethod)
        {
            var lastRunResult = testMethod.RunResults.Last();

            if (lastRunResult is TestPassedResult)
                TestPassed(lastRunResult as TestPassedResult);
            else if (lastRunResult is TestFailedResult)
                TestFailed(lastRunResult as TestFailedResult);
            else
                TestSkipped(lastRunResult as TestSkippedResult);
        }

        private void TestPassed(TestPassedResult testResult)
        {
            var result = JsonConvert.DeserializeObject<Models.TestResult>(testResult.Output);
            var testResultViewModel = new TestResultViewModel
            {
                Status = result.Status,
                Name = result.TestName,
                Description = result.TestDescription,
                CollectionDescription = result.CollectionDescription,
                Url = result.Url,
                ScreenshotPath = result.ScreenshotPath,
                ResultOutput = result.TraceResult,
                Browser = result.DriverType.Description(),
            };

            var collectionName = result.CollectionName;

            AddResult(collectionName, testResultViewModel);
        }

        private void TestFailed(TestFailedResult testResult)
        {
            if (string.IsNullOrEmpty(testResult.Output))
            {
                var errorResultViewModel = new TestResultViewModel
                {
                    Status = ResultStatus.Failed,
                    Name = "Error",
                    Description = "An unknown error occured executing the test.",
                    CollectionDescription = "Error",
                    ResultOutput = testResult.ExceptionMessage,
                    StackTrace = testResult.ExceptionStackTrace,
                    Browser = DriverType.Unknown.ToString(),
                };

                AddResult("Test Execution Failure", errorResultViewModel);
                return;
            }

            var result = JsonConvert.DeserializeObject<Models.TestResult>(testResult.Output);
            var testResultViewModel = new TestResultViewModel
            {
                Status = result.Status,
                Name = result.TestName,
                Description = result.TestDescription,
                CollectionDescription = result.CollectionDescription,
                Url = result.Url,
                ScreenshotPath = result.ScreenshotPath,
                ResultOutput = testResult.ExceptionMessage + Environment.NewLine + result.TraceResult,
                StackTrace = testResult.ExceptionStackTrace,
                Browser = result.DriverType.Description(),
            };

            var collectionName = result.CollectionName;

            AddResult(collectionName, testResultViewModel);
        }

        private void TestSkipped(TestSkippedResult testResult)
        {
            // CCHINN: Add support for skipped tests.
            //var result = JsonConvert.DeserializeObject<Models.TestResult>(testResult.Output);
            //var testResultViewModel = new TestResultViewModel
            //{
            //    Status = result.Status,
            //    Name = result.Test.Name,
            //        Url = result.Url,
            //    ResultOutput = testResult.Reason,
            //};

            //var collectionName = result.Test.CollectionName;

            //AddResult(collectionName, testResultViewModel);
        }

        public void DoDone()
        {
            CommandManager.InvalidateRequerySuggested();
            _viewModel.IsRunning = false;
        }

        public void DoExceptionThrown(TestAssembly testAssembly, Exception exception)
        {
            var testResultViewModel = new TestResultViewModel
            {
                Status = ResultStatus.Failed,
                Name = "Test Runner Error",
                StackTrace = exception.StackTrace,
                ResultOutput = exception.Message,
                Browser = "Unknown",
            };

            AddResult("Test Failure", testResultViewModel);
        }

        #endregion

        #region ITestMethodRunnerCallback Implementation
        public void AssemblyFinished(TestAssembly testAssembly, int total, int failed, int skipped, double time)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                _viewModel.PassedTests = total - failed - skipped;
                _viewModel.FailedTests = failed;
                _viewModel.SkippedTests = skipped;
                _viewModel.TimeElapsed = time;
            }));
        }

        public void AssemblyStart(TestAssembly testAssembly)
        {
        }

        public bool ClassFailed(TestClass testClass, string exceptionType, string message, string stackTrace)
        {
            return true;
        }

        public void ExceptionThrown(TestAssembly testAssembly, System.Exception exception)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => DoExceptionThrown(testAssembly, exception)));
        }

        public bool TestFinished(TestMethod testMethod)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() => DoProcessTest(testMethod)));
            return !_isCancelRequested;
        }

        public bool TestStart(TestMethod testMethod)
        {
            return !_isCancelRequested;
        }
        #endregion
    }
}
