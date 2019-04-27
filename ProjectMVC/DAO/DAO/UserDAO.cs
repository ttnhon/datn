using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonProject.CommonConstant;
using DAO.EF;
using PagedList;

namespace DAO.DAO
{
    public class UserDAO
    {
        DCCourseCoding db = null;
        public UserDAO()
        {
            db = new DCCourseCoding();
        }

        /// <summary>
        /// check if have user name exist
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Boolean CheckUserNameExist(string userName)
        {
            return db.USER_INFOS.Count(u => u.UserName == userName) > 0;
        }

        /// <summary>
        /// check if have email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Boolean CheckEmailExist(string email)
        {
            return db.USER_INFOS.Count(u => u.Email == email) > 0;
        }

        /// <summary>
        /// insert information of user in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean Insert(USER_INFO entity)
        {
            try
            {
                db.USER_INFOS.Add(entity);
                db.SaveChanges();
                return true;
            }      
            catch(Exception e)
            {
                return false;
            }
        }
       
        /// <summary>
        /// update information for user in database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Boolean Update(USER_INFO entity)
        {
            try
            {
                var u = db.USER_INFOS.Find(entity.ID);
                if (u != null)
                {
                    u.UserName = entity.UserName;
                    u.FirstName = entity.FirstName;
                    u.LastName = entity.LastName;
                    u.PasswordUser = entity.PasswordUser;
                    u.PhotoURL = entity.PhotoURL;
                    u.RoleUser = entity.RoleUser;
                    u.StatusUser = entity.RoleUser;                
                }
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public int UpdateStatus(int id)
        {
            var user = db.USER_INFOS.Find(id);
            if (user.StatusUser.Equals(1))
            {
                user.StatusUser = 0;
                db.SaveChanges();
                return 0;
            }
            else
            {
                user.StatusUser = 1;
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// delete user in database by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean Delete(int userId)
        {
            try
            {
                var u = db.USER_INFOS.Find(userId);
                db.USER_INFOS.Remove(u);
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// get information of user 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public USER_INFO GetUserById(int userId)
        {
            return db.USER_INFOS.Find(userId);
        }

        /// <summary>
        /// login with user name and pass
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public int Login(String userName, string pass)
        {
            var res = db.USER_INFOS.SingleOrDefault(u => u.UserName == userName);
            if (res == null) return CommonConstant.STATUS_NOT_EXIST_ACCOUNT;
            else if (res.StatusUser.Equals(CommonConstant.STATUS_LOCK_ACCOUNT)) return CommonConstant.STATUS_LOCK_ACCOUNT;// dang bi khoa
            else
            {
                if (res.PasswordUser.Equals(pass)) return CommonConstant.STATUS_RIGHT_ACCOUNT;
                else return CommonConstant.STATUS_WRONG_PASS_ACCOUNT;
            }
        }

        /// <summary>
        /// get information of user by username
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public USER_INFO GetByName(string Name)
        {
            return db.USER_INFOS.SingleOrDefault(x => x.UserName == Name);
        }

        //paging data
        public IEnumerable<USER_INFO> ListAllPaging(string searchString, int? page, int pageSize)
        {
            IQueryable<USER_INFO> model = db.USER_INFOS;
            if (String.IsNullOrEmpty(searchString) == false)
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Email.Contains(searchString));

            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page ?? 1, pageSize);
        }

    }
}
