using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Menu_pausa : MonoBehaviour {

	//private float timer = 2f;
    public GameObject pausaPanel,hud, mSonido,mSalir,mGeneral,mGrafico, mGlosario, mPausa, mOpciones;
    public AudioMixerSnapshot paused, unpaused;
    public bool isPausado;
    public AudioClip menuPausaSonido;
    public AudioSource audiofuente;

	// Use this for initialization
	void Start () {
       isPausado = false;
    }
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update () {
		if (isPausado)
			PausarJuego (true);
		else
			PausarJuego (false);
		
		if (Input.GetButtonDown ("Menu")) {
			SwitchPausa ();
			audiofuente.PlayOneShot (menuPausaSonido);
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = true;
		}
	}

    void PausarJuego(bool state)
    {
        if (state)
        {
			pausaPanel.SetActive (true);
			pausaPanel.GetComponent<Image>().color = new Color(227, 220, 189, 231);
			Time.timeScale = 0.0f;
			hud.SetActive (false);
        }
        else
         {
			pausaPanel.SetActive (false);
			mGrafico.SetActive (false);
			mGlosario.SetActive (false);
			mGeneral.SetActive (false);
			mSalir.SetActive (false);
			mSonido.SetActive (false);
			mPausa.SetActive (true);
			mOpciones.SetActive (false);

			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.lockState = CursorLockMode.Confined;
			Time.timeScale = 1.0f;
			hud.SetActive (true);
        }
        pausaPanel.SetActive(state);
    }
    public void CerrarJuego()
    {
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Menu_principal");
    }

	public void ReiniciarPartida()
	{
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name);
	}

    public void SwitchPausa()
    {
        if (isPausado)
        {
            isPausado = false;
        }
        else
        {
            isPausado = true;
        }
    }
}
