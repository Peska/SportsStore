using SportsStore.Domain.Abstract;
using System.Collections.Generic;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
	public class EFProductRepository : IProductRepository
	{
		private EFDbContext context = new EFDbContext();

		public IEnumerable<Product> Products
		{
			get
			{
				return context.Products;
			}
		}
	}
}
