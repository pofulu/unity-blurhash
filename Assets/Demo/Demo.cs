using Blurhash.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    [SerializeField] Text textBlurhash;
    [SerializeField] RawImage imgDecode;

    public void Encode(Texture2D texture)
    {
        var blurhash = BlurHash.EncodeToBlurHash(texture);
        textBlurhash.text = blurhash;
        imgDecode.texture = BlurHash.DecodeToTexture2D(blurhash, 20, 15);
    }
}
