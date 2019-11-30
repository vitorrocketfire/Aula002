using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using aula2.DAO;
using aula2.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace aula2.Controllers
{
    [Route("api/Endereco")]
    [ApiController]
    public class EnderecoController : Controller
    {
        private readonly EnderecoDAO _enderecoDAO;
        public EnderecoController(EnderecoDAO enderecoDAO)
        {
            _enderecoDAO = enderecoDAO;
        }
        public IActionResult Index()
        {
            ViewBag.Data = DateTime.Now;
            return View(_enderecoDAO.Listar());

        }

        public IActionResult CadastrarEndereco()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CadastrarEndereco(Endereco e)
        {
            string url =
                @"https://viacep.com.br/ws/" + e.Cep + "/json/";
            WebClient client = new WebClient();
            string resultado = client.DownloadString(url);
            Endereco enderecoNovo =
                JsonConvert.DeserializeObject<Endereco>(resultado);

            _enderecoDAO.Cadastrar(enderecoNovo);


            return RedirectToAction("Index");


        }
        [HttpGet("Enderecos")]
        public List<Endereco> ListAll()
        {
            return _enderecoDAO.Listar();
        }
        [HttpGet("Enderecos/{cep}")]
        public List<Endereco>ListCep(string cep)
        {
            List<Endereco> allEnderecos = _enderecoDAO.Listar();
            List<Endereco> enderecos = new List<Endereco>();
            foreach (var endereco in allEnderecos)
            {
                if (endereco.Cep.Equals(cep))
                {
                    enderecos.Add(endereco);
                }
            }
            return enderecos;
        }
        [HttpGet("EnderecosPorEstado/{uf}")]
        public List<Endereco> ListEstado(string uf)
        {
            List<Endereco> allEnderecos = _enderecoDAO.Listar();
            List<Endereco> enderecos = new List<Endereco>();
            foreach (var endereco in allEnderecos)
            {
                if (endereco.Uf.Equals(uf))
                {
                    enderecos.Add(endereco);
                }
            }
            return enderecos;
        }
    }
}