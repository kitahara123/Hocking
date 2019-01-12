using UnityEngine;

namespace DefaultNamespace
{
    public class WebLoadingBillboard : MonoBehaviour
    {
        public void Operate()
        {
            Managers.Managers.Images.GetWebImage(OnWebImage);
        }

        private void OnWebImage(Texture2D image)
        {
            GetComponent<Renderer>().material.mainTexture = image;
        }
    }
}