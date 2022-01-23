using Bookinist.DAL.Entities.Base;
using System.Collections.Generic;

namespace Bookinist.DAL.Entities
{
    public class Category : NamedEntity
    {
        public virtual ICollection<Book> Books { get; set; }
    }
}
