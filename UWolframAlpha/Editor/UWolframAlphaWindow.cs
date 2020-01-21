using System.Threading.Tasks;

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

using UWolframAlpha.Serialization;

namespace UWolframAlpha
{
	public class UWolframAlphaWindow : EditorWindow
	{
		const string k_editorprefskey_appid = "UWolframAlphaWindow.appid";
		const string k_editorprefskey_input = "UWolframAlphaWindow.input_value";
		Color _color1 = new Color{ r=0.6f , g=0.6f , b=0.6f , a=1 };
		TextField OUTPUT;

		public void OnEnable ()
		{
			if( OUTPUT==null )
			{

				var BAR = new VisualElement();
				{
					var style = BAR.style;
					StyleMargin( style );
					style.flexDirection = FlexDirection.Row;
					style.minHeight = 30;


					var INPUT = new TextField();
					INPUT.style.flexGrow = 1f;
					{
						INPUT.value = EditorPrefs.GetString( k_editorprefskey_input , "Answer to the Ultimate Question of Life, the Universe, and Everything" );
						INPUT.RegisterValueChangedCallback( (e) => EditorPrefs.SetString(k_editorprefskey_input,e.newValue) );
					}
					BAR.Add( INPUT );


					var BUTTON = new Button(
						async () => {
							Debug.Log($"UWolframAlpha.Query( \"{INPUT.value}\" )");
							var queryResult = await UWolframAlpha.Query( INPUT.value );
							OUTPUT.value = JsonUtility.ToJson(queryResult,true);
							OUTPUT.MarkDirtyRepaint();
						}
					);
					BUTTON.style.flexGrow = 0.5f;
					{
						BUTTON.text = "=";
					}
					BAR.Add( BUTTON );
				}
				rootVisualElement.Add( BAR );


				var SCROLLVIEW = new ScrollView();
				{
					var style = SCROLLVIEW.style;
					StyleMargin( style );
					StyleBorder( style , _color1 );
					style.flexDirection = FlexDirection.Column;
					style.flexGrow = 1f;

					OUTPUT = new TextField();
					OUTPUT.value = "<results will appear here>";
					OUTPUT.isReadOnly = true;
					SCROLLVIEW.Add( OUTPUT );
				}
				rootVisualElement.Add( SCROLLVIEW );


				var APPIP = new TextField( 100 , false , true , '#' );
				{
					var style = APPIP.style;
					StyleMargin( style );

					APPIP.value = EditorPrefs.GetString( k_editorprefskey_appid , string.Empty );
					APPIP.RegisterValueChangedCallback( (e) => EditorPrefs.SetString(k_editorprefskey_appid,e.newValue) );
				}
				rootVisualElement.Add( APPIP );

			}
		}

		[MenuItem("Tools/UWolframAlpha")]
		static void ShowWindow ()
		{
			var window = GetWindow<UWolframAlphaWindow>();
			window.titleContent = new GUIContent("UWolframAlpha");
		}

		void StyleMargin ( IStyle style , int margin = 5 ) => style.marginTop = style.marginBottom = style.marginLeft = style.marginRight = margin;
		void StyleBorder ( IStyle style , Color color , int width = 1 )
		{
			style.borderTopWidth = style.borderBottomWidth = style.borderLeftWidth = style.borderRightWidth = width;
			style.borderTopColor = style.borderBottomColor = style.borderLeftColor = style.borderRightColor = color;
		}

	}
}
