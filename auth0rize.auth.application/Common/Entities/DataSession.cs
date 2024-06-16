namespace auth0rize.auth.application.Common.Entities
{
    public class DataSession
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public long Domain { get; set; }
        public string DomainName { get; set; }
    }
}
