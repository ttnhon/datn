using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class UserDataDAO
    {
        DCCourseCoding db = null;
        public UserDataDAO()
        {
            db = new DCCourseCoding();
        }

        public Boolean Delete(int userId)
        {
            try
            {
                var u = db.ADD_DATAS.Find(userId);
                db.ADD_DATAS.Remove(u);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
