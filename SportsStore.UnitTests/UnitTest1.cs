using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void CanPaginate()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(x => x.Products).Returns(new Product[] 
			{
				new Product { ProductID = 1, Name = "P1" },
				new Product { ProductID = 2, Name = "P2" },
				new Product { ProductID = 3, Name = "P3" },
				new Product { ProductID = 4, Name = "P4" },
				new Product { ProductID = 5, Name = "P5" },
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			ProductListViewModel result = controller.List(2).Model as ProductListViewModel;
			Product[] prodArray = result.Products.ToArray();

			Assert.AreEqual(2, prodArray.Length);
			Assert.AreEqual(prodArray[0].ProductID, 4);
			Assert.AreEqual(prodArray[1].ProductID, 5);
		}

		[TestMethod]
		public void CanGeneratePageLinks()
		{
			HtmlHelper myHelper = null;

			PagingInfo pagingInfo = new PagingInfo
			{
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};

			Func<int, string> pageUrlDelegate = x => string.Format("Page {0}", x);

			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

			string expected =
				@"<a class=""btn btn-default"" href=""Page 1"">1</a>" +
				@"<a class=""btn btn-default btn-primary selected"" href=""Page 2"">2</a>" +
				@"<a class=""btn btn-default"" href=""Page 3"">3</a>";

			Assert.AreEqual(expected, result.ToHtmlString());
		}

		[TestMethod]
		public void CanSendPaginationViewModel()
		{
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(x => x.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1" },
				new Product { ProductID = 2, Name = "P2" },
				new Product { ProductID = 3, Name = "P3" },
				new Product { ProductID = 4, Name = "P4" },
				new Product { ProductID = 5, Name = "P5" },
			});

			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			ProductListViewModel result = controller.List(2).Model as ProductListViewModel;

			PagingInfo pageInfo = result.PaginInfo;

			Assert.AreEqual(2, pageInfo.CurrentPage);
			Assert.AreEqual(3, pageInfo.ItemsPerPage);
			Assert.AreEqual(5, pageInfo.TotalItems);
			Assert.AreEqual(2, pageInfo.TotalPages);
		}
	}
}
