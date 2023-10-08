namespace cens.auth.domain.Entities
{
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string RegistrationUser { get; set; }
        public DateTime ModificationDate { get; set; }
        public string ModificationUser { get; set; }
        public DateTime DeleteDate { get; set; }
        public string DeleteUser { get; set; }
        public bool Delete { get; set; }
    }
}
