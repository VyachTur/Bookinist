namespace Bookinist.DAL.Entities.Base
{
    public abstract class Person : NamedEntity
    {
        public string Surname { get; set; }

        public string Patronymic { get; set; }
    }
}
