namespace cens.auth.domain.Person;
public class Person : EntityBase
{
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MotherLastName { get; set; } = null!;
    public DateTime Birthday { get; set; }
}
