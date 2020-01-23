#define PRINT_TYPE

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

				ROOT.Add( subpod.CreateVisualElement() );
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
			{
				var LABEL = new Label(title);
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				ROOT.Add( img.CreateVisualElement() );

				var PLAINTEXT = _Factory.TextField( plaintext );
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
				// TODO: display image from src field

				ROOT.Add( _Factory.TextField(src) );
				// ROOT.Add( _Factory.TextField(alt) );
				// ROOT.Add( _Factory.TextField(title) );
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
		public static TextField TextField ( string text )
		{
			if( text?.Length==0 ) return null;
			var textField = new TextField();
			textField.value = text;
			textField.isReadOnly = true;
			return textField;
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
			SetMargin( style , 5 );
			SetPadding( style , 5 );
			SetBorder( style , Color.grey  , 1 );
		}
		public static void Label ( VisualElement ve )
		{
			var style = ve.style;
			SetMargin( style , 5 );
			// SetPadding( style , 5 );
			// SetBorder( style , Color.grey  , 1 );
		}
		public static void SetMargin ( IStyle style , int margin ) => style.marginTop = style.marginBottom = style.marginLeft = style.marginRight = margin;
		public static void SetPadding ( IStyle style , int margin ) => style.paddingTop = style.paddingBottom = style.paddingLeft = style.paddingRight = margin;
		public static void SetBorder ( IStyle style , Color color , int width )
		{
			style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = width;
			style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = color;
		}
	}

	interface ICreateVisualElement { VisualElement CreateVisualElement(); }

}
