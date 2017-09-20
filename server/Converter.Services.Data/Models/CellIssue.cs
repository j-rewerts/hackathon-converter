namespace Converter.Services.Data.Models
{
    internal class CellIssue : IssueBase
    {
        public string CellReference { get; set; }
        public Worksheet Worksheet { get; set; }
    }
}
