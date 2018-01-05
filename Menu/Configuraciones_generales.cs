using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Configuraciones_generales : MonoBehaviour {

    public Dropdown dificultadDropdown;
    public Dropdown nivelViolenciaDropdown;
    public Dropdown idiomaDropdown;
    public Button guardar;
    public Slider slSpeed;
    public Toggle puntero;


    public Game_confi configuracion;

    private void OnEnable()
    {
        configuracion = new Game_confi();
        dificultadDropdown.onValueChanged.AddListener(delegate { OnDificultadChange(); });
        nivelViolenciaDropdown.onValueChanged.AddListener(delegate { OnNivelViolenciaChange(); });
        idiomaDropdown.onValueChanged.AddListener(delegate { OnIdiomaChange(); });
        guardar.onClick.AddListener(delegate { OnGuardar(); });
        slSpeed.onValueChanged.AddListener(delegate { cambiarVelocidad(); });
        puntero.onValueChanged.AddListener(delegate { activarPuntero(); });

        LoadConfiguracion();
    }

    public void activarPuntero()
    {
        if (puntero.isOn)
        {
            configuracion.puntero= Variables_globales.puntero = true;
        }
        if (!puntero.isOn)
        {
            configuracion.puntero = Variables_globales.puntero = false;
        }
    }

   public  void cambiarVelocidad()
    {
        configuracion.sensibilidad = slSpeed.value;
    }
    public void OnDificultadChange()
    {
        configuracion.dificultad = dificultadDropdown.value;
        if (dificultadDropdown.value == 0)
        {
            Variables_globales.dificultad = "facil";
        }
        if (dificultadDropdown.value == 1)
        {
            Variables_globales.dificultad = "normal";
        }
        if (dificultadDropdown.value == 2)
        {
            Variables_globales.dificultad = "dificil";
        }
    }
    public void OnNivelViolenciaChange()
    {
        configuracion.nivelViolencia = nivelViolenciaDropdown.value;
        if (nivelViolenciaDropdown.value == 0)
        {
            Variables_globales.violencia = true;
        }
        if (nivelViolenciaDropdown.value == 1)
        {
            Variables_globales.violencia = false;
        }
    }

    public void OnIdiomaChange()
    {
        configuracion.idioma = idiomaDropdown.value;
        if (idiomaDropdown.value == 0)
        {
			Variables_globales.idioma = "espanol";
        }
        if (idiomaDropdown.value == 1)
        {
			Variables_globales.idioma = "ingles";
        }
        if (idiomaDropdown.value == 2)
        {
			Variables_globales.idioma = "mapu";
        }
    }
    public void OnGuardar()
    {
        SaveSettings();
    }

    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(configuracion, true);
        File.WriteAllText(Application.persistentDataPath + "/general.json", jsonData);
    }

    public void LoadConfiguracion()
    {
		if (Variables_globales.idioma == "ingles")
        { idiomaDropdown.value = 1; }
		if (Variables_globales.idioma == "mapu")
        { idiomaDropdown.value = 2; }
		if (Variables_globales.idioma == "espanol")
        { idiomaDropdown.value = 0; }

		if (Variables_globales.dificultad == "normal")
		{ dificultadDropdown.value = 1; }
		if (Variables_globales.dificultad == "dificil")
		{ dificultadDropdown.value = 2; }
		if (Variables_globales.dificultad == "facil")
		{ dificultadDropdown.value = 0; }

        configuracion = JsonUtility.FromJson<Game_confi>(File.ReadAllText(Application.persistentDataPath + "/general.json"));
        dificultadDropdown.value = configuracion.dificultad;
        idiomaDropdown.value = configuracion.idioma;
        nivelViolenciaDropdown.value = configuracion.nivelViolencia;
        slSpeed.value = configuracion.sensibilidad;
        puntero.isOn = configuracion.puntero;
    } 
    
}
