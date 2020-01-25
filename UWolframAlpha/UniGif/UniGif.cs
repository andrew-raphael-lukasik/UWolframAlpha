/*
UniGif
Copyright (c) 2015 WestHillApps (Hironari Nishioka)
This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System.Threading.Tasks;
using UnityEngine;

public static partial class UniGif
{
	/// <summary>
	/// Get GIF texture list Coroutine
	/// </summary>
	/// <param name="bytes">GIF file byte data</param>
	/// <param name="callback">Callback method(param is GIF texture list, Animation loop count, GIF image width (px), GIF image height (px))</param>
	/// <param name="filterMode">Textures filter mode</param>
	/// <param name="wrapMode">Textures wrap mode</param>
	/// <param name="debugLog">Debug Log Flag</param>
	/// <returns>IEnumerator</returns>
	public async static Task<GifTexture[]> GetTexturesAsync
	(
		byte[] bytes ,
		FilterMode filterMode = FilterMode.Bilinear ,
		TextureWrapMode wrapMode = TextureWrapMode.Clamp ,
		bool debugLog = false
	)
	{
		// Set GIF data
		var gifData = new GifData();
		if( !SetGifData( bytes , ref gifData , debugLog ) )
		{
			Debug.LogError( "GIF file data set error." );
			await Task.Delay( 1 );
		}

		// Decode to textures from GIF data
		var textures = await DecodeTexturesAsync( gifData , filterMode , wrapMode );

		if( textures==null || textures.Length==0 )
		{
			Debug.LogError( "GIF texture decode error." );
			await Task.Delay( 1 );
		}

		int loopCount = gifData.m_appEx.loopCount;
		int width = gifData.m_logicalScreenWidth;
		int height = gifData.m_logicalScreenHeight;

		return textures;
	}

	public async static Task<Texture2D> GetTextureAsync
	(
		byte[] bytes ,
		FilterMode filterMode = FilterMode.Bilinear ,
		TextureWrapMode wrapMode = TextureWrapMode.Clamp ,
		bool debugLog = false
	)
	{
		// Set GIF data
		var gifData = new GifData();
		if( !SetGifData( bytes , ref gifData , debugLog ) )
		{
			Debug.LogError( "GIF file data set error." );
			await Task.Delay( 1 );
		}

		// Decode to textures from GIF data
		Texture2D texture = await DecodeTextureAsync( gifData , filterMode , wrapMode );

		if( texture==null )
		{
			Debug.LogError( "GIF texture decode error." );
			await Task.Delay( 1 );
		}

		int loopCount = gifData.m_appEx.loopCount;
		int width = gifData.m_logicalScreenWidth;
		int height = gifData.m_logicalScreenHeight;

		return texture;
	}
}
