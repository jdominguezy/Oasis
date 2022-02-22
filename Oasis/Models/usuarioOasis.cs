//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Oasis.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class usuarioOasis
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public usuarioOasis()
        {
            this.detalle_roles_usuario = new HashSet<detalle_roles_usuario>();
            this.detalle_usuario_empresa_sucursal = new HashSet<detalle_usuario_empresa_sucursal>();
        }
    
        public int id_usuario { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string nombre { get; set; }
        public string nombre_2 { get; set; }
        public string apellido { get; set; }
        public string apellido_2 { get; set; }
        public string cedula { get; set; }
        public Nullable<int> id_empresa { get; set; }
        public string email { get; set; }
        public int id_departamento { get; set; }
        public string url_foto { get; set; }
        public string comentario { get; set; }
        public Nullable<byte> online { get; set; }
        public string ultima_ip { get; set; }
        public Nullable<bool> indicador_vendedor { get; set; }
        public Nullable<int> id_vendedor { get; set; }
        public byte activo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_roles_usuario> detalle_roles_usuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<detalle_usuario_empresa_sucursal> detalle_usuario_empresa_sucursal { get; set; }
        public virtual empresaOasis empresaOasis { get; set; }
    }
}
