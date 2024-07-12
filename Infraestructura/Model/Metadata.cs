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
        public string Estado_usuario { get; set; }
       [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$", ErrorMessage = "El correo electrónico introducido no tiene un formato válido.")]
        public string Correo { get; set; }
        [Display(Name = "Teléfono")]
        [Required(ErrorMessage = "{0} es un dato requerido")]
        [RegularExpression(@"^\d{4}-?\d{4}$", ErrorMessage = "El número de teléfono no tiene un formato válido.")]
        public string Telefono { get; set; }
    }
}
