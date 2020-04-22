using UnityEditor;
using UnityEngine;

namespace UWolframAlpha.Editor
{
	public class UWolframAlphaWindow : EditorWindow
	{

		public void OnEnable () => new UWolframAlpha.Window.Instance().Bind(rootVisualElement);

		[MenuItem("Tools/UWolframAlpha")]
		static void ShowWindow ()
		{
			var window = GetWindow<UWolframAlphaWindow>();
			window.titleContent = new GUIContent("UWolframAlpha");
		}

	}
}
