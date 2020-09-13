using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace UWolframAlpha.Runtime
{
	[RequireComponent( typeof(UIDocument) )]
	public class UWolframAlphaComponent : MonoBehaviour
	{
		void Start ()
		{
			VisualElement root = GetComponent<UIDocument>().rootVisualElement;
			new UWolframAlpha.Window.Instance().Bind( root );
		}
	}
}
