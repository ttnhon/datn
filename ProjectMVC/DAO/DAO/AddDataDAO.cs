using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class AddDataDAO
    {
        DCCourseCoding db = null;
        public AddDataDAO()
        {
            db = new DCCourseCoding();
        }

        public int AddRequestTeacher(ADD_DATA entity)
        {
            try
            {
                ADD_DATA isExist = db.ADD_DATAS.Where(table => table.Title == entity.Title && table.Data == entity.Data.ToString()).FirstOrDefault();
                if (isExist != null)
                {
                    return -1;
                }
                db.ADD_DATAS.Add(entity);
                db.SaveChanges();
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}
