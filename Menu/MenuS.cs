using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuS : MonoBehaviour {

    //sonido de botones
    public AudioSource source;
    public AudioClip encima;
    public AudioClip dropdown;
    public AudioClip click;
    public GameObject panel;
  
    // para ocultar el panel se cambia el alpha chanel del Panel donde esta menu pausa
    public void CambiarPanel()
    {
        panel.GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }
    public void VolverPanel()
    {
        panel.GetComponent<Image>().color = new Color(227, 220, 189, 231);
    }
    //asignacion para no destruir objeto menu
    /*void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }*/

    //asignacion de sonidos botones
    public void OnEncima()
    {
        source.PlayOneShot(encima);
    }
    public void OnDropdown()
    {
        source.PlayOneShot(dropdown);
    }
    public void OnClick()
    {
        source.PlayOneShot(click);
    }
    //fin de asignacion de sonidos botones


    // funcion para reproducir el sonido de menu y luego cargar escena

      //  IEnumerator CargarEscena() { yield return new WaitForSeconds (1f); SceneManager.LoadScene ("v0.3"); }



    //selecion de idioma pantalla inicio
    public  void Ingles()
    {
		Variables_globales.idioma = "ingles";
        cargarMenu();
    }
    public  void Espanol()
    {
		Variables_globales.idioma = "espanol";
        cargarMenu();
    }
    public  void mapudungun()
    {
		Variables_globales.idioma = "mapu";
        cargarMenu();
    }
    void cargarMenu()
    {
        Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Menu_principal");
    }
    //fin de seleccion de idioma.


    //asignacion de escenas jugar y salir
   	public void jugar()
	{
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Tucapel_01");
    }
	public void jugarTutorial()
	{
		Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Tutorial");
	}
	public void salir()
	{
		Application.Quit ();
	}

     

}