namespace cens.auth.domain.User.Business;
public class UserGet
{
    public int Id { get; set; } = 0!;
    public string NameComplete { get; set; } = null!;
    public string Application { get; set; } = null!;
    public string Icon { get; set; } = null!;
    public bool Delete { get; set; }
}
