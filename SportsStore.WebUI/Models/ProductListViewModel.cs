using SportsStore.Domain.Entities;
using System.Collections.Generic;

namespace SportsStore.WebUI.Models
{
	public class ProductListViewModel
	{
		public IEnumerable<Product> Products { get; set; }
		public PagingInfo PaginInfo { get; set; }
		public string CurrentCategory { get; set; }
	}
}