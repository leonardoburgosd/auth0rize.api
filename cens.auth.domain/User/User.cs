namespace cens.auth.domain.User;
public class User : EntityBase
{
    public string UserName { get; set; } = null!;
    public byte[] Password { get; set; } = null!;
    public byte[] Salt { get; set; } = null!;
    public bool IsDoubleFactorActivate { get; set; } = false;
    public int TypeId { get; set; } = 0!;
    public int PersonId { get; set; } = 0!;
    public string Avatar { get; set; } = null!;
}
