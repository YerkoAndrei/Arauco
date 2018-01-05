using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System;

public class ReproduccionVideo : MonoBehaviour {
    [SerializeField]
    private VideoPlayer vpE, vpI;
    string idiomaV;

	private float tiempo_minimo = 2f;

    // Use this for initialization
    void Start () {
		idiomaV = Variables_globales.idioma;
		//debug
		if (idiomaV == null)
			idiomaV = "espanol";

        Video(idiomaV);
	}

    private void Video(string a)
    {
		if (a.Equals("espanol") || a.Equals("mapu"))
        {
            vpE.gameObject.SetActive(true);
            vpI.gameObject.SetActive(false);
        }
        if (a.Equals("ingles"))
        {
            vpE.gameObject.SetActive(false);
            vpI.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
		// Para cancelar
		/*if (Input.GetButtonDown("Menu") &&  vpE.isPlaying){
			vpE.Stop();
            SceneManager.LoadScene("Pantalla_carga");
        }
        if (Input.GetButtonDown("Menu") && vpI.isPlaying)
        {
            vpE.Stop();
            SceneManager.LoadScene("Pantalla_carga");
        }*/

		// cuando el video termine
		if (idiomaV.Equals ("espanol") || idiomaV.Equals ("mapu")) {
			if (Convert.ToInt32(vpE.frame) == Convert.ToInt32(vpE.frameCount)) {
				Pantalla_carga.vid = false;
				Pantalla_carga.cargar_escena (SceneManager.GetActiveScene ().name, Pantalla_carga.destino);
			}
		}
		if (idiomaV.Equals ("ingles")) {
			if (Convert.ToInt32(vpI.frame) == Convert.ToInt32(vpI.frameCount)) {
				Pantalla_carga.vid = false;
				Pantalla_carga.cargar_escena (SceneManager.GetActiveScene ().name, Pantalla_carga.destino);
			}
		}
	}
}
