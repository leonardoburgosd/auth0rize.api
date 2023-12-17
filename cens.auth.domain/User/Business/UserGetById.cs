namespace cens.auth.domain.User.Business;
public class UserGetById
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime Birthday { get; set; }
    public string MotherLastName { get; set; }
    public string Avatar { get; set; }
    public bool IsDoubleFactorActivate { get; set; }
    public int TypeId { get; set; }
    public string UserName { get; set; }
}
