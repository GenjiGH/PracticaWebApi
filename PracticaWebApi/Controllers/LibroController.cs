﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticaWebApi.Models;

namespace PracticaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {

        private readonly bibliotecaContext _bibliotecaContext;
        public LibroController(bibliotecaContext bibliotecaContext)
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
            List<Libro> ListadoLibros = (from e in _bibliotecaContext.libro select e).ToList();
            if (ListadoLibros.Count() == 0)
            {
                return NotFound();
            }
            return Ok(ListadoLibros);
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            Libro? libro = (from e in _bibliotecaContext.libro where e.id_libro == id select e).FirstOrDefault();
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarAutor([FromBody] Libro libro)
        {
            try
            {
                _bibliotecaContext.libro.Add(libro);
                _bibliotecaContext.SaveChanges();
                return Ok(libro);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarLibro(int id, [FromBody] Libro libroModificar)
        {
            Libro? libroActual = (from e in _bibliotecaContext.libro where e.id_libro == id select e).FirstOrDefault();
            if (libroActual == null)
            {
                return NotFound();
            }

            libroActual.titulo = libroModificar.titulo;
            libroActual.anio_publicacion = libroModificar.anio_publicacion;
            libroActual.autor_id = libroModificar.autor_id;
            libroActual.categoria_id = libroModificar.categoria_id;
            libroActual.resumen = libroModificar.resumen;

            _bibliotecaContext.Entry(libroActual).State = EntityState.Modified;
            _bibliotecaContext.SaveChanges();

            return Ok(libroModificar);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            Libro? libro = (from e in _bibliotecaContext.libro where e.id_libro == id select e).FirstOrDefault();
            if (libro == null)
            {
                return NotFound();
            }

            _bibliotecaContext.libro.Attach(libro);
            _bibliotecaContext.libro.Remove(libro);
            _bibliotecaContext.SaveChanges();

            return Ok(libro);
        }

        [HttpGet]
        [Route("GetAutoresMasLibros")]
        public IActionResult GetAutoresMasLibros()
        {
            var autores = _bibliotecaContext.libro
                .GroupBy(l => l.autor_id)
                .Select(g => new
                {
                    AutorId = g.Key,
                    Nombre = _bibliotecaContext.autor.Where(a => a.id_autor == g.Key).Select(a => a.nombre).FirstOrDefault(),
                    CantidadLibros = g.Count()
                })
                .OrderByDescending(a => a.CantidadLibros)
                .ToList();

            return Ok(autores);
        }

        [HttpGet]
        [Route("GetLibrosMasRecientes")]
        public IActionResult GetLibrosMasRecientes()
        {
            var libros = _bibliotecaContext.libro
                .OrderByDescending(l => l.anio_publicacion)
                .Take(3) 
                .Select(l => new
                {
                    l.id_libro,
                    l.titulo,
                    l.anio_publicacion,
                    AutorNombre = _bibliotecaContext.autor.Where(a => a.id_autor == l.autor_id).Select(a => a.nombre).FirstOrDefault()
                })
                .ToList();

            return Ok(libros);
        }


        [HttpGet]
        [Route("GetTotalLibrosPorAnio")]
        public IActionResult GetTotalLibrosPorAnio()
        {
            var librosPorAnio = _bibliotecaContext.libro
                .GroupBy(l => l.anio_publicacion)
                .Select(g => new
                {
                    Anio = g.Key,
                    TotalLibros = g.Count()
                })
                .OrderByDescending(g => g.Anio)
                .ToList();

            return Ok(librosPorAnio);
        }


    }
}
