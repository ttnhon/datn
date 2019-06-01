using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAO.EF;

namespace DAO.DAO
{
    public class CompeteDAO
    {
        DCCourseCoding db = null;
        public CompeteDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get language list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<COMPETE> GetAll()
        {
            return db.COMPETES.ToList();
        }

        public List<COMPETE> GetAll(int id)
        {
            return db.COMPETES.Where(table => table.USER_INFO.ID == id).ToList();
        }

        public List<COMPETE> GetTen(int id)
        {
            return db.COMPETES.Where(table => table.USER_INFO.ID == id).Take(10).ToList();
        }


        public COMPETE GetOne(int id)
        {
            return db.COMPETES.Find(id);
        }
    }
}