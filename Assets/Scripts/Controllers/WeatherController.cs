using UnityEngine;

/// <summary>
/// Меняет скайбокс с песмурного на чистый и обратно
/// </summary>
public class WeatherController : MonoBehaviour
{
    [SerializeField] private Material sky;
    [SerializeField] private Light sun;
    [SerializeField] private float blendingSpeed = .0005f;

    private float fullIntensity;
    private float cloudValue = 0f;
    private bool _switch = true;

    private void Start()
    {
        fullIntensity = sun.intensity;
    }

    private void Update()
    {
        SetOvercast(cloudValue);
        if (cloudValue <= 1f && _switch)
        {
            cloudValue += blendingSpeed;
        }
        else if (cloudValue <= 0f)
        {
            _switch = true;
        }
        else
        {
            cloudValue -= blendingSpeed;
            _switch = false;
        }
    }

    private void SetOvercast(float value)
    {
        sky.SetFloat("_Blend", value);
        sun.intensity = fullIntensity - (fullIntensity * value);
    }
}