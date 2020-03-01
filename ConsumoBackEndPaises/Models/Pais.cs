using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ConsumoBackEndPaises.Models
{
    public class Pais
    {
        [Key]
        [Required]
        public int idPais { get; set; }
        [Required]
        public string NombrePais { get; set; }
        public string descripcion { get; set; }

        public ICollection<Departamento> departamentos { get; set; }
    }
}