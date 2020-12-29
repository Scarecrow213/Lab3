using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace lab2.Models
{
    public interface IRepository : IDisposable
    {
        List<Part> GetPartList();
        Part GetPart(int? id);
        List<Shop> GetShopList();
        Shop GetShop(int? id);
        List<Part> Include(int? shop, string kind);
        void Create(Part item);
        void Create_shop(Shop item);
        void Update(Part item);
        void Delete(int id);
        void Delete_shop(int id);
        void Save();
    }
    public class PartRepository : IRepository
    {
        private PartContext db;
        public PartRepository()
        {
            this.db = new PartContext();
        }
        public List<Part> GetPartList()
        {
            return db.Parts.ToList();
        }
        public List<Shop> GetShopList()
        {
            return db.Shops.ToList();
        }
        public List<Part> Include(int? shop, string kind)
        {
            IQueryable<Part> parts = db.Parts.Include(p => p.Shop);
            if (shop != null && shop != 0)
            {
                parts = parts.Where(p => p.ShopId == shop);
            }
            if (!String.IsNullOrEmpty(kind) && !kind.Equals("Все"))
            {
                parts = parts.Where(p => p.Kind == kind);
            }
            return parts.ToList();
        }
        public Part GetPart(int? id)
        {
            return db.Parts.Find(id);
        }
        public Shop GetShop(int? id)
        {
            return db.Shops.Find(id);
        }

        public void Create(Part p)
        {
            db.Parts.Add(p);
        }
        public void Create_shop(Shop s)
        {
            db.Shops.Add(s);
        }
        public void Update(Part p)
        {
            db.Entry(p).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Part p = db.Parts.Find(id);
            if (p != null)
                db.Parts.Remove(p);
        }
        public void Delete_shop(int id)
        {
            Shop s = db.Shops.Find(id);
            if (s != null)
                db.Shops.Remove(s);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}