using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaCuentasBancarias.Models
{
    public class Servicio
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingrese un nombre para el servicio")]
        [Display(Name ="Nombre del servicio")]
        public string Nombre { get; set; }
        [Display(Name = "Orden de Visualización")]
        [Range(1,100, ErrorMessage="El valor debe estar entre 1 y 100")]
        public int? Orden { get; set; }
    }
}
