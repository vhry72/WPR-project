using System.ComponentModel.DataAnnotations;
using WPR_project.Models;
using Xunit.Sdk;

namespace WPR_project.DTO_s
{
    public class AbonnementDTO
    {

        [Required]
            public Guid AbonnementId { get; set; }
        [Required]
            public DateTime vervaldatum { get; set; }

        [Required]
            public AbonnementType Type { get; set; }

            [Range(0, double.MaxValue, ErrorMessage = "Bedrag moet positief zijn.")]
            public decimal Bedrag { get; set; }
    }
}
