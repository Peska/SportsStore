using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class CartTests
	{
		[TestMethod]
		public void CanAddNewLines()
		{
			Product p1 = new Product() { ProductID = 1, Name = "P1" };
			Product p2 = new Product() { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);

			Assert.AreEqual(2, cart.Lines.Count());
			Assert.AreEqual(1, cart.Lines.Single(x => x.Product.ProductID == p1.ProductID).Quantity);
			Assert.AreEqual(1, cart.Lines.Single(x => x.Product.ProductID == p2.ProductID).Quantity);
		}

		[TestMethod]
		public void CanAddQuantityForExistingLines()
		{
			Product p1 = new Product() { ProductID = 1, Name = "P1" };
			Product p2 = new Product() { ProductID = 2, Name = "P2" };

			Cart cart = new Cart();

			cart.AddItem(p1, 1);
			cart.AddItem(p2, 1);
			cart.AddItem(p1, 10);

			Assert.AreEqual(2, cart.Lines.Count());
			Assert.AreEqual(11, cart.Lines.Single(x => x.Product.ProductID == p1.ProductID).Quantity);
			Assert.AreEqual(1, cart.Lines.Single(x => x.Product.ProductID == p2.ProductID).Quantity);
		} 

		[TestMethod]
		public void CanAddToCart()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(x => x.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Category = "Apples", Name = "P1" }
			});

			Cart cart = new Cart();

			CartController target = new CartController(mock.Object, null);

			target.AddToCart(cart, 1, null);

			Assert.AreEqual(1, cart.Lines.Count());
			Assert.AreEqual(1, cart.Lines.ElementAt(0).Product.ProductID);
		}

		[TestMethod]
		public void CannotChekoutEmptyCart()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			ShippingDetails shippingDetails = new ShippingDetails();

			CartController target = new CartController(null, mock.Object);

			ViewResult result = target.Checkout(cart, shippingDetails);

			mock.Verify(x => x.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);

			Assert.AreEqual(string.Empty, result.ViewName);
			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void CannotChekoutInvalidShippingDetails()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Product(), 1);

			ShippingDetails shippingDetails = new ShippingDetails();

			CartController target = new CartController(null, mock.Object);
			target.ModelState.AddModelError("error", "error");

			ViewResult result = target.Checkout(cart, shippingDetails);

			mock.Verify(x => x.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never);

			Assert.AreEqual(string.Empty, result.ViewName);

			Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
		}

		[TestMethod]
		public void CanChekoutAndSubmitOrder()
		{
			Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

			Cart cart = new Cart();
			cart.AddItem(new Product(), 1);

			ShippingDetails shippingDetails = new ShippingDetails();

			CartController target = new CartController(null, mock.Object);

			ViewResult result = target.Checkout(cart, shippingDetails);

			mock.Verify(x => x.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once);

			Assert.AreEqual("Completed", result.ViewName);
			Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
		}
    }
}
