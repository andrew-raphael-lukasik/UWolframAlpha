// #define PRINT_TYPE

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace UWolframAlpha.Data
{
	
	public partial struct QueryResult : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				var INFOBAR = new Label($"success:{success} | error:{error} | timedout:{timedout} | timing:{timing}");
				INFOBAR.SetEnabled( false );
				ROOT.Add( INFOBAR );

				if( pod_array!=null )
					foreach( var next in pod_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct Pod : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			_Stylist.Box( ROOT );
			{
				var LABEL = new Label(title);
				_Stylist.Label( LABEL );
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				foreach( var next in subpod_array )
					ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}
	
	public partial struct SubPod : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			_Stylist.Box( ROOT );
			{
				var LABEL = new Label(title);
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				ROOT.Add( img.CreateVisualElement() );

				var IMAGESOURCE = _Factory.Text( "Source:" , imagesource );
				ROOT.Add( IMAGESOURCE );

				var PLAINTEXT = _Factory.TextGrid( plaintext );
				ROOT.Add( PLAINTEXT );
			}
			return ROOT;
		}
	}

	public partial struct Img : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				ROOT.Add( new WebImage(src) );
				// ROOT.Add( _Factory.Text(alt) );
				// ROOT.Add( _Factory.Text(title) );
			}
			return ROOT;
		}
	}

	public partial struct Link : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				ROOT.Add( new Label(url) );
				ROOT.Add( new Label(text) );
				ROOT.Add( new Label(title) );
			}
			return ROOT;
		}
	}

	public partial struct Assumptions : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				if( assumption_array!=null )
					foreach( var next in assumption_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct Assumption : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				//TODO: create a fitting ui representation
			}
			return ROOT;
		}
	}

	public partial struct Value : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			ROOT.Add( _Dev.TypeLabel(this) );
			{
				ROOT.Add( new Label(name) );
				ROOT.Add( new Label(desc) );
				ROOT.Add( new Label(input) );
			}
			return ROOT;
		}
	}

	static class _Factory
	{
		public static TextField Text ( string text )
		{
			if( text!=null && text.Length!=0 )
			{
				var FIELD = new TextField();
				FIELD.value = text;
				FIELD.isReadOnly = true;
				return FIELD;
			}
			else return null;
		}
		public static TextField Text ( string label , string text )
		{
			var FIELD = Text( text );
			if( FIELD!=null ) FIELD.label = label;
			return FIELD;
		}
		public static VisualElement TextGrid ( string text )
		{
			if( text!=null && text.Length!=0 )
			{
				var ROOT = new VisualElement();

				string[] rows = text.Split('\n');
				foreach( string row in rows )
				{
					var ROW = new VisualElement();
					ROW.style.flexDirection = FlexDirection.Row;
					{
						string[] cells = row.Split('|');
						foreach( string cell in cells )
						{
							VisualElement CELL;
							{
								if( cell.Length!=1 )
								{
									CELL = new TextField();
									((TextField)CELL).value = cell;
									((TextField)CELL).isReadOnly = true;
								}
								else
									CELL = new VisualElement();
							}
							{
								var style = CELL.style;
								style.flexBasis = 600f / (float)cells.Length;
								style.flexShrink = 1f;
								_Stylist.SetBorders( style , Color.grey , 1 );
								_Stylist.SetPadding( style , 4 );
								_Stylist.SetMargins( style , 0 );
							}

							ROW.Add( CELL );
						}
					}
					ROOT.Add( ROW );
				}

				return ROOT;
			}
			else return null;
		}
	}

	static class _Dev
	{
		public static Label TypeLabel ( ICreateVisualElement icve )
		#if PRINT_TYPE
			{
				var label = new Label($"type: {icve.GetType().Name}");
				label.style.color = Color.magenta;
				return label;
			}
		#else
			=> null;
		#endif
	}

	static class _Stylist
	{
		public static void Box ( VisualElement ve )
		{
			var style = ve.style;
			SetMargins( style , 5 );
			SetPadding( style , 5 );
			SetBorders( style , Color.grey  , 1 );
		}
		public static void Label ( VisualElement ve )
		{
			var style = ve.style;
			SetMargins( style , 5 );
			// SetPadding( style , 5 );
			// SetBorder( style , Color.grey  , 1 );
		}
		public static void SetMargins ( IStyle style , int margin ) => style.marginTop = style.marginBottom = style.marginLeft = style.marginRight = margin;
		public static void SetPadding ( IStyle style , int margin ) => style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = margin;
		public static void SetBorders ( IStyle style , Color color , int width )
		{
			style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = width;
			style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = color;
		}
	}

	interface ICreateVisualElement { VisualElement CreateVisualElement(); }

	public class WebImage : Image, System.IDisposable
    {
		string _url;
		Texture2D _texture;
		Image _image;
        
		public WebImage ()
			: this( string.Empty ) {}
		public WebImage ( string url )
			: base()
		{
			this._url = url;
			_image = new Image();
			_image.scaleMode = ScaleMode.ScaleToFit;
			_image.style.alignSelf = Align.FlexStart;
			this.Add( _image );
			StartDownloadingTexture();
		}

		~WebImage () => Dispose();

		public void Dispose ()
		{
			if( _texture!=null )
				Object.Destroy( _texture );
		}

		async void StartDownloadingTexture ()
		{
			while( !UWolframAlpha.Internal.IsInternetReachable() )
			{
				await Task.Delay(1000);
				if( this==null ) return;
			}
			
			_texture = await UWolframAlpha.Internal.LoadTexture( _url );
			_image.image = _texture;

			MarkDirtyRepaint();
		}
		
    }

}
