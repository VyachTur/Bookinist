using System.ComponentModel.DataAnnotations;

namespace Bookinist.DAL.Entities.Base
{
    public abstract class NamedEntity : Entity
    {
        [Required]
        public string Name { get; set; }
    }
}
