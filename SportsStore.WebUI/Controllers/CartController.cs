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

		public ActionResult Index(Cart cart, string returnUrl)
		{
			CartIndexViewModel model = new CartIndexViewModel()
			{
				Cart = cart,
				ReturnUrl = returnUrl
			};

			return View(model);
		}

		public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);

			if (product != null)
				cart.AddItem(product, 1);

			return RedirectToAction("Index", new { returnUrl });
		}

		public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
		{
			Product product = repository.Products.FirstOrDefault(x => x.ProductID == productId);

			if (product != null)
				cart.RemoveLine(product);

			return RedirectToAction("Index", new { returnUrl });
		}

		public PartialViewResult Summary(Cart cart)
		{
			return PartialView(cart);
		}
	}
}