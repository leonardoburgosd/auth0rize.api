namespace cens.auth.domain.Entities
{
    public class User : EntityBase
    {
        public string UserName { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public bool IsDoubleFactorActivate { get; set; }
        public int TypeId { get; set; }
        public int PersonId { get; set; }
        public string Avatar { get; set; }
    }
}
