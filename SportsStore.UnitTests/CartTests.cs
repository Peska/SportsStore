using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

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
	}
}
