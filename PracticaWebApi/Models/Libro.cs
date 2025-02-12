using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PracticaWebApi.Models
{
    public class Libro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_libro { get; set; }

        [Required]
        [StringLength(255)]
        public string titulo { get; set; }

        [Required]
        public int anio_publicacion { get; set; }

        public int autor_id { get; set; }

        public int categoria_id { get; set; }

        [StringLength(600)]
        public string resumen { get; set; }
    }
}
