using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Audio;


public class Configuracion_audio : MonoBehaviour {
     
    public Slider volGeneral, volMusica, volSfx;
    
    public AudioMixer masterMixer;
    private Game_sonido sonido;

    void OnEnable()
    {
        //sonido = new Game_sonido();
        /*volMusica.value = -15f;
        volSfx.value = -15f;
        volGeneral.value = -15f;*/
		volGeneral.onValueChanged.AddListener(delegate { SetVolumenGeneral(); });
        volMusica.onValueChanged.AddListener(delegate { SetVolumenMusica(); });
        volSfx.onValueChanged.AddListener(delegate { SetVolumenSfx(); });
        LoadState();
    }
    
    public void SetVolumenGeneral()
    {
		float v_g = volGeneral.value;
        //sonido.volumenGeneral = v_g;
        masterMixer.SetFloat("VolumenGeneral", v_g);
    }
    public void SetVolumenSfx()
    {
		float v_e= volSfx.value;
        //sonido.volumenMusica = v_e;
        masterMixer.SetFloat("VolumenSfx", v_e);
    }
    public void SetVolumenMusica()
    {
		float v_m = volMusica.value;
        //sonido.volumenMusica = v_m;
        masterMixer.SetFloat("VolumenMusica", v_m);
    }
    void LoadState()
    {
        sonido = JsonUtility.FromJson<Game_sonido>(File.ReadAllText(Application.persistentDataPath + "/Audio.json"));
        
        /*if (sonido == null)
        {
            volGeneral.value = -15f;
            volMusica.value = -15f;
            volSfx.value = -15f;
            masterMixer.SetFloat("VolumenGeneral", -15f);
            masterMixer.SetFloat("VolumenMusica", -15f);
            masterMixer.SetFloat("VolumenSfx", -15f);
        }
        else
        {*/
           	float gen= volGeneral.value = sonido.volumenGeneral;
           	float mus= volMusica.value = sonido.volumenMusica;
            float sfx= volSfx.value = sonido.volumenSfx;
            masterMixer.SetFloat("VolumenGeneral", gen);
            masterMixer.SetFloat("VolumenMusica", mus);
            masterMixer.SetFloat("VolumenSfx", sfx);
        //}
    }
    public void GuardarSonidoConfig()
    {
        string jsonData = JsonUtility.ToJson(sonido, true);
        File.WriteAllText(Application.persistentDataPath + "/Audio.json", jsonData);
    }
}
