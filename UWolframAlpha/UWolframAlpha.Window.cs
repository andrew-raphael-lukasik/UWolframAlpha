using UnityEngine;
using UnityEngine.UIElements;

namespace UWolframAlpha.Window
{
	public class Instance
	{
		const string k_playerprefskey_appid = "UWolframAlphaWindow.appid";
		const string k_playerprefskey_input = "UWolframAlphaWindow.input_value";
		Color _color1 = new Color{ r=0.6f , g=0.6f , b=0.6f , a=1 };

		VisualElement OUTPUT;
		TextField INPUT;

		public void Bind ( VisualElement rootVisualElement )
		{
			var ROOT = new VisualElement();
			{
				var style = ROOT.style;
				style.backgroundColor = Color.white;
				style.flexGrow = 1f;
			}
			rootVisualElement.Add( ROOT );


			var BAR = new VisualElement();
			{
				var style = BAR.style;
				StyleMargin( style );
				style.flexDirection = FlexDirection.Row;
				style.minHeight = 30;
			}
			{
				INPUT = new TextField();
				{
					INPUT.style.flexGrow = 1f;
				}
				{
					INPUT.isDelayed = true;
					INPUT.SetValueWithoutNotify( PlayerPrefs.GetString( k_playerprefskey_input , "gold vs carbon vs iridium" ) );
					INPUT.RegisterValueChangedCallback( (e) => {
						PlayerPrefs.SetString( k_playerprefskey_input , e.newValue );
						OnButtonDown();
					});
				}
				BAR.Add( INPUT );


				var BUTTON = new Button( OnButtonDown );
				{
					BUTTON.style.flexGrow = 0.5f;
				}
				{
					BUTTON.text = "=";
				}
				BAR.Add( BUTTON );
			}
			ROOT.Add( BAR );


			var SCROLLVIEW = new ScrollView();
			{
				var style = SCROLLVIEW.style;
				StyleMargin( style );
				StyleBorder( style , _color1 );
				style.flexGrow = 1f;
			}
			{
				OUTPUT = new VisualElement();
				SCROLLVIEW.Add( OUTPUT );
			}
			ROOT.Add( SCROLLVIEW );


			var APPID_BAR = new VisualElement();
			{
				var style = APPID_BAR.style;
				StyleMargin( APPID_BAR.style );
				style.minHeight = 20;
				style.flexDirection = FlexDirection.Row;
			}
			{
				var APPID_FIELD = new TextField( 100 , false , true , '*' );
				{
					var style = APPID_FIELD.style;
					style.flexGrow = 0.5f;
				}
				{
					var LABEL = new Label("appid:");
					APPID_BAR.Add( LABEL );

					APPID_FIELD.SetValueWithoutNotify( PlayerPrefs.GetString( k_playerprefskey_appid , string.Empty ) );
					APPID_FIELD.RegisterValueChangedCallback( (e) => PlayerPrefs.SetString( k_playerprefskey_appid , e.newValue ) );
					APPID_FIELD.isDelayed = true;

					LABEL.SetEnabled( false );
				}
				APPID_BAR.Add( APPID_FIELD );

				var EMPTY = new VisualElement();
				{
					var style = EMPTY.style;
					style.flexGrow = 1;
				}
				APPID_BAR.Add( EMPTY );
			}
			ROOT.Add( APPID_BAR );
		}

		async void OnButtonDown ()
		{
			Debug.Log($"UWolframAlpha.Query( \"{INPUT.value}\" )");
			OUTPUT.SetEnabled( false );

			string appid = PlayerPrefs.GetString( k_playerprefskey_appid , string.Empty );
			var queryResult =
				appid.Length!=0
				? await Query.Data( INPUT.value , appid )
				: await Query.Data( INPUT.value );
			
			OUTPUT.Clear();
			OUTPUT.SetEnabled( true );
			OUTPUT.Add( queryResult.CreateVisualElement() );
			OUTPUT.MarkDirtyRepaint();
		}

		void StyleMargin ( IStyle style , int margin = 5 ) => style.marginTop = style.marginBottom = style.marginLeft = style.marginRight = margin;
		void StyleBorder ( IStyle style , Color color , int width = 1 )
		{
			style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = width;
			style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = color;
		}

	}
}
