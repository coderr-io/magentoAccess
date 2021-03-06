﻿using System.Linq;
using System.Xml.Linq;
using MagentoAccess.Models.Services.Rest.GetStockItems;

namespace MagentoAccess.Services.Parsers
{
	internal class MegentoInventoryResponseParser : MagentoBaseResponseParser< GetStockItemsResponse >
	{
		protected override GetStockItemsResponse ParseWithoutExceptionHanding( XElement root )
		{
			XNamespace ns = "";

			var dataItemsNodes = root.Nodes();

			var inventoryItems = dataItemsNodes.Select( x => XElement.Parse( x.ToString() ) ).ToList();

			var orders = inventoryItems.Select( x =>
			{
				var resultOrder = new StockItem
				{
					ItemId = GetElementValue( x, ns, "item_id" ),
					ProductId = GetElementValue( x, ns, "product_id" ),
					Qty = GetElementValue( x, ns, "qty" ),
					BackOrders = GetElementValue( x, ns, "backorders" )
				};

				return resultOrder;
			} ).ToList();

			return new GetStockItemsResponse { Items = orders };
		}
	}
}