//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infraestructura.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Usuario()
        {
            this.Asistencia = new HashSet<Asistencia>();
            this.Invitacion = new HashSet<Invitacion>();
        }
    
        public int Id_Usuario { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Estado_usuario { get; set; }
        public string Estado_2 { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Asistencia> Asistencia { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Invitacion> Invitacion { get; set; }
    }
}
