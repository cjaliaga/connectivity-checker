using System.ComponentModel.DataAnnotations;

namespace ConnectivityChecker.Models
{
    public class ConnectionString
    {
        [Required]
        public string Value { get; set; }
    }
}
