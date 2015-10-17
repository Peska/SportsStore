using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
	public class CartController : Controller
	{
		private IProductRepository repository;

		public CartController(IProductRepository repo)
		{
			repository = repo;
		}

		public ActionResult Index(string returnUrl)
		{
			CartIndexViewModel model = new CartIndexViewModel()
			{
				Cart = GetCart(),
				ReturnUrl = returnUrl
			};

			return View(model);
		}

		public RedirectToRouteResult AddToCart(int productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);

			if (product != null)
				GetCart().AddItem(product, 1);

			return RedirectToAction("Index", new { returnUrl });
		}

		public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);

			if (product != null)
				GetCart().RemoveLine(product);

			return RedirectToAction("Index", new { returnUrl });
		}

		private Cart GetCart()
		{
			Cart cart = (Cart)Session["Cart"];

			if (cart == null)
			{
				cart = new Cart();
				Session["Cart"] = cart;
			}

			return cart;
		}
	}
}