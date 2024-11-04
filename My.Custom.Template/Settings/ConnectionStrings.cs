using System.ComponentModel.DataAnnotations;

namespace My.Custom.Template.Settings;

public class ConnectionStrings
{
    [Required]
    public string DefaultConnection { get; set; }
}