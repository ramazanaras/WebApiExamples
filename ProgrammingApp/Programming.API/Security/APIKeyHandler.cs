using Programming.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Programming.API.Security
{
    public class APIKeyHandler : DelegatingHandler
    {
        //Authorize attributenü kullandığımız için bu metoda düştük.
        ////
        protected override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            var queryString = request.RequestUri.ParseQueryString();

            //http://localhost:35207/api/languages?apiKey=c0a3e4da-9400-4caf-9729-88e8622beb3f
            var apiKey = queryString["apiKey"];

            //var apiKey = request.Headers.GetValues("apiKey").FirstOrDefault();//diğer yöntem (fiddlerda Header kısmında gönderdiğimizde değeri alır.)

            UserDAL userDAL = new UserDAL();
            var user = userDAL.GetUserByApiKey(apiKey);//verdiğimiz Userkeyde veritabanında bir user var mı

            if (user!=null)
            {

                //geçerli bir kullanıcı bir istekte bulunursa bu metoda düş

                //bu kodlar ile authorize işlemini aşmış olacak ve Get() Actionına gidebiliceğiz.(breakpoint ile düşecek).yani geçerli bir kullanıcı istekte bulunursa actiona düşer.
                var principal = new ClaimsPrincipal(new GenericIdentity(user.Name, "APIKEY"));
                HttpContext.Current.User = principal;
            }
            else
            {

            }


            return base.SendAsync(request, cancellationToken);
        }

    }
}