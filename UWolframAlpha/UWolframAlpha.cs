using System.Threading.Tasks;
using System.Xml.Serialization;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

using QueryResult = UWolframAlpha.Data.QueryResult;

namespace UWolframAlpha
{
    public static class UWolframAlpha
    {

		const string k_default_appid = "TE2UAQ-R25Q5U8VTA";

        /// <summary> WolframAlpha query. </summary>
		/// <returns> XML string </returns>
        public static async Task<string> QueryXML ( string query , string appid = k_default_appid )
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
		/// <returns> Deserialized data. </returns>
        public static async Task<QueryResult> Query ( string query , string appid = k_default_appid )
        {
			string xml = await QueryXML( query , appid );
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

			/// <summary> Creates Texture2D from given URI </summary>
			public static async Task<Texture2D> LoadTexture ( string url )
			{
				Assert.IsNotNull( url , "url is null" );
				Assert.IsTrue( url.Length!=0 , "url is of Length 0" );

				using( var www = UnityWebRequestTexture.GetTexture(url) )
				{
					var asyncOp = www.SendWebRequest();
					while( asyncOp.isDone==false )
					{
						await Task.Delay( 100 );
					}

					if( www.isNetworkError || www.isHttpError )
					{
						#if DEBUG
						Debug.LogError($"{www.error} at URL:{www.url}");
						#endif

						return null;
					}
					else
					{
						return DownloadHandlerTexture.GetContent( www );
					}
				}
			}

			public static bool IsInternetReachable () => Application.internetReachability!=0;

		}

    }
}