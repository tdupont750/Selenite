namespace Selenite.Models
{
    public class TestResult
    {
        public bool Passed { get; set; }
        public Test Test { get; set; }
        public string TraceResult { get; set; }
    }
}
