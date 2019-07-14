﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            return db.COMPETES.Where(table => table.USER_INFO.ID == id).OrderByDescending(table => table.ID).ToList();
        }

        public List<COMPETE> GetJoined(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Where(table => table.UserID == id && table.TimeJoined != null).Select(table=> table.COMPETE).ToList();
        }

        public List<COMPETE> GetJoinedAndPublic(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic && table.p.FirstOrDefault().TimeJoined != null || table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).ToList();
        }

        public List<COMPETE> GetScheduledCompetes(int id, int take = 5)
        {
            return db.COMPETES.Where(table => table.TimeEnd > DateTime.Now).GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).OrderBy(table => table.TimeEnd).Take(take).ToList();
        }

        public int CountJoined(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.p.FirstOrDefault(list => list.UserID == id) != null)
                .Select(table => table.t).Count();
        }

        public List<COMPETE> GetPublic(int id)
        {
            return db.COMPETES.GroupJoin(db.COMPETE_PARTICIPANTSS, t => t.ID, p => p.CompeteID, (t, p) => new { t, p })
                .Where(table => table.t.IsPublic && table.p.FirstOrDefault(list => list.UserID == id) == null)
                .Select(table => table.t).ToList();
        }

        public List<COMPETE_PARTICIPANTS> GetParticipantList(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Where(table => table.CompeteID == id).OrderByDescending(t => t.TimeJoined).ThenBy(t => t.UserID).ToList();
        }

        public Boolean CheckParticipantExist(int id, int competeId)
        {
            return db.COMPETE_PARTICIPANTSS.Count(u => u.UserID == id && u.CompeteID == competeId && u.TimeJoined != null) > 0;
        }

        public Boolean CheckParticipantInvited(int id, int competeId)
        {
            return db.COMPETE_PARTICIPANTSS.Count(u => u.UserID == id && u.CompeteID == competeId && u.TimeJoined == null) > 0;
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
                var u = db.COMPETE_PARTICIPANTSS.Find(model.CompeteID,model.UserID);
                db.COMPETE_PARTICIPANTSS.Remove(u);
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

        public bool? IsOwner(int competeID, int userID)
        {
            var item = db.COMPETES.Find(competeID);
            if (item != null)
            {
                return item.OwnerID == userID;
            }
            return null;
        }

        public List<USER_INFO> GetParticipants(int id)
        {
            return db.COMPETE_PARTICIPANTSS.Where(table => table.CompeteID == id).Select(item => item.USER_INFO).ToList();
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
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
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

        public Boolean UpdateTimeJoined(COMPETE_PARTICIPANTS c)
        {
            var u = db.COMPETE_PARTICIPANTSS.Find(c.CompeteID,c.UserID);
            try
            {
                u.TimeJoined = c.TimeJoined;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
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
                if (userID == participant.UserID && participant.TimeJoined != null)
                {
                    return true;
                }
            }

            return false;
        }
        
    }
}