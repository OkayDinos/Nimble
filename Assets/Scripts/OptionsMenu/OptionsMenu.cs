using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixer audioMixerSFX;

    public Dropdown resolutionDropdown;
    Resolution[] resolutions;

    [SerializeField]
    TextMeshProUGUI mr_MusicValue, mr_SFXValue;

    private void Start()
    {

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

    }
    public void SetVolume(float af_SliderValue) 
    {

        audioMixer.SetFloat("musicVolume", Mathf.Log10(af_SliderValue) * 20);

        float lf_Value = af_SliderValue;
        if (lf_Value == 0.0001)
            lf_Value = 0;
        lf_Value = lf_Value * 100;

        mr_MusicValue.text = $"{(int)lf_Value}";


    }

    public void SetSFXVolume(float af_SliderValue)
    {

        audioMixerSFX.SetFloat("sfxVolume", Mathf.Log10(af_SliderValue) * 20);

        float lf_Value = af_SliderValue;
        if (lf_Value == 0.0001)
            lf_Value = 0;
        lf_Value = lf_Value * 100;



        mr_MusicValue.text = $"{(int)lf_Value}";

    }

    public void SetQuality(int qualityIndex) 
    {

        QualitySettings.SetQualityLevel(qualityIndex);
    
    }

    public void SetFullScreen(bool isFullScreen) 
    {

        Screen.fullScreen = isFullScreen;
    
    }
}
