using Bookinist.DAL.Entities.Base;

namespace Bookinist.DAL.Entities
{
    public class Book : NamedEntity
    {
        public virtual Category Category { get; set; }
    }
}
