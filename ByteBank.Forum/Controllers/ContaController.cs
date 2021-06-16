using ByteBank.Forum.Models;
using ByteBank.Forum.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ByteBank.Forum.Controllers
{
    public class ContaController : Controller
    {
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registrar(ContaRegistrarViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var dbContext = new IdentityDbContext<UsuarioAplicacao>("DefaultConnection");
                //A classe que conversa com o dbContext (banco de dados) é a UserStore
                //Possui uma propriedade boolean chamada AutoSaveChanges, quando está está marcada como True, sempre que dermos um userStore.Create/userStore.Update/userStore.Delete, automaticamente vai salvar no dbContext (banco de dados)
                //O usarmos as classes IdentityDbContext, UserStore e a UserManager, nossa aplicação independe do tipo de banco de dados que está sendo utilizado,
                //pois o banco será definido na ConnectionString passada por parâmetro no construtor do IdentityDbContext, podendo ser SQL Server, MySQl, PL\SQL entre outros
                var userStore = new UserStore<UsuarioAplicacao>(dbContext);
                var userManager = new UserManager<UsuarioAplicacao>(userStore);

                var novoUsuario = new UsuarioAplicacao();

                novoUsuario.Email = modelo.Email;
                novoUsuario.UserName = modelo.UserName;
                novoUsuario.NomeCompleto = modelo.NomeCompleto;

                //Alterado o Create por CreateAsync, para que a operação seja assíncrona, foi necessário também adicionar o await na chamada do método,
                //e na declaração dessa função aqui, adicionamos o async e mudamos o tipo de ActionResult para Task<ActionResult>
                await userManager.CreateAsync(novoUsuario, modelo.Senha);
                //Podemos incluir o usuário
                return RedirectToAction("Index", "Home");
            }

            //Alguma coisa de errado aconteceu!
            return View(modelo);
        }
    }
}