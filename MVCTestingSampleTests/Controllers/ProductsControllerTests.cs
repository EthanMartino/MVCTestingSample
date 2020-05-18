using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MVCTestingSample.Controllers;
using MVCTestingSample.Models;
using MVCTestingSample.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MVCTestingSample.Controllers.Tests
{
    [TestClass()]
    public class ProductsControllerTests
    {
        [TestMethod]
        public async Task Index_ReturnsAViewResult_WithAListOfAllProducts()
        {
            //Arrange
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.GetAllProductsAsync())
                .ReturnsAsync(GetProducts());

            ProductsController prodController = new ProductsController(mockRepo.Object);

            //Act
            IActionResult result = await prodController.Index();

            //Assert
            //Ensures View is returned
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            //Ensures List<Product> passed to the View
            ViewResult viewResult = result as ViewResult;
            var model = viewResult.ViewData.Model;
            Assert.IsInstanceOfType(model, typeof(List<Product>));

            //Ensures all Products are passed into the View
            List<Product> productModel = model as List<Product>;
            Assert.AreEqual(3, productModel.Count);
        }

        private List<Product> GetProducts()
        {
            List<Product> prods = new List<Product>()
            {
                new Product()
                {
                    ProductId = 1, Name = "Computer", Price = "99.99"
                },
                new Product()
                {
                    ProductId = 2, Name = "WebCam", Price = "49.99"
                },
                new Product()
                {
                    ProductId = 3, Name = "Desk", Price = "199.99"
                }
            };
            return prods;
        }

        [TestMethod]
        public void Add_ReturnsAViewResult()
        {
            Mock<IProductRepository> mockRepo = new Mock<IProductRepository>();
            ProductsController controller = new ProductsController(mockRepo.Object);

            var result = controller.Add();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task AddPost_ResturnsARedirectAndAddsProduct_WhenModelStateIsValid()
        {
            var mockRepo = new Mock<IProductRepository>();
            mockRepo.Setup(repo => repo.AddProductAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask)
                .Verifiable();

            var controller = new ProductsController(mockRepo.Object);
            Product p = new Product()
            {
                Name = "Widget",
                Price = "9.99"
            };
            var result = await controller.Add(p);

            //Ensures user is redirected after successfully adding a product
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult), "Return value should be a RedirectToAction");

            //Ensure Controller name is not specified in the RedirectToAction
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNull(redirectResult.ControllerName, "Controller Name should not be specified in the redirect");

            //Ensures the Redirect is to the Index Action
            Assert.AreEqual("Index", redirectResult.ActionName, "User should be redirected to Index");
        }

        [TestMethod]
        public async Task AddPost_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            var mockRepo = new Mock<IProductRepository>();
            var controller = new ProductsController(mockRepo.Object);
            var invalidProduct = new Product()
            {
                Name = null, //Name is required for MOdel State to be valid
                Price = "9.99",
                ProductId = 1
            };

            //Marks ModelState as Invalid
            controller.ModelState.AddModelError("Name", "Required");

            IActionResult result = await controller.Add(invalidProduct);

            //Ensuring a View is returned
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            ViewResult viewResult = result as ViewResult;

            //Ensures returned View is model bound to a Product object
            Assert.IsInstanceOfType(viewResult.Model, typeof(Product));

            //Ensures invalid product is what is getting passed into the view
            Product modelBoundProduct = viewResult.Model as Product;
            Assert.AreEqual(modelBoundProduct, invalidProduct, "The invalid product should be passed into the View");
        }
    }
}