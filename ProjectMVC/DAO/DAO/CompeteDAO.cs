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
    }
}