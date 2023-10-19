namespace cens.auth.domain.User.Business;
public class UserDetail
{
    public int Id { get; set; } = 0!;
    public string UserName { get; set; } = null!;
    public byte[] Password { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string MotherLastName { get; set; } = null!;
    public bool IsDoubleFactorActivate { get; set; }
    public string Avatar { get; set; } = null!;
    public string Detail { get; set; } = null!;
}
