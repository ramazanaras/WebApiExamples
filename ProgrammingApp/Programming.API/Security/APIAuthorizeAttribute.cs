using Programming.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace Programming.API.Security
{
    //yetki işlemleri(Authorize) için
    public class APIAuthorizeAttribute : AuthorizeAttribute
    {

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            string actionRoles = Roles;//actiondaki [APIAuthorizeAttribute(Roles = "A,U")] rolleri getirir.

            var userName = HttpContext.Current.User.Identity.Name; //kullanıcının usernameini aldık(apiKey ile uyuşan  user)
            UserDAL userDal = new UserDAL();

            var user = userDal.GetUserByName(userName); //user varmı
            if (user != null && actionRoles.Contains(user.Role)) //userın rolü içeriyorsa
            {

            }
            else
            {
                //yetkisi olmayacak ve actiona metoduna düşemeyecek 
                actionContext.Response = new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized);
            }


        }
    }
}