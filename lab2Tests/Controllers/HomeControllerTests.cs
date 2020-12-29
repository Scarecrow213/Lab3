using Microsoft.VisualStudio.TestTools.UnitTesting;
using lab2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using lab2.Models;
using System.Web;
using System.Web.Mvc;

namespace lab2.Controllers.Tests
{
    [TestClass()]
    public class HomeControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            var mock = new Mock<IRepository>();
            var shop = 2;
            var kind = "Процессор";
            mock.Setup(a => a.Include(shop, kind)).Returns(new List<Part>()) ;
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Index(shop, kind) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void CreateShopTest()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.CreateShop() as ViewResult;

            Assert.AreEqual("CreateShop", result.ViewName);
        }
        [TestMethod()]
        public void CreatePartTest()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>() { new Shop()});
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.CreatePart() as ViewResult;
            SelectList actual = result.ViewBag.Shops as SelectList;

            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void CreatePartEqualTest()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>() { new Shop() { ShopId = 2, NameShop = "DNS" } });
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.CreatePart() as ViewResult;
            SelectList actual = result.ViewBag.Shops as SelectList;

            // Assert
            Assert.AreEqual("NameShop", actual.DataTextField);
        }

        [TestMethod()]
        public void EditPart_partTest()
        {
            var mock = new Mock<IRepository>();
            var id = 3;
            mock.Setup(a => a.GetPart(id)).Returns(new Part());
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.EditPart(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void EditPart_shopListTest()
        {
            var mock = new Mock<IRepository>();
            var id = 2;
            mock.Setup(a => a.GetPart(id)).Returns(new Part());
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>());
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.EditPart(id) as ViewResult;
            SelectList actual = result.ViewBag.Shops as SelectList;
            // Assert
            Assert.IsNotNull(actual);
        }

        [TestMethod()]
        public void Delete_partTest()
        {
            var mock = new Mock<IRepository>();
            var id = 4;
            mock.Setup(a => a.GetPart(id)).Returns(new Part() { 
                                                        Id = id, 
                                                        Kind = "Материнская плата", 
                                                        Name = "GIGABAITE b450m", 
                                                        Price = 13000, 
                                                        ShopId = id });
            mock.Setup(a => a.GetShop(id)).Returns(new Shop());
            HomeController controller = new HomeController(mock.Object);
            // Act
            ViewResult result = controller.Delete(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }

        [TestMethod()]
        public void Delete_nameShopTest()
        {
            var mock = new Mock<IRepository>();
            var id = 4;
            string name = "MVideo";
            mock.Setup(a => a.GetPart(id)).Returns(new Part()
            {
                Id = id,
                Kind = "Материнская плата",
                Name = "GIGABAITE b450m",
                Price = 13000,
                ShopId = id
            });
            mock.Setup(a => a.GetShop(id)).Returns(new Shop()
            { 
                NameShop = "MVideo"
            });
            HomeController controller = new HomeController(mock.Object);
            // Act
            ViewResult result = controller.Delete(id) as ViewResult;
            string actual = result.ViewBag.Shop as string;

            // Assert
            Assert.AreEqual(name, actual);
        }

        [TestMethod()]
        public void Delete_shopTest()
        {
            var mock = new Mock<IRepository>();
            mock.Setup(a => a.GetShopList()).Returns(new List<Shop>() { new Shop(), new Shop()});
            HomeController controller = new HomeController(mock.Object);

            // Act
            ViewResult result = controller.Delete_shop() as ViewResult;
            SelectList actual = result.ViewBag.Shops as SelectList;

            // Assert
            Assert.AreEqual(actual.Count(), 2);
        }

        [TestMethod()]
        public void PartViewTest()
        {
            var mock = new Mock<IRepository>();
            var id = 4;
            mock.Setup(a => a.GetPart(id)).Returns(new Part()
            {
                Id = id,
                Kind = "Материнская плата",
                Name = "GIGABAITE b450m",
                Price = 13000
            });
            HomeController controller = new HomeController(mock.Object);
            // Act
            ViewResult result = controller.PartView(id) as ViewResult;

            // Assert
            Assert.IsNotNull(result.Model);
        }
    }
}