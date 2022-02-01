using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Oasis.Models.Login
{
    public class CustomMemberShipUser : MembershipUser
    {
        #region User Properties

        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public ICollection<roles> roles { get; set; }

        #endregion

        public CustomMemberShipUser(usuarioOasis user) : base("CustomMembership", user.username, user.id_usuario, user.email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            id_usuario = user.id_usuario;
            nombre = UppercaseFirst(user.nombre.ToLower());
            apellido = UppercaseFirst(user.apellido.ToLower());
            roles = user.detalle_roles_usuario.Select(x=>x.roles).ToList();
        }

        static string UppercaseFirst(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}