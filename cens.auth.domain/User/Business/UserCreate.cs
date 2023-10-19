namespace cens.auth.domain.User.Business;
public class UserCreate
{
    public string UserName { get; set; } = null!;
    public string Avatar { get; set; } = null!;
    public byte[] Password { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string MotherLastName { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public string RegistrationUser { get; set; } = null!;
}
