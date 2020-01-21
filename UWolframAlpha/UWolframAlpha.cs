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
			Debug.Log($"\tWebRequest: {uri}");
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
					{
						await Task.Delay( System.TimeSpan.FromMilliseconds( 100 ) );
					}

					#if DEBUG
					if( www.isNetworkError || www.isHttpError )
					{
						Debug.LogWarning( www.error );
					}
					#endif
				}
				return www;
			}

			/// <summary> Creates Texture2D from given URI </summary>
			public static async Task<Texture2D> LoadTexture ( string url )
			{
				//asserions:
				Assert.IsNotNull( url , "url is null" );
				Assert.IsTrue( url.Length!=0 , "url is of Length 0" );

				//
				using( UnityWebRequest www = UnityWebRequestTexture.GetTexture( url ) )
				{
					var asyncOp = www.SendWebRequest();
					while( asyncOp.isDone==false )
					{
						await Task.Delay( 100 );
					}

					#if UNITY_EDITOR
					if( UnityEditor.EditorApplication.isPlaying==false )
					{
						//Debug.LogWarning( "aborted" );
						www.Dispose();
						throw new System.Threading.Tasks.TaskCanceledException( "ignore, this is fine (async task ended outside play mode)" );//return null;
					}
					#endif

					if( www.isNetworkError || www.isHttpError )
					{
						#if DEBUG
						Debug.Log( $"{ www.error }, URL:{ www.url }" );
						#endif

						return null;
					}
					else if( www.isDone==false )
					{
						Debug.LogError( "www.isDone==false" );
						return null;
					}
					else
					{
						var result = DownloadHandlerTexture.GetContent( www );
						if( result!=null )
						{
							int width = result.width;
							int height = result.height;
							var format = result.format;
							var pixels = result.GetPixels();

							UnityEngine.Object.Destroy( result );

							result = new Texture2D( width , height , format , true );
							result.SetPixels( pixels );
							result.filterMode = FilterMode.Trilinear;
							result.Apply();
						}
						else Debug.LogError("texture is null while seemingly no network error");

						return result;
					}
				}
			}

			public static bool IsInternetReachable ()
			{
				var status = Application.internetReachability;
				if( status==0 )
				{
					#if DEBUG
					Debug.LogError( "internet access attempt failed" );
					#endif
					
					return false;
				}
				else
				{
					#if DEBUG
					Debug.Log( "internet access attempt succeeded" );
					#endif

					return true;
				}
			}
		}

    }
}