using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Selenite.Browsers;
using Selenite.Enums;
using Selenite.Models;
using Xunit.Extensions;

namespace Selenite.Tests.Browsers
{
    public class PhantomJs : BrowserBase
    {
        public override DriverType DriverType
        {
            get { return DriverType.PhantomJs; }
        }

#if PHANTOMJS
        [Theory]
#else
        [Theory(Skip = "Not built for PhantomJs")]
#endif
        [PropertyData(TestDataMember)]
        public void ExecuteTests(Test test)
        {
            ExecuteTest(test);
        }
    }
}