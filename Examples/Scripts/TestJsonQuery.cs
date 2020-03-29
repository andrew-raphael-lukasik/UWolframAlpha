using UnityEngine;

namespace UWolframAlpha.Samples
{
    public class TestJsonQuery : MonoBehaviour
    {

        [SerializeField] UnityEngine.UI.InputField _input = null;
        [SerializeField] TMPro.TMP_Text _output = null;

        void Start ()
        {
            _input.onEndEdit.AddListener( Query );
        }

        async void Query ( string text )
        {
            var response = await UWolframAlpha.Query.JSON( text );
            _output.text = response;
        }
        
    }
}
