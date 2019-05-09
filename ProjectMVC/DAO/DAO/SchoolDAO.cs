using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.EF;

namespace DAO.DAO
{
    public class SchoolDAO
    {
        DCCourseCoding db = null;
        public SchoolDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// Get School list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<SCHOOL> GetList()
        {
            return db.SCHOOLS.ToList();
        }

        /// <summary>
        /// Get School by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SCHOOL GetSchoolByID(int id)
        {
            return db.SCHOOLS.Find(id);
        }

    }
}
