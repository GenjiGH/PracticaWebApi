using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaWebApi.Models;

namespace PracticaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {

        private readonly bibliotecaContext _bibliotecaContext;
        public AutorController(bibliotecaContext bibliotecaContext)
        {
            _bibliotecaContext = bibliotecaContext;
        }

        /// <summary>
        /// ENdpoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<Autor> ListadoAutores = (from e in _bibliotecaContext.autor select e).ToList();
            if (ListadoAutores.Count() == 0)
            {
                return NotFound();
            }
            return Ok(ListadoAutores);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            Autor? autor = (from e in _bibliotecaContext.autor where e.id_autor == id select e).FirstOrDefault();
            if (autor == null)
            {
                return NotFound();
            }
            return Ok(autor);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarAutor([FromBody] Autor autor)
        {
            try
            {
                _bibliotecaContext.autor.Add(autor);
                _bibliotecaContext.SaveChanges();
                return Ok(autor);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] Autor autorModificar)
        {
            Autor? autorActual = (from e in _bibliotecaContext.autor where e.id_autor == id select e).FirstOrDefault();
            if (autorActual == null)
            {
                return NotFound();
            }

            autorActual.nombre = autorModificar.nombre;
            autorActual.nacionalidad = autorModificar.nacionalidad;

            _bibliotecaContext.Entry(autorActual).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();

            return Ok(autorModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarAutor(int id)
        {
            Autor? autor = (from e in _bibliotecaContext.autor where e.id_autor == id select e).FirstOrDefault();
            if (autor == null)
            {
                return NotFound();
            }

            _bibliotecaContext.autor.Attach(autor);
            _bibliotecaContext.autor.Remove(autor);
            _bibliotecaContext.SaveChanges();

            return Ok(autor);
        }

        [HttpGet]
        [Route("VerificarAutorTieneLibros/{autorId}")]
        public IActionResult VerificarAutorTieneLibros(int autorId)
        {
            bool tieneLibros = _bibliotecaContext.libro.Any(l => l.autor_id == autorId);

            return Ok(new { AutorId = autorId, TieneLibros = tieneLibros });
        }

        [HttpGet]
        [Route("GetPrimerLibroAutor/{autorId}")]
        public IActionResult GetPrimerLibroAutor(int autorId)
        {
            var primerLibro = _bibliotecaContext.libro
                .Where(l => l.autor_id == autorId)
                .OrderBy(l => l.anio_publicacion)
                .Select(l => new
                {
                    l.id_libro,
                    l.titulo,
                    l.anio_publicacion
                })
                .FirstOrDefault();

            if (primerLibro == null)
            {
                return NotFound();
            }

            return Ok(primerLibro);
        }
    }
}
