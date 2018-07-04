using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Programming.DAL
{
    public class UserDAL:BaseDAL
    {
        public Users GetUserByApiKey(string apiKey)
        {

            return db.Users.FirstOrDefault(x=>x.UserKey.ToString()==apiKey);
        }

        public Users GetUserByName(string userName)
        {

            return db.Users.FirstOrDefault(x=>x.Name==userName);
        }

    }
}
