using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonProject;
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
        /// update role user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="typeRole"></param>
        /// <returns></returns>
        public Boolean UpdateRole(int id,int typeRole)
        {
            try
            {
                var u = db.USER_INFOS.Find(id);
                u.RoleUser = typeRole;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        /// <summary>
        /// check if have email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Boolean CheckEmailExist(string email)
        {
            return db.USER_INFOS.Count(u => u.Email.Equals(email)) > 0;
        }

        public USER_INFO GetUserByEmail(string email)
        {
            return db.USER_INFOS.Where(u => u.Email == email).FirstOrDefault();
        }

        public USER_INFO GetUserByUsername(string name)
        {
            return db.USER_INFOS.Where(u => u.UserName == name).FirstOrDefault();
        }

        public List<string> GetAllUserEmailExcept(int id)
        {
            return db.USER_INFOS
                .Where(u => u.ID != id)
                .Select(u => u.Email)
                .ToList();
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
        public Boolean UpdateIntro(USER_INFO entity)
        {
            try
            {
                var u = db.USER_INFOS.Find(entity.ID);
                if(u.ID > 0)
                {
                    u.FirstName = entity.FirstName;
                    u.LastName = entity.LastName;
                    u.Country = entity.Country;   
                }
                db.SaveChanges();
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public Boolean UpdateAbout(USER_INFO entity)
        {
            try
            {
                var u = db.USER_INFOS.Find(entity.ID);
                if (u.ID > 0)
                {
                    u.About = entity.About;
                    u.YearGraduation = entity.YearGraduation;
                    u.SchoolID = entity.SchoolID;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean UpdatePhoto(USER_INFO entity, string filename)
        {
            try
            {
                var u = db.USER_INFOS.Find(entity.ID);
                if (u.ID > 0)
                {
                    u.PhotoURL = filename;
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public Boolean Update(USER_INFO user)
        {
            var resultUser = db.USER_INFOS.Find(user.ID);
            try
            {
                resultUser.UserName = user.UserName;
                resultUser.LastName = user.LastName;
                resultUser.FirstName = user.FirstName;
                resultUser.Email = user.Email;
                resultUser.PasswordUser = user.PasswordUser;
                resultUser.Country = user.Country;
                resultUser.YearGraduation = user.YearGraduation;
                resultUser.RoleUser = user.RoleUser;
                resultUser.StatusUser = user.StatusUser;
                resultUser.About = user.About;
                resultUser.PhotoURL = user.PhotoURL;
                resultUser.FacebookLink = user.FacebookLink;
                resultUser.GoogleLink = user.GoogleLink;
                db.SaveChanges();
                return true;
            }
            catch(Exception ex)
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
        public USER_INFO GetByEmail(string email)
        {
            return db.USER_INFOS.SingleOrDefault(x => x.Email == email);
        }
        public USER_INFO GetByName(string name)
        {
            return db.USER_INFOS.SingleOrDefault(x => x.UserName == name);
        }
       

        /// <summary>
        /// get Name by ID
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public string GetNameByID(int id)
        {
            USER_INFO user_info = db.USER_INFOS.SingleOrDefault(x => x.ID == id);
            return user_info.FirstName + " " + user_info.LastName;
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

        public IEnumerable<USER_INFO> ListAllPagingRequestAdmin(string searchString, int? page, int pageSize)
        {
            IQueryable<USER_INFO> model = db.USER_INFOS;
            List<string> requestIds = db.ADD_DATAS.Where(table => table.Title == CommonConstant.REQUEST_MODERATOR)
                .Select(table => table.Data).ToList();
            model= db.USER_INFOS.Where(table => requestIds.Contains((table.ID).ToString()));
            //model = (from u in db.USER_INFOS
            //         from ud in db.ADD_DATAS
            //         //join ud in db.ADD_DATAS on new { ID = u.ID.ToString() } equals new { ID = ud.Data.ToString() }
            //         where ud.Title == CommonConstant.REQUEST_MODERATOR && ud.Data.Contains(u.ID.ToString())
            //         select u
            //    );

            if (String.IsNullOrEmpty(searchString) == false)
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Email.Contains(searchString));

            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page ?? 1, pageSize);
        }
    }
}
