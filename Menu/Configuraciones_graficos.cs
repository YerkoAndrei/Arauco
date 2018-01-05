using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using UnityEditor;

public class Configuraciones_graficos : MonoBehaviour {
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown textureQualityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Dropdown shadowDropdown;
    public Dropdown shadowResolutionDropdown;
    public Button aplicarButton;
     

    public Resolution[] resolutions;
    public Game_settings gameSettings;

    void OnEnable()
    {
        gameSettings = new Game_settings();
        //gameSettings.antialiasing = 2;

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        shadowDropdown.onValueChanged.AddListener(delegate { OnShadowsChange(); });
        shadowResolutionDropdown.onValueChanged.AddListener(delegate { OnShadowResolutionChange(); });
        aplicarButton.onClick.AddListener(delegate { OnAplicar(); });

        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
        

    }
    public void OnFullscreenToggle()
    {
       gameSettings.fullscreen= Screen.fullScreen = fullscreenToggle.isOn;
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }

    public void OnTextureQualityChange()
    {
       QualitySettings.masterTextureLimit= gameSettings.calidadTextura = textureQualityDropdown.value;
         
    }

    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gameSettings.antialiasing = (int)Mathf.Pow(2f, antialiasingDropdown.value);
    }

    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.VSync = vSyncDropdown.value;
    }

    public void OnShadowsChange()
    {
        gameSettings.shadowsQ = shadowDropdown.value;

        if (gameSettings.shadowsQ == 0)
        {
            QualitySettings.shadows = ShadowQuality.Disable;
            shadowResolutionDropdown.interactable = false;
            QualitySettings.shadowResolution = ShadowResolution.Low;
            shadowResolutionDropdown.value = 0;

        }
        if (gameSettings.shadowsQ == 1)
        {
            QualitySettings.shadows = ShadowQuality.HardOnly;
            shadowResolutionDropdown.interactable = true;
        }
        if(gameSettings.shadowsQ == 2)
        {
            QualitySettings.shadows = ShadowQuality.All;
            shadowResolutionDropdown.interactable = true;
        }
       
        
    }
    public void OnShadowResolutionChange()
    {
       int res= gameSettings.shadowResolution = shadowResolutionDropdown.value;
        if (res == 0)
        {
            QualitySettings.shadowResolution = ShadowResolution.Low;
        }
        if (res == 1)
        {
            QualitySettings.shadowResolution = ShadowResolution.Medium;
        }
        if (res == 2)
        {
            QualitySettings.shadowResolution = ShadowResolution.High;
        }
        if (res == 3)
        {
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        }

    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
        Debug.Log("YA ME GUARDE WEEE");
    }
    public void OnAplicar()
    {
        SaveSettings();
    }
    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<Game_settings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));

        antialiasingDropdown.value = gameSettings.antialiasing;
        vSyncDropdown.value = gameSettings.VSync;
        textureQualityDropdown.value = gameSettings.calidadTextura;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        shadowDropdown.value = gameSettings.shadowsQ;
        shadowResolutionDropdown.value = gameSettings.shadowResolution;
        fullscreenToggle.isOn = Screen.fullScreen;
       // Screen.fullScreen = gameSettings.fullscreen;
        resolutionDropdown.RefreshShownValue();

    }
 }
