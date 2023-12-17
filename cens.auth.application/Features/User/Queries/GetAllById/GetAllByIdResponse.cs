using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cens.auth.application.Features.User.Queries.GetAllById
{
    public class GetAllByIdResponse
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string MotherLastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Avatar { get; set; }
        public bool IsDoubleFactorActive { get; set; }
        public string Type { get; set; }
        public int TypeId { get; set; }
        public List<ApplicationsByUser> Applications { get; set; }
    }

    public class ApplicationsByUser
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}