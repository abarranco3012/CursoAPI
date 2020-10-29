using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.API.Data;
using SmartSchool.API.Models;
using SQLitePCL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {

        //private readonly DataContext _context;
        private readonly IRepository _repo;

        public AlunoController(IRepository repo)
        {
            _repo = repo;
            //_context = context;
        }

        //[HttpGet("pegaResposta")]
        //public IActionResult pegaResposta()
        //{
        //    return Ok(_repo.pegaResposta());
        //}

        [HttpGet]
        public IActionResult Get()
        {
            var result = _repo.GetAllAlunos(true);

            return Ok(result);
            //return Ok(_context.Alunos);
        }

        // GET: api/<AlunoController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/aluno/5
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var aluno = _repo.GetAlunoById(id);
            if (aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        // nesse caso tem que passar os parâmetros via QueryString
        // api/aluno/ByName?nome=<nome>&sobrenome=<sobrenome>
        //[HttpGet("ByName")]
        //public IActionResult GetByName(string nome, string sobrenome)
        //{
        //    var aluno = _context.Alunos.FirstOrDefault(a => a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));
        //    if (aluno == null) return BadRequest("O aluno não foi encontrado");

        //    return Ok(aluno);
        //}

        // POST api/aluno
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            _repo.Add(aluno);
            if (_repo.SaveChanges())
            {
                return Ok(aluno);
            }

            return BadRequest("Erro ao cadastrar Aluno");

            //_context.Add(aluno);
            //_context.SaveChanges();
            //return Ok(aluno);
        }

        // PUT api/aluno/5
        // utilizado para Update
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            var alu = _repo.GetAlunoById(id);

            if (alu == null) return BadRequest("Aluno não encontrado !");

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                return Ok(aluno);
            }

            return BadRequest("Erro ao atualizar Aluno");
        }

        // atualizar o registro parcialmente
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            // Busca sem travar o registro
            var alu = _repo.GetAlunoById(id);

            if (alu == null) return BadRequest("Aluno não encontrado !");

            _repo.Update(aluno);
            if (_repo.SaveChanges())
            {
                return Ok(aluno);
            }

            return BadRequest("Erro ao atualizar Aluno");
        }

        // DELETE api/aluno/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var aluno = _repo.GetAlunoById(id);

            if (aluno == null) return BadRequest("Aluno não encontrado !");

            _repo.Delete(aluno);
            if (_repo.SaveChanges())
            {
                return Ok(aluno);
            }

            return BadRequest("Erro ao deletar Aluno");
        }
    }
}
