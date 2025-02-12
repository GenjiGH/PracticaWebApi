using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PracticaWebApi.Models
{
    public class Autor
    {

        [Key]  // Define que esta es la clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_autor { get; set; }

        [Required]
        [StringLength(100)]

        public string nombre { get; set; }

        [StringLength(100)]
        public string nacionalidad { get; set; }
    }
}
