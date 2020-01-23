using UnityEngine;
using UnityEngine.UIElements;

namespace UWolframAlpha.Data
{
	
	public partial struct QueryResult : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				var INFOBAR = new Label($"success:{success} | error:{error} | timedout:{timedout} | timing:{timing}");
				INFOBAR.SetEnabled( false );
				ROOT.Add( INFOBAR );

				if( pod_array!=null )
					foreach( var next in pod_array )
						ROOT.Add( next.CreateVisualElement() );

				assumptions.CreateVisualElement();
			}
			return ROOT;
		}
	}

	public partial struct Pod : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			_Stylist.Box( ROOT );
			{
				var LABEL = new Label(title);
				_Stylist.Label( LABEL );
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				if( subpod_array!=null )
					foreach( var next in subpod_array )
						ROOT.Add( next.CreateVisualElement() );

				expressiontypes.CreateVisualElement();
				states.CreateVisualElement();
				infos.CreateVisualElement();
			}
			return ROOT;
		}
	}
	
	public partial struct SubPod : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				var LABEL = new Label(title);
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				img.CreateVisualElement();

				var PLAINTEXT = new TextField();
				PLAINTEXT.value = plaintext;
				PLAINTEXT.isReadOnly = true;
				ROOT.Add( PLAINTEXT );
			}
			return ROOT;
		}
	}

	public partial struct ExpressionTypes : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				if( expressiontype_array!=null )
					foreach( var next in expressiontype_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct ExpressionType : ICreateVisualElement
	{
		public VisualElement CreateVisualElement () => new Label(name);
	}

	public partial struct States : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				if( state_array!=null )
					foreach( var next in state_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct State : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				var LABEL = new Label(name);
				LABEL.SetEnabled( false );
				ROOT.Add( LABEL );

				ROOT.Add( new Label(input) );
			}
			return ROOT;
		}
	}

	public partial struct Infos : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				if( info_array!=null )
					foreach( var next in info_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct Info : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				ROOT.Add( new Label(text) );
				img.CreateVisualElement();
				
				if( link_array!=null )
					foreach( var next in link_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct Img : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				var SRC = new TextField();
				SRC.value = src;
				SRC.isReadOnly = true;
				ROOT.Add( SRC );
				
				var ALT = new TextField();
				ALT.value = alt;
				ALT.isReadOnly = true;
				ALT.SetEnabled( false );
				ROOT.Add( ALT );

				var l  = new Label(title);
				

				ROOT.Add( new Label(title) );
				ROOT.Add( new Label($"width:{width} | height:{height}") );
			}
			return ROOT;
		}
	}

	public partial struct Link : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
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
			{
				ROOT.Add( new Label(type) );
				ROOT.Add( new Label(word) );
				ROOT.Add( new Label(template) );

				if( value_array!=null )
					foreach( var next in value_array )
						ROOT.Add( next.CreateVisualElement() );
			}
			return ROOT;
		}
	}

	public partial struct Value : ICreateVisualElement
	{
		public VisualElement CreateVisualElement ()
		{
			var ROOT = new VisualElement();
			{
				ROOT.Add( new Label(name) );
				ROOT.Add( new Label(desc) );
				ROOT.Add( new Label(input) );
			}
			return ROOT;
		}
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
