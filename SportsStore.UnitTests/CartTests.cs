using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;

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

			CartController target = new CartController(mock.Object);

			target.AddToCart(cart, 1, null);

			Assert.AreEqual(1, cart.Lines.Count());
			Assert.AreEqual(1, cart.Lines.ElementAt(0).Product.ProductID);
		}
	}
}
