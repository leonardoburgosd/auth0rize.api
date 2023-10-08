namespace cens.auth.domain.Entities
{
    public class Person : EntityBase
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime Birthday { get; set; }
    }
}
