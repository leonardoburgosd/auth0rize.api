namespace auth0rize.auth.domain.ConfirmAccount
{
    public class ConfirmAccount
    {
        public Guid code { get; set; }
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int UserRegistration { get; set; }
        public DateTime DateUpdate { get; set; }
        public bool IsConfirm { get; set; }

        public virtual User.User User { get; set; }
    }
}
