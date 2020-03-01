using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsumoBackEndPaises.Models
{
    public class Departamento
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public string nombre { get; set; }
        public string descripcion { get; set; }
        [Required]
        public int idPais { get; set; }

        public Pais pais { get; set; }
    }
}