﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MagentoAccess.Magento2catalogProductRepositoryV1_v_2_0_2_0_CE;
using MagentoAccess.MagentoSoapServiceReference;

namespace MagentoAccess.Models.Services.Soap.GetProducts
{
	internal class SoapGetProductsResponse
	{
		public SoapGetProductsResponse( catalogProductListResponse res )
		{
			this.Products = res.result.Select( x => new SoapProduct( x ) );
		}

		public SoapGetProductsResponse( MagentoSoapServiceReference_v_1_14_1_EE.catalogProductListResponse res )
		{
			this.Products = res.result.Select( x => new SoapProduct( x ) );
		}

		public SoapGetProductsResponse( List< CatalogDataProductInterface > res )
		{
			this.Products = res.Select( x => new SoapProduct( x ) );
		}

		public IEnumerable< SoapProduct > Products{ get; set; }
	}

	internal class SoapProduct
	{
		public SoapProduct( catalogProductEntity catalogProductEntity )
		{
			CategoryIds = catalogProductEntity.category_ids.ToList();
			Name = catalogProductEntity.name;
			ProductId = catalogProductEntity.product_id;
			Set = catalogProductEntity.set;
			Sku = catalogProductEntity.sku;
			this.Type = catalogProductEntity.type;
			WebsiteIds = catalogProductEntity.website_ids.ToList();
		}

		public SoapProduct( MagentoSoapServiceReference_v_1_14_1_EE.catalogProductEntity catalogProductEntity )
		{
			CategoryIds = catalogProductEntity.category_ids.ToList();
			Name = catalogProductEntity.name;
			ProductId = catalogProductEntity.product_id;
			Set = catalogProductEntity.set;
			Sku = catalogProductEntity.sku;
			this.Type = catalogProductEntity.type;
			WebsiteIds = catalogProductEntity.website_ids.ToList();
		}

		public SoapProduct( CatalogDataProductInterface catalogProductEntity )
		{
			Name = catalogProductEntity.name;
			ProductId = catalogProductEntity.id.ToString( CultureInfo.InvariantCulture );
			Sku = catalogProductEntity.sku;
			this.Type = catalogProductEntity.typeId;
		}

		public string Type{ get; set; }

		public List< string > WebsiteIds{ get; set; }

		public string Sku{ get; set; }

		public string Set{ get; set; }

		public string ProductId{ get; set; }

		public string Name{ get; set; }

		public List< string > CategoryIds{ get; set; }
	}
}