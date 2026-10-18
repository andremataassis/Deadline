using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;
    public static float brightness = 0.8f;
    public Light mainLight;
    public Slider brightnessSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        mainLight.intensity = brightness;
        brightnessSlider.value = brightness;
    }

    public void updateBrightness(float a)
    {
        brightness = a;
        mainLight.intensity = brightness;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
