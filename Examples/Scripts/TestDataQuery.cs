using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UWolframAlpha.Samples
{
    public class TestDataQuery : MonoBehaviour
    {

        [SerializeField] InputField _input = null;
        [SerializeField] RectTransform _outputContainer = null;

        Queue<Texture> _imagesToDispose = new Queue<Texture>(10);

        void Start ()
        {
            _input.onEndEdit.AddListener( Query );
        }

        void OnDestroy () => Dispose();

        async void Query ( string text )
        {
            // clear output view:
            for( int i=_outputContainer.childCount-1 ; i!=-1 ; i-- )
                Object.Destroy( _outputContainer.GetChild(i).gameObject );
            Dispose();

            // query:
            var response = await UWolframAlpha.Query.Data( text );

            // append output view:
            foreach( var pod in response.pod_array )
            {
                var entry = new GameObject( $"pod {pod.title}" , typeof(RectTransform) , typeof(VerticalLayoutGroup) , typeof(Image) );
                entry.transform.SetParent( _outputContainer.transform , false );
                {
                    var layout = entry.GetComponent<VerticalLayoutGroup>();
                    layout.childForceExpandHeight = false;
                    layout.childControlWidth = true;
                    layout.childControlHeight = false;
                }
                {
                    var bgimage = entry.GetComponent<Image>();
                    bgimage.color = Color.gray;
                }

                // label:
                {
                    var go = new GameObject( "label" , typeof(RectTransform) , typeof(TMPro.TextMeshProUGUI) );
                    go.transform.SetParent( entry.transform , false );
                    go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , 32f );
                    var label = go.GetComponent<TMPro.TextMeshProUGUI>();
                    label.text = pod.title;
                    label.fontSize = 12;
                }
                
                // sub-pods:
                foreach( var subpod in pod.subpod_array )
                {
                    // image:
                    {
                        var parent = new GameObject( "image container" , typeof(RectTransform) ).GetComponent<RectTransform>();
                        parent.SetParent( entry.transform , false );
                        
                        var go = new GameObject( $"image {subpod.img.title}" , typeof(RectTransform) , typeof(RawImage) );
                        go.transform.SetParent( parent , false );
                        var image = await UWolframAlpha.Query.Internal.DownloadTextureGif( subpod.img.src );
                        _imagesToDispose.Enqueue( image );
                        go.GetComponent<RawImage>().texture = image;
                        var rect = go.GetComponent<RectTransform>();
                        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal , image.width );
                        rect.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , image.height );
                        
                        parent.SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , image.height );
                    }

                    // plain text
                    {
                        var go = new GameObject( "plaintext" , typeof(RectTransform) , typeof(TMPro.TextMeshProUGUI) );
                        go.transform.SetParent( entry.transform , false );
                        go.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical , 128 );
                        var label = go.GetComponent<TMPro.TextMeshProUGUI>();
                        label.text = subpod.plaintext;
                        label.fontSize = 12;
                    }
                }
            }
        }

        void Dispose ()
        {
            while( _imagesToDispose.Count!=0 )
                Object.Destroy( _imagesToDispose.Dequeue() );
        }
        
    }
}
