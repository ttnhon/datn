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

        public List<COMPETE> GetJoined(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Where(table => table.UserID == id).Select(table=> table.COMPETE).ToList();
        }

        public List<COMPETE> GetJoinedAndPublic(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic || table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).ToList();
        }

        public List<COMPETE> GetScheduledCompetes(int id, int take = 5)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic || table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).OrderBy(table => table.TimeEnd).Take(take).ToList();
        }

        public int CountJoinedAndPublic(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic || table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).Count();
        }

        public List<COMPETE> GetPublic(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic && table.p.FirstOrDefault(list => list.UserID == id) == null)
                .Select(table => table.t).ToList();
        }

        public List<int> GetParticipantList(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Where(table => table.CompeteID == id).Select(s => s.UserID).ToList();
        }

        public Boolean CheckParticipantExist(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Count(u => u.UserID == id) > 0;
        }

        public Boolean InsertParticipant(COMPETE_PARTICIPANTS model)
        {
            try
            {
                db.COMPETE_PARTICIPANTSS.Add(model);
                db.SaveChanges();
                return true;

            } catch(Exception e)
            {
                return false;
            }
        }

        public Boolean DeleteParticipant(COMPETE_PARTICIPANTS model)
        {
            try
            {
                db.COMPETE_PARTICIPANTSS.Remove(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool EnterCompete(int userID, int competeID)
        {
            try
            {
                COMPETE_PARTICIPANTS entity = new COMPETE_PARTICIPANTS()
                {
                    UserID = userID,
                    CompeteID = competeID,
                    TimeJoined = DateTime.Now
                };
                db.COMPETE_PARTICIPANTSS.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool? IsPublic(int id)
        {
            var item = db.COMPETES.Find(id);
            if(item != null)
            {
                return item.IsPublic;
            }
            return null;
        }

        public List<COMPETE> GetTen(int id)
        {
            return db.COMPETES.Where(table => table.USER_INFO.ID == id).Take(10).ToList();
        }


        public COMPETE GetOne(int id)
        {
            return db.COMPETES.Find(id);
        }

        public Boolean Insert(COMPETE c)
        {
            try
            {
                db.COMPETES.Add(c);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Boolean Update(COMPETE c)
        {
            var u = db.COMPETES.Find(c.ID);
            try
            {
                u.ID = c.ID;
                u.OwnerID = c.OwnerID;
                u.Title = c.Title;
                u.Description = c.Description;
                u.TimeEnd = c.TimeEnd;
                u.IsPublic = c.IsPublic;
                db.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public Boolean CanAccess(int competeID, int userID)
        {
            var compete = db.COMPETES.Where(table => table.ID == competeID).FirstOrDefault();

            if (compete.IsPublic)
            {
                return true;
            }

            foreach (var participant in compete.COMPETE_PARTICIPANTS)
            {
                if (userID == participant.UserID)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}