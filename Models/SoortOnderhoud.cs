using System.Runtime.Serialization;

namespace WPR_project.Models
{
    public enum SoortOnderhoud
    {
        [EnumMember(Value = "onderhoud")]
        onderhoud,
        [EnumMember(Value = "reparatie")]
        reparatie   
    }
}
