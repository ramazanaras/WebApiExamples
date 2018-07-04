using Programming.API.Attributes;
using Programming.API.Security;
using Programming.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Programming.API.Controllers
{
    [ApiExceptionAttribute] //herhangi bir controllerda hata olduğunda bizi bu sınıfa yollayaca

    ////[APIAuthorizeAttribute(Roles = "A")] //sadece admin görüntüler (Tüm actionlarda geçerli olur )
    public class LanguagesController : ApiController
    {
        LanguagesDAL languageDAL = new LanguagesDAL();

        //http://localhost:35207/api/Languages
        [HttpGet]
        //[NonAction] //buraya artık istekte bulunamazsın  //dışarıya açılmasını kapattık
        [ApiExceptionAttribute] //kendi attributemüz  //actionda bir hata oluştuğunda class a gidip işlemler yapıcak.yani her actionda try catch kullanmaya gerek olmayacak

        [Authorize] // geçerli bir kullanıcı(yani veritabanında mevcut olan bir kullancıı) bir istekte bulunursa bu metoda düş
        //[APIAuthorizeAttribute(Roles = "A")] //sadece admin görüntüler
        public HttpResponseMessage Get()
        {
            //throw new Exception("Sıfıra bölünemez");
            var languages = languageDAL.GetAllLanguages();

            return Request.CreateResponse(HttpStatusCode.OK, languages);//geriye Response döndürüyoruz.Frontendci bu response a göre client tarafında işlem yapabilir.


        }


        //[ResponseType(typeof(IEnumerable<Language>))] //geriye ne döndürdüğünü söyleyebiliriz.
        //public IHttpActionResult Get()
        //{
        //    var languages = languageDAL.GetAllLanguages();

        //    //   return Request.CreateResponse(HttpStatusCode.OK, languages);//geriye Response döndürüyoruz.Frontendci bu response a göre client tarafında işlem yapabilir.
        //    return Ok(languages); //yukarıdaki metotla   aynı mantıkta çalışıyor (WebApi2 ile gelen özellik)

        //}



        //http://localhost:35207/api/Languages/1
        [HttpGet]
        //A ve U rolüne ait kullanıclar bu actiona erişebilir
        [APIAuthorizeAttribute(Roles = "A,U")]  //KENDİ YAZDIĞIMIZ ATTRIBUTE (YETKI ATTRİBUTE Ü)  yetkisi yoksa erişemesin varsa erişssin 
        //http://localhost:35207/api/languages?apiKey=c0a3e4da-9400-4caf-9729-88e8622beb3f%20&id=2 bu şekilde yazınca düşebilirsin
        public HttpResponseMessage Get(int id)
        {
            try
            {
                var language = languageDAL.GetLanguageById(id);
                if (language == null)
                {
                    //eğer veri bulunamadı ise geriye 404 döndürürüz.bu response a göre client tarafında işlem yapılabilir.
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Böyle bir kayıt bulunamadı"); //404 döner (NotFound) 
                }

                return Request.CreateResponse(HttpStatusCode.OK, language);  //200 döner (OK)
            }
            catch (Exception ex)
            {
                //WEb apide hata Yönetimi

                HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.BadGateway);
                errorResponse.ReasonPhrase = ex.Message;

                throw new HttpResponseException(errorResponse);
            }
        }

        //[ResponseType(typeof(Language))]  //geriye ne döndürdüğünü söyleyebiliriz.
        //public IHttpActionResult Get(int id)
        //{
        //    var language = languageDAL.GetLanguageById(id);
        //    if (language == null)
        //    {
        //        //eğer veri bulunamadı ise geriye 404 döndürürüz.bu response a göre client tarafında işlem yapılabilir.
        //    //    return Request.CreateResponse(HttpStatusCode.NotFound, "Böyle bir kayıt bulunamadı"); //404 döner (NotFound) 

        //        return NotFound();  //yukarıdaki metotla   aynı mantıkta çalışıyor (WebApi2 ile gelen özellik)

        //    }

        //    //return Request.CreateResponse(HttpStatusCode.OK, language);  //200 döner (OK)
        //    return Ok();   //yukarıdaki metotla   aynı mantıkta çalışıyor (WebApi2 ile gelen özellik)
        //}




        ////kaydetme
        //
        //http://localhost:35207/api/Languages

        public HttpResponseMessage Post(Language language)
        {
            //model belirttiğim kurallara uygunsa
            if (ModelState.IsValid)
            {
                var createdLanguage = languageDAL.CreateLanguage(language);
                return Request.CreateResponse(HttpStatusCode.Created, createdLanguage);//201 döner
            }
            else
            {
                //cliente hata döndercez(Validation hatası) frontendci bunun bir validation hatası olduğunu bilir.
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);//400 döner
            }

        }



        //[ResponseType(typeof(Language))]  //geriye ne döndürdüğünü söyleyebiliriz.
        //public IHttpActionResult Post(Language language)
        //{
        //    //model belirttiğim kurallara uygunsa
        //    if (ModelState.IsValid)
        //    {
        //        var createdLanguage = languageDAL.CreateLanguage(language);
        //        //return Request.CreateResponse(HttpStatusCode.Created, createdLanguage);//201 döner

        //        return CreatedAtRoute("DefaultApi", new { id = createdLanguage.ID }, createdLanguage);  //yukarıdaki metotla   aynı mantıkta çalışıyor (WebApi2 ile gelen özellik)
        //    }
        //    else
        //    {
        //        //cliente hata döndercez(Validation hatası) frontendci bunun bir validation hatası olduğunu bilir.
        //        //return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);//400 döner

        //        return BadRequest(ModelState);  //yukarıdaki metotla   aynı mantıkta çalışıyor (WebApi2 ile gelen özellik)

        //    }

        //}







        ////güncelleme
        // //http://localhost:35207/api/Languages/1
        public HttpResponseMessage Put(int id, Language language)
        {
            //idye ait kayıt yoksa
            if (languageDAL.IsThereAnyLanguage(id) == false) //kayıt yoksa
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kayıt bulunamadı"); //404
            }
            else if (ModelState.IsValid == false) //   //model belirttiğim kurallara uygun değilse  //   //language modeli doğrulanmadıysa
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState); //400 
            }
            else
            {
                //kaydı güncelliyoruz 
                return Request.CreateResponse(HttpStatusCode.OK, languageDAL.UpdateLanguage(id, language)); //200 //  //OK   
            }


        }



        //[ResponseType(typeof(Language))]  //geriye ne döndürdüğünü söyleyebiliriz.
        //public IHttpActionResult Put(int id, Language language)
        //{
        //    //idye ait kayıt yoksa
        //    if (languageDAL.IsThereAnyLanguage(id) == false) //kayıt yoksa
        //    {
        //        //return Request.CreateResponse(HttpStatusCode.NotFound, "Kayıt bulunamadı"); //404

        //        return NotFound();
        //    }
        //    else if (ModelState.IsValid == false) //   //model belirttiğim kurallara uygun değilse  //   //language modeli doğrulanmadıysa
        //    {
        //        //return Request.CreateResponse(HttpStatusCode.BadRequest, ModelState); //400 

        //        return BadRequest(ModelState);
        //    }
        //    else
        //    {
        //        //kaydı güncelliyoruz 
        //        //return Request.CreateResponse(HttpStatusCode.OK, languageDAL.UpdateLanguage(id, language)); //200 //  //OK   


        //        return Ok(languageDAL.UpdateLanguage(id, language));
        //    }


        //}











        public HttpResponseMessage Delete(int id)
        {
            if (languageDAL.IsThereAnyLanguage(id) == false)//kayıt yoksa
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "Kayıt bulunamadı"); //404
            }
            else
            {
                languageDAL.DeleteLanguage(id);

                return Request.CreateResponse(HttpStatusCode.NoContent); //204 
            }

        }

        [HttpGet]
        //http://localhost:35207/api/languages?name=ramazan
        [Authorize]
        public IHttpActionResult GetSearchByName(string name)
        {
            //return Ok("Name:" + name);
            //giriş yapan kullanıcının ismini alabiliriz.
            return Ok("Name:" + User.Identity.Name);
        }


        //http://localhost:35207/api/languages?surname=ali
        public IHttpActionResult GetSearchBySurName(string surname)
        {
            return Ok("Name:" + surname);
        }



        //public IHttpActionResult Delete(int id)
        //{
        //    if (languageDAL.IsThereAnyLanguage(id) == false)//kayıt yoksa
        //    {
        //        //return Request.CreateResponse(HttpStatusCode.NotFound, "Kayıt bulunamadı"); //404

        //        return NotFound();
        //    }
        //    else
        //    {
        //        languageDAL.DeleteLanguage(id);

        //        //return Request.CreateResponse(HttpStatusCode.NoContent); //204 

        //          return StatusCode(HttpStatusCode.NoContent);

        //    }

        //}




        //================================
        //public IEnumerable<Language> Get()
        //{

        //    return languageDAL.GetAllLanguages();
        //}

        //public Language Get(int id)
        //{
        //    return languageDAL.GetLanguageById(id);
        //}

        ////kaydetme
        //public Language Post(Language language)
        //{
        //    return languageDAL.CreateLanguage(language);
        //}

        ////güncelleme
        //public Language Put(int id, Language language)
        //{
        //    return languageDAL.UpdateLanguage(id,language);
        //}

        //public void Delete(int id)
        //{
        //    languageDAL.DeleteLanguage(id);
        //}





    }
}


//referencelara DAL'ı ekle.

//webconfige DAL projesindeki connection stringi alıp yapıştırıyoruz

/*
 * 
   <connectionStrings>
    <add name="ProgrammingDbEntities" connectionString="metadata=res://*ProgrammingModel.csdl|res://*ProgrammingModel.ssdl|res://*ProgrammingModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=.;initial catalog=ProgrammingDb;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework&quot;" providerName="System.Data.EntityClient" />
  </connectionStrings>

 
 */

/*Hata ve Çözüm
 * //The 'ObjectContent`1' type failed to serialize the response body for content type 'application/xml; charset=utf-8'.
 * APi projesine EntityFramework ve EntityFramework.SQlserver dll'lerini eklemen gerekiyor.
 
 
 */
