using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming.DAL
{
    public class LanguagesDAL
    {
        ProgrammingDbEntities db = new ProgrammingDbEntities();

        public IEnumerable<Language> GetAllLanguages()
        {
            return db.Language;
        }

        public Language GetLanguageById(int id)
        {
            return db.Language.Find(id);
        }

        public Language CreateLanguage(Language language)
        {
            db.Language.Add(language);
            db.SaveChanges();
            return language;
        }

        public Language UpdateLanguage(int id,Language language)
        {
            db.Entry(language).State = EntityState.Modified;
            db.SaveChanges();
            return language;
        }


        public void DeleteLanguage(int id)
        {
            db.Language.Remove(db.Language.Find(id));
            db.SaveChanges();
          
        }

        public bool IsThereAnyLanguage(int id)
        {

            return db.Language.Any(x => x.ID == id);
        }
    }
}
