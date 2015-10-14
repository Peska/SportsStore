using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace SportsStore.WebUI.Controllers
{
	public class ProductController : Controller
    {
		private IProductRepository repository;
		public int PageSize = 4;

		public ProductController(IProductRepository repositoryParam)
		{
			repository = repositoryParam;
		}

		public ViewResult List(string category, int page = 1)
		{
			ProductListViewModel model = new ProductListViewModel
			{
				Products = repository.Products
					.Where(x => string.IsNullOrEmpty(category) || x.Category == category)
					.OrderBy(x => x.ProductID)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),
				PaginInfo = new PagingInfo
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = repository.Products.Count(x => string.IsNullOrEmpty(category) ? true : x.Category == category)
				},
				CurrentCategory = category
			};

			return View(model);
		}
    }
}