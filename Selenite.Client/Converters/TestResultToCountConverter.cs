using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using Selenite.Client.ViewModels.WebAutomation;
using Selenite.Models;

namespace Selenite.Client.Converters
{
    public class TestResultToCountConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value produced by the binding source.</param><param name="targetType">The type of the binding target property.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var resultView = value as ListCollectionView;

            if (resultView == null)
                return string.Empty;

            var results = resultView.SourceCollection as IEnumerable<TestResultViewModel>;

            if (results == null)
                return string.Empty;

            int passed = 0, failed = 0, skipped = 0;

            foreach (var test in results)
            {
                if (test.Status == ResultStatus.Passed) passed++;
                if (test.Status == ResultStatus.Failed) failed++;
                if (test.Status == ResultStatus.Skipped) skipped++;
            }

            var resultText = new List<string>();
            if (passed > 0) resultText.Add(string.Format("{0} passed", passed));
            if (failed > 0) resultText.Add(string.Format("{0} failed", failed));
            if (skipped > 0) resultText.Add(string.Format("{0} skipped", skipped));

            return string.Format("- {0}", string.Join("; ", resultText));
        }

        /// <summary>
        /// Converts a value. 
        /// </summary>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        /// <param name="value">The value that is produced by the binding target.</param><param name="targetType">The type to convert to.</param><param name="parameter">The converter parameter to use.</param><param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
