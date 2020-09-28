using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartSchool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunoController : ControllerBase
    {

        public List<Aluno> Alunos = new List<Aluno>()
        {
            new Aluno()
            {
                Id = 1,
                Nome = "Adriana",
                Sobrenome = "Barranco",
                Telefone = "30631512"
            },

            new Aluno()
            {
                Id = 2,
                Nome = "Rafael",
                Sobrenome = "Almeida",
                Telefone = "30641614"
            },

            new Aluno()
            {
                Id = 3,
                Nome = "Rosana",
                Sobrenome = "Cardoso",
                Telefone = "30651517"
            },
        };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Alunos);
        }

        // GET: api/<AlunoController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/aluno/5
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Id == id);
            if (aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        // nesse caso tem que passar os parâmetros via QueryString
        // api/aluno/ByName?nome=<nome>&sobrenome=<sobrenome>
        [HttpGet("ByName")]
        public IActionResult GetByName(string nome, string sobrenome)
        {
            var aluno = Alunos.FirstOrDefault(a => a.Nome.Contains(nome) && a.Sobrenome.Contains(sobrenome));
            if (aluno == null) return BadRequest("O aluno não foi encontrado");

            return Ok(aluno);
        }

        // POST api/aluno
        [HttpPost]
        public IActionResult Post(Aluno aluno)
        {
            return Ok(aluno);
        }

        // PUT api/aluno/5
        // utilizado para Update
        [HttpPut("{id}")]
        public IActionResult Put(int id, Aluno aluno)
        {
            return Ok(aluno);
        }

        // atualizar o registro parcialmente
        [HttpPatch("{id}")]
        public IActionResult Patch(int id, Aluno aluno)
        {
            return Ok(aluno);
        }

        // DELETE api/aluno/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
