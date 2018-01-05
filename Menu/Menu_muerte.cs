using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_muerte : MonoBehaviour {
	private float timer = 3.5f;
	public GameObject hud;
	public GameObject menu_pausa,n;

	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Protagonista_comp.vida <= 0)
			timer -= Time.deltaTime;
		
		if (timer <= 0) {
			Time.timeScale = 0.0f;
			hud.SetActive(false);
			menu_pausa.GetComponentInChildren<Menu_pausa> ().isPausado = true;
			menu_pausa.GetComponentInChildren<Menu_pausa> ().pausaPanel.SetActive (false);
			menu_pausa.SetActive (false);
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			foreach(Transform child in transform)
				child.gameObject.SetActive (true);
		}
	}

	public void reintentar()
	{
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, SceneManager.GetActiveScene().name);
	}

	public void menu_principal()
	{
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Menu_principal");
        n.gameObject.SetActive(false);
       
	}
}
