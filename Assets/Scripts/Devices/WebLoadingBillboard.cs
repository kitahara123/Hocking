using UnityEngine;

public class WebLoadingBillboard : BaseDevice
{
    public override void Operate()
    {
        Managers.Managers.Images.GetWebImage(OnWebImage);
    }

    private void OnWebImage(Texture2D image)
    {
        GetComponent<Renderer>().material.mainTexture = image;
    }
}