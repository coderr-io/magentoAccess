﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MagentoAccess.Models.Services.Rest.GetOrders;
using MagentoAccess.Models.Services.Rest.GetProduct;
using MagentoAccess.Models.Services.Rest.GetProducts;
using MagentoAccess.Models.Services.Rest.GetStockItems;
using MagentoAccess.Models.Services.Rest.PutStockItems;
using StockItem = MagentoAccess.Models.Services.Rest.PutStockItems.StockItem;

namespace MagentoAccess.Services.Rest
{
	internal interface IMagentoServiceLowLevelRest
	{
		string AccessToken { get; }

		string AccessTokenSecret { get; }

		string ConsumerKey { get; }

		string ConsumerSecretKey { get; }

		string RequestToken { get; }

		TransmitVerificationCodeDelegate TransmitVerificationCode { get; set; }
		string BaseMagentoUrl { get; }

		Task InitiateDescktopAuthenticationProcess();

		Task< GetProductResponse > GetProductAsync( string id );

		Task< GetProductsResponse > GetProductsAsync( int page, int limit, bool thrownExc = false );

		Task< GetStockItemsResponse > GetStockItemsAsync( int page, int limit );

		Task< PutStockItemsResponse > PutStockItemsAsync( IEnumerable< StockItem > inventoryItems, string mark );

		Task< GetOrdersResponse > GetOrdersAsync();

		Task< GetOrdersResponse > GetOrdersAsync( DateTime dateFrom, DateTime dateTo );

		VerificationData RequestVerificationUri();

		void PopulateAccessTokenAndAccessTokenSecret( string verificationCode, string requestToken, string requestTokenSecret );

		string ToJsonRestInfo();

		Task< GetOrdersResponse > GetOrderAsync( string incrementId );
	}
}