using Microsoft.AspNetCore.Identity;

namespace VISI.Models
{
    public class UsuarioViewModel
    {
        public string Email { get; set; }
        //public List<IdentityUserRole<string>> Roles { get; set; }
        public List<string> Roles { get; set; }
        public string Id { get; set; }

    }
}
