using ByteBank.Forum.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

//Aqui definimos que o tipo que será usado na classe de inicialização do Owin (OwinStartup), será a nossa classe (ByteBank.Forum.Startup)
[assembly: OwinStartup(typeof(ByteBank.Forum.Startup))]
namespace ByteBank.Forum
{
    public class Startup
    {
        //Aqui configuramos todo o contexto do Owin, esse contexto será montado sempre que uma requisição é feita à nossa aplicação (GET,PUT,PATCH,DELETE,POST,etc...)
        public void Configuration(IAppBuilder builder)
        {
            //Aqui definimos o dbContext que o Owin deve usar
            builder.CreatePerOwinContext<DbContext>(() =>
                new IdentityDbContext<UsuarioAplicacao>("DefaultConnection"));

            //Aqui definimos o IUserStore que o Owin deve usar (usamos a interface ao invés da classe para não termos que depender do EntityFramework)
            //Foi utilizada a expressão lambda abaixo para resgatar o mesmo dbContext setado na chamada acima
            builder.CreatePerOwinContext<IUserStore<UsuarioAplicacao>>(
                (opcoes, contextoOwin) =>
                {
                    var dbContext = contextoOwin.Get<DbContext>();
                    return new UserStore<UsuarioAplicacao>(dbContext);
                });

            builder.CreatePerOwinContext<UserManager<UsuarioAplicacao>>(
                (opcoes, contextoOwin) =>
                {
                    var userStore = contextoOwin.Get<IUserStore<UsuarioAplicacao>>();
                    var userManager = new UserManager<UsuarioAplicacao>(userStore);

                    //Classe usada para as validações
                    var userValidator = new UserValidator<UsuarioAplicacao>(userManager);
                    userValidator.RequireUniqueEmail = true; //Propriedade que verifica emails duplicados

                    userManager.UserValidator = userValidator;

                    return userManager;
                });
        }
    }
}