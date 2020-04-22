using System.Collections.Generic;
using UnityEngine;
using Unity.UIElements.Runtime;

namespace UWolframAlpha.Samples
{
	[RequireComponent(typeof(PanelRenderer))]
	public class TestRuntimeWindow : MonoBehaviour
	{

		PanelRenderer _panelRenderer = null;

		void Start ()
		{
			// magical pixie dust that makes ui appear (no idea why tho, might be a bug in early UIElements version)
			_panelRenderer.enabled = false;
			_panelRenderer.enabled = true;
		}
		
		void OnEnable ()
		{
			_panelRenderer = GetComponent<PanelRenderer>();
			_panelRenderer.postUxmlReload += BindPanelRenderer;
		}

		void OnDisable ()
		{
			_panelRenderer.postUxmlReload -= BindPanelRenderer;
		}

		IEnumerable<UnityEngine.Object> BindPanelRenderer ()
		{
			new UWolframAlpha.Window.Instance().Bind( _panelRenderer.visualTree );
			return null;
		}

	}
}
