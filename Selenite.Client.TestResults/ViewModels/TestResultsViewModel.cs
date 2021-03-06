﻿using System.Collections.ObjectModel;
using System.Windows.Input;
using Common.Extensions;
using Common.ViewModels;
using Microsoft.Practices.Prism.Commands;

namespace Selenite.Client.TestResults.ViewModels
{
    public class TestResultsViewModel : ViewModelBase
    {
        public TestResultsViewModel()
        {
            TestResults = new ObservableCollection<TestResultCollectionViewModel>();
        }

        public ObservableCollection<TestResultCollectionViewModel> TestResults { get; set; }
        public ViewModelBase SelectedTestResult
        {
            get { return Get(() => SelectedTestResult); }
            set { Set(value, () => SelectedTestResult); }
        }

        public int TotalTests
        {
            get { return PassedTests + FailedTests + SkippedTests; }
        }

        public int PassedTests
        {
            get { return Get(() => PassedTests); }
            set { Set(value, () => PassedTests, () => TotalTests); }
        }

        public int FailedTests
        {
            get { return Get(() => FailedTests); }
            set { Set(value, () => FailedTests, () => TotalTests); }
        }

        public int SkippedTests
        {
            get { return Get(() => SkippedTests); }
            set { Set(value, () => SkippedTests, () => TotalTests); }
        }

        public double TimeElapsed
        {
            get { return Get(() => TimeElapsed); }
            set { Set(value, () => TimeElapsed); }
        }

        public bool IsRunning
        {
            get { return Get(() => IsRunning); }
            set
            {
                Set(value, () => IsRunning);

                RunTestsCommand.RaiseCanExecuteChanged();
                CancelTestRunCommand.RaiseCanExecuteChanged();
            }
        }

        public bool UseFirefox
        {
            get { return Get(() => UseFirefox); }
            set
            {
                Set(value, () => UseFirefox);

                EnabledBrowsersChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool UseChrome
        {
            get { return Get(() => UseChrome); }
            set
            {
                Set(value, () => UseChrome);

                EnabledBrowsersChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool UseInternetExplorer
        {
            get { return Get(() => UseInternetExplorer); }
            set
            {
                Set(value, () => UseInternetExplorer);

                EnabledBrowsersChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool UsePhantomJs
        {
            get { return Get(() => UsePhantomJs); }
            set
            {
                Set(value, () => UsePhantomJs);

                EnabledBrowsersChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool ShowPassed
        {
            get { return Get(() => ShowPassed); }
            set
            {
                Set(value, () => ShowPassed);

                TestResultsFilterChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool ShowFailed
        {
            get { return Get(() => ShowFailed); }
            set
            {
                Set(value, () => ShowFailed);

                TestResultsFilterChangedCommand.VerifyAndExecute(value);
            }
        }

        public bool ShowSkipped
        {
            get { return Get(() => ShowSkipped); }
            set
            {
                Set(value, () => ShowSkipped);

                TestResultsFilterChangedCommand.VerifyAndExecute(value);
            }
        }

        #region Commands

        public ICommand RunTestsCommand { get; set; }

        public ICommand CancelTestRunCommand { get; set; }

        public ICommand ExportToClipboardCommand { get; set; }

        public ICommand EnabledBrowsersChangedCommand { get; set; }

        public ICommand TestResultsFilterChangedCommand { get; set; }

        #endregion
    }
}
