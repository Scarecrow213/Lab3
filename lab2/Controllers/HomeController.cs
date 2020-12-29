using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using lab2.Models;

namespace lab2.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;
        public HomeController(IRepository r)
        {
            repo = r;
        }
        public HomeController()
        {
            repo = new PartRepository();
        }
        public ActionResult Index(int? shop, string kind)
        {
            var shops = repo.GetShopList();
            // устанавливаем начальный элемент, который позволит выбрать всех 
            shops.Insert(0, new Shop { NameShop = "Все", ShopId = 0 });
            PartListViewModel plvm = new PartListViewModel
            {
                Parts = repo.Include(shop, kind),
                Shops = new SelectList(shops, "ShopId", "NameShop"),
                Kind = new SelectList(new List<string>()
                {
                "Все",
                "Видеокарта",
                "Процессор",
                "Оперативная память",
                "Материнская плата"
                })
            };
            return View(plvm);
        }
        //добавление магазина 
        [HttpGet]
        public ActionResult CreateShop()
        {
            return View("CreateShop");
        }
        [HttpPost]
        public ActionResult CreateShop(Shop shop)
        {
            repo.Create_shop(shop);
            repo.Save();

            return RedirectToAction("Index");
        }
        //Создание новой комплектующей 
        [HttpGet]
        public ActionResult CreatePart()
        {
            // Формируем список магазинов для передачи в представление 
            SelectList shops = new SelectList(repo.GetShopList(), "ShopId", "NameShop");
            SelectList Kind = new SelectList(new List<string>()
            {
                "Видеокарта",
                "Процессор",
                "Оперативная память",
                "Материнская плата"
            });
            ViewBag.Kinds = Kind;
            ViewBag.Shops = shops;
            return View();
        }

        [HttpPost]
        public ActionResult CreatePart(Part part)
        {
            //Добавляем комплектующую в таблицу
            repo.Create(part);
            repo.Save();
            // перенаправляем на главную страницу 
            return RedirectToAction("Index");
        }

        //Редактирование записи 
        [HttpGet]
        public ActionResult EditPart(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            // Находим в бд комплектующую 
            Part part = repo.GetPart(id);
            if (part != null)
            {
                // Создаем список магазинов для передачи в представление 
                SelectList shops = new SelectList(repo.GetShopList(), "ShopId", "NameShop", part.ShopId);
                SelectList Kind = new SelectList(new List<string>()
                {
                    "Видеокарта",
                    "Процессор",
                    "Оперативная память",
                    "Материнская плата"
                });
                ViewBag.Shops = shops;
                ViewBag.Kinds = Kind;
                return View(part);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult EditPart(Part part)
        {
            repo.Update(part);
            repo.Save();
            return RedirectToAction("Index");
        }

        //удаление комплектующей 
        [HttpGet]
        public ActionResult Delete(int id)
        {
            Part b = repo.GetPart(id); 
            if (b == null)
            {
                return HttpNotFound();
            }
            Shop shop = repo.GetShop(b.ShopId);
            ViewBag.Shop = shop.NameShop;
            return View(b);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.Delete(id);
            repo.Save();
            return RedirectToAction("Index");
        }

        //Удаление магазина
        [HttpGet]
        public ActionResult Delete_shop()
        {
            // Формируем список магазинов для передачи в представление
            SelectList shops = new SelectList(repo.GetShopList(), "ShopId", "NameShop");
            ViewBag.Shops = shops;
            return View();
        }

        [HttpPost, ActionName("Delete_shop")]
        public ActionResult DeleteConfirmed_shop(Shop shop)
        {
            repo.Delete_shop(shop.ShopId);
            repo.Save();
            return RedirectToAction("Index");
        }

        public ActionResult PartView(int id)
        {
            Part b = repo.GetPart(id);
            return View(b);
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}