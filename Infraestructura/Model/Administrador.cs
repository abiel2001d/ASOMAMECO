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
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(AdministradorMetadata))]
    public partial class Administrador
    {
        public int ID_Administrador { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Correo { get; set; }
        public Nullable<int> CodigoVerificacion { get; set; }
        public Nullable<int> ID_Rol { get; set; }
        public string Nombre { get; set; }
    
        public virtual Rol Rol { get; set; }
    }
}
