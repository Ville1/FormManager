using System.ComponentModel.DataAnnotations.Schema;

namespace FormManager.Data.Models
{
    public class User : IDatabaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
