using Entidades.Modelos;
using Microsoft.AspNetCore.Identity;

namespace MatriculaPUCRS.Models
{
    public class ApplicationUser : IdentityUser
    {
        public long? EstudanteId { get; set; }
    }
}
