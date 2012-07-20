using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domas.MVC3;
using Domas.MVC3.Controllers;
using Domas.DAP.ADF.License.Module;
using System.ComponentModel.DataAnnotations;

namespace Domas.MVC3.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {         
           // Arrange
            List<ValidationResult> vErrors=new List<ValidationResult>();
            HomeController controller = new HomeController();
            var m=new Module()
            {
                Memo = "123"
            };
            ValidationContext vc = new ValidationContext(m, null, null);

            Validator.TryValidateObject(m, vc, vErrors, false);
            foreach (var e in vErrors)
            {
                throw new Exception(e.ErrorMessage);
            }
            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.AreEqual("Welcome to ASP.NET MVC!", result.ViewBag.Message);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
