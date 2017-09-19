using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Converter.Services.Data.Models
{
    public class AnalysisStatus
    {
        private AnalysisStatus(AnalysisStatusEnum @enum)
        {
            Id = (int)@enum;
            Name = @enum.ToString();
        }

        protected AnalysisStatus() { }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public static implicit operator AnalysisStatus(AnalysisStatusEnum @enum) => new AnalysisStatus(@enum);

        public static implicit operator AnalysisStatusEnum(AnalysisStatus analysisStatus) => (AnalysisStatusEnum)analysisStatus.Id;
    }

    public enum AnalysisStatusEnum
    {
        NotStarted,
        InProgress,
        Completed
    }
}
