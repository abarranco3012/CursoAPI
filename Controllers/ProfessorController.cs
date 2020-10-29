using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Models;

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {

        private readonly IRepository _repo;

        public ProfessorController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllProfessores(true);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var professor = _repo.GetProfessotById(id);
            if (professor == null) return BadRequest("Professor não encontrado !");

            return Ok(professor);
        }

        // nesse caso tem que passar os parâmetros via QueryString
        // api/aluno/ByName?nome=<nome>
        //[HttpGet("byName")]
        //public IActionResult GetByName(string nome)
        //{
        //    var professor = _context.Professores.FirstOrDefault(a => a.Nome.Contains(nome));
        //    if (professor == null) return BadRequest("Professor não encontrado !");

        //    return Ok(professor);
        //}

        [HttpPost]
        public IActionResult Post(Professor professor)
        {

            _repo.Add(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("Erro ao cadastrar Professor");

            //_context.Add(professor);
            //_context.SaveChanges();
            //return Ok(professor);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Professor professor)
        {
            var prof = _repo.GetProfessotById(id);

            if (prof == null) return BadRequest("Professor não encontrado !");

            _repo.Update(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("Erro ao atualizar Professor");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Professor professor)
        {
            var prof = _repo.GetProfessotById(id);

            if (prof == null) return BadRequest("Professor não encontrado !");

            _repo.Update(professor);
            if (_repo.SaveChanges())
            {
                return Ok(professor);
            }

            return BadRequest("Erro ao atualizar Professor");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var professor = _repo.GetProfessotById(id);
            if (professor == null) return BadRequest("Professor não encontrado");

            _repo.Delete(professor);
            if (_repo.SaveChanges())
            {
                return Ok("Professor apagado");
            }

            return BadRequest("Erro ao apagar Professor");
        }
    }
}
