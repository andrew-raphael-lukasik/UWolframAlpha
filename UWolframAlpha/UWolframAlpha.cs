﻿using System.Threading.Tasks;
using System.Xml.Serialization;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

using QueryResult = UWolframAlpha.Data.QueryResult;

namespace UWolframAlpha
{
    public static class Query
    {

		const string k_default_appid = "TE2UAQ-R25Q5U8VTA";

        /// <summary> WolframAlpha query. </summary>
		/// <returns> XML string </returns>
        public static async Task<string> XML ( string query , string appid = k_default_appid )
        {
			appid = appid!=null && appid.Length!=0 ? appid : k_default_appid;
            string uri = $"http://api.wolframalpha.com/v2/query?input={query}&appid={appid}";
			
			#if DEBUG
			Debug.Log($"\tWebRequest: {uri.Replace(appid,"<appid>")}");
			#endif
            
            var webRequest = await Internal.WebRequest( uri );
			string response = webRequest.downloadHandler.text;
			
			#if DEBUG
			Debug.Log($"\t\tResponse: {response}");
			#endif

			return response;
        }

		/// <summary> WolframAlpha query. </summary>
		/// <returns> JSON string </returns>
        public static async Task<string> JSON ( string query , string appid = k_default_appid )
        {
			appid = appid!=null && appid.Length!=0 ? appid : k_default_appid;
            string uri = $"http://api.wolframalpha.com/v2/query?input={query}&appid={appid}&output=json";
			
			#if DEBUG
			Debug.Log($"\tWebRequest: {uri.Replace(appid,"<appid>")}");
			#endif
            
            var webRequest = await Internal.WebRequest( uri );
			string response = webRequest.downloadHandler.text;
			
			#if DEBUG
			Debug.Log($"\t\tResponse: {response}");
			#endif

			return response;
        }

		/// <summary> WolframAlpha query. </summary>
		/// <returns> Deserialized data. </returns>
        public static async Task<QueryResult> Data ( string query , string appid = k_default_appid )
        {
			string xml = await XML( query , appid );
			return await DeserializeXML( xml );
        }

		/// <summary> Deserializes WolframAlpha XML query. </summary>
		/// <returns> Deserialized data. </returns>
		public static async Task<QueryResult> DeserializeXML ( string xml )
        {
			return await Task.Run( ()=>{
				var serializer = new XmlSerializer(typeof(QueryResult));
				var stream = new System.IO.StringReader(xml);
				QueryResult result = (QueryResult)serializer.Deserialize( stream );
				stream.Dispose();
				return result;
			});
        }

		public static class Internal
		{
			public static async Task<UnityWebRequest> WebRequest ( string uri )
			{
				var www = UnityWebRequest.Get( uri );
				{
					var asyncOp = www.SendWebRequest();
					while( asyncOp.isDone==false )
						await Task.Delay( System.TimeSpan.FromMilliseconds(100) );

					#if DEBUG
					if( www.isNetworkError || www.isHttpError )
						Debug.LogWarning( www.error );
					#endif
				}
				return www;
			}

			/// <summary> Creates Texture2D from given URI of a JPG or PNG file </summary>
			public static async Task<Texture2D> DownloadTextureJpgPng ( string url )
			{
				Assert.IsNotNull( url , "url is null" );
				Assert.IsTrue( url.Length!=0 , "url is of Length 0" );

				using( var www = UnityWebRequestTexture.GetTexture(url) )
				{
					var asyncOp = www.SendWebRequest();
					while( asyncOp.isDone==false )
						await Task.Delay( 100 );

					if( www.isNetworkError || www.isHttpError )
					{
						#if DEBUG
						Debug.LogError($"{www.error} at URL:{www.url}");
						#endif

						return null;
					}
					else return DownloadHandlerTexture.GetContent( www );
				}
			}

			/// <summary> Creates Texture2D from given URI of a GIF file </summary>
			public static async Task<Texture2D> DownloadTextureGif ( string url )
			{
				Assert.IsNotNull( url , "url is null" );
				Assert.IsTrue( url.Length!=0 , "url is of Length 0" );

				using( var www = UnityWebRequest.Get(url) )
				{
					var asyncOp = www.SendWebRequest();
					while( asyncOp.isDone==false )
						await Task.Delay( 100 );

					if( www.isNetworkError || www.isHttpError )
					{
						#if DEBUG
						Debug.LogError($"{www.error} at URL:{www.url}");
						#endif

						return null;
					}
					else
					{
						var handle = www.downloadHandler;
						byte[] bytes = handle.data;

						//var texture = new Texture2D();

						var frames = await UniGif.GetTexturesAsync( bytes );

						return frames[0].m_texture2d;

						//var r = DownloadHandlerBuffer.GetContent( www );

						//return null;
					}
				}
			}

			public static bool IsInternetReachable () => Application.internetReachability!=0;

		}

    }
}