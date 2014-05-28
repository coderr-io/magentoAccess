﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagentoAccess.Models.GetOrders;
using MagentoAccess.Models.GetProducts;
using MagentoAccess.Models.PutStockItems;
using StockItem = MagentoAccess.Models.GetSrockItems.StockItem;

namespace MagentoAccess
{
	public interface IMagentoService
	{
		Task< IEnumerable< Order > > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

		Task< IEnumerable< Order > > GetOrdersAsync();

		Task UpdateProductsAsync( IEnumerable< InventoryItem > products );

		Task< IEnumerable< Product > > GetProductsSimpleAsync();

		Task< IEnumerable< StockItem > > GetProductsAsync();
	}
}