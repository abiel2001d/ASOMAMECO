using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Model
{
    internal partial class UsuarioMetadata
    {

        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Nombre { get; set; }
        [Display(Name = "Cédula")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^\d{1,3}-?\d{3,4}-?\d{4}$", ErrorMessage = "El número de cédula no tiene un formato válido.")]
        public string Cedula { get; set; }
        [Display(Name = "Estado miembro")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Estado_usuario { get; set; }
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "El correo electrónico introducido no tiene un formato válido.")]
        public string Correo { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^\d{4}-?\d{4}$", ErrorMessage = "El número de teléfono no tiene un formato válido.")]
        public string Telefono { get; set; }
    }

    internal partial class EventoMetadata
    {
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Display(Name = "Nombre del Evento")]
        public string Nombre_Evento { get; set; }
        [Display(Name = "Fecha del Evento")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public DateTime Fecha_Evento { get; set; }
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "{0} es un dato requerido")]
        public string Lugar { get; set; }
    }

    internal class AdministradorMetadata
    {
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres.", MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "La contraseña debe tener al menos 8 caracteres, un número y una mayúscula.")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "El correo electrónico introducido no tiene un formato válido.")]
        public string Correo { get; set; }

        public Nullable<int> CodigoVerificacion { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Display(Name = "Nombre Completo")]
        public Nullable<int> ID_Rol { get; set; }

        [Required(ErrorMessage = "{0} es un dato requerido")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }
    }
}
