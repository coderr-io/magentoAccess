﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MagentoAccess.Misc;

namespace MagentoAccess.Services
{
	internal class WebRequestServices : IWebRequestServices
	{
		#region BaseRequests
		[ Obsolete ]
		public WebRequest CreateServiceGetRequest( string serviceUrl, Dictionary< string, string > rawUrlParameters )
		{
			var parametrizedServiceUrl = serviceUrl;

			if( rawUrlParameters.Any() )
			{
				parametrizedServiceUrl += "?" + rawUrlParameters.Keys.Aggregate( string.Empty,
					( accum, item ) => accum + "&" + string.Format( "{0}={1}", item, rawUrlParameters[ item ] ) );
			}

			var serviceRequest = ( HttpWebRequest )WebRequest.Create( parametrizedServiceUrl );
			serviceRequest.Method = WebRequestMethods.Http.Get;
			//
			serviceRequest.ContentType = "text/html";
			serviceRequest.KeepAlive = true;
			serviceRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			serviceRequest.CookieContainer = new CookieContainer();
			serviceRequest.CookieContainer.Add( new Uri( "http://192.168.0.104" ), new Cookie( "PHPSESSID", "mfl1c4qsrjs647chj2ummgo886" ) );
			serviceRequest.CookieContainer.Add( new Uri( "http://192.168.0.104" ), new Cookie( "adminhtml", "mk8rlurr9c4kaecnneakg55rv7" ) );
			serviceRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
			//
			return serviceRequest;
		}

		public async Task< WebRequest > CreateServiceGetRequestAsync( string serviceUrl, string body, Dictionary< string, string > rawHeaders )
		{
			try
			{
				var encoding = new UTF8Encoding();
				var encodedBody = encoding.GetBytes( body );

				var serviceRequest = ( HttpWebRequest )WebRequest.Create( serviceUrl );
				serviceRequest.Method = WebRequestMethods.Http.Get;
				serviceRequest.ContentType = "application/json";
				serviceRequest.ContentLength = encodedBody.Length;
				serviceRequest.KeepAlive = true;

				foreach( var rawHeadersKey in rawHeaders.Keys )
				{
					serviceRequest.Headers.Add( rawHeadersKey, rawHeaders[ rawHeadersKey ] );
				}

				using( var newStream = await serviceRequest.GetRequestStreamAsync().ConfigureAwait( false ) )
					newStream.Write( encodedBody, 0, encodedBody.Length );

				return serviceRequest;
			}
			catch( Exception exc )
			{
				string headers;
				headers = rawHeaders == null ? "null" : rawHeaders.Aggregate( "", ( ac, x ) => string.Format( "[{0}-{1}]", x.Key ?? "null", x.Value ?? "null" ) );
				throw new MagentoRestException( string.Format( "Exception occured on CreateServiceGetRequestAsync(serviceUrl:{0},body:{1}, headers{2})", serviceUrl ?? "null", body ?? "null", headers ), exc );
			}
		}

		public void PopulateRequestByBody( string body, HttpWebRequest webRequest )
		{
			try
			{
				if( !string.IsNullOrWhiteSpace( body ) )
				{
					var encodedBody = new UTF8Encoding().GetBytes( body );

					webRequest.ContentLength = encodedBody.Length;
					webRequest.ContentType = "text/xml";
					var getRequestStremTask = webRequest.GetRequestStreamAsync();
					getRequestStremTask.Wait();
					using( var newStream = getRequestStremTask.Result )
						newStream.Write( encodedBody, 0, encodedBody.Length );
				}
			}
			catch( Exception exc )
			{
				var webrequestUrl = "null";

				if( webRequest != null )
				{
					if( webRequest.RequestUri != null )
					{
						if( webRequest.RequestUri.AbsoluteUri != null )
							webrequestUrl = webRequest.RequestUri.AbsoluteUri;
					}
				}

				throw new MagentoRestException( string.Format( "Exception occured on PopulateRequestByBody(body:{0}, webRequest:{1})", body ?? "null", webrequestUrl ), exc );
			}
		}
		#endregion

		#region ResponseHanding
		public Stream GetResponseStream( WebRequest webRequest )
		{
			try
			{
				using( var response = ( HttpWebResponse )webRequest.GetResponse() )
				using( var dataStream = response.GetResponseStream() )
				{
					var memoryStream = new MemoryStream();
					if( dataStream != null )
						dataStream.CopyTo( memoryStream, 0x100 );
					memoryStream.Position = 0;
					return memoryStream;
				}
			}
			catch( Exception ex )
			{
				var webrequestUrl = "null";

				if( webRequest != null )
				{
					if( webRequest.RequestUri != null )
					{
						if( webRequest.RequestUri.AbsoluteUri != null )
							webrequestUrl = webRequest.RequestUri.AbsoluteUri;
					}
				}

				throw new MagentoRestException( string.Format( "Exception occured on GetResponseStream( webRequest:{0})", webrequestUrl ), ex );
			}
		}

		public async Task< Stream > GetResponseStreamAsync( WebRequest webRequest )
		{
			try
			{
				using( var response = ( HttpWebResponse )await webRequest.GetResponseAsync().ConfigureAwait( false ) )
				using( var dataStream = await new TaskFactory< Stream >().StartNew( () => response != null ? response.GetResponseStream() : null ).ConfigureAwait( false ) )
				{
					var memoryStream = new MemoryStream();
					await dataStream.CopyToAsync( memoryStream, 0x100 ).ConfigureAwait( false );
					memoryStream.Position = 0;
					return memoryStream;
				}
			}
			catch( Exception ex )
			{
				var webrequestUrl = PredefinedValues.NotAvailable;

				if( webRequest != null )
				{
					if( webRequest.RequestUri != null )
					{
						if( webRequest.RequestUri.AbsoluteUri != null )
							webrequestUrl = webRequest.RequestUri.AbsoluteUri;
					}
				}

				throw new MagentoRestException( string.Format( "Exception occured on GetResponseStreamAsync( webRequest:{0})", webrequestUrl ), ex );
			}
		}
		#endregion
	}
}