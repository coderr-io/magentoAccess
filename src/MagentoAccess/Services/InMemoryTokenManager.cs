﻿using System;
using System.Collections.Generic;
using DotNetOpenAuth.OAuth.ChannelElements;
using DotNetOpenAuth.OAuth.Messages;

namespace MagentoAccess.Services
{
	internal class InMemoryTokenManager : IConsumerTokenManager
	{
		public Dictionary<string, string> tokensAndSecrets = new Dictionary<string, string>();

		internal InMemoryTokenManager()
		{
		}

		public string ConsumerKey { get; internal set; }

		public string ConsumerSecret { get; internal set; }

		#region ITokenManager Members
		public string GetConsumerSecret(string consumerKey)
		{
			if (consumerKey == this.ConsumerKey)
				return this.ConsumerSecret;
			else
				throw new ArgumentException("Unrecognized consumer key.", "consumerKey");
		}

		public string GetTokenSecret(string token)
		{
			return this.tokensAndSecrets[token];
		}

		public void StoreNewRequestToken(UnauthorizedTokenRequest request, ITokenSecretContainingMessage response)
		{
			this.tokensAndSecrets[response.Token] = response.TokenSecret;
		}

		public void ExpireRequestTokenAndStoreNewAccessToken(string consumerKey, string requestToken, string accessToken, string accessTokenSecret)
		{
			this.tokensAndSecrets.Remove(requestToken);
			this.tokensAndSecrets[accessToken] = accessTokenSecret;
		}

		/// <summary>
		/// Classifies a token as a request token or an access token.
		/// </summary>
		/// <param name="token">The token to classify.</param>
		/// <returns>Request or Access token, or invalid if the token is not recognized.</returns>
		public TokenType GetTokenType(string token)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}