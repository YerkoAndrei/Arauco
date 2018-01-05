using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Variables_globales : MonoBehaviour {

//	public static string idioma;
	public static string dificultad;
	public static bool violencia;
	public static bool puntero;
	public static string idioma;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);

		//Por defecto
		dificultad = "normal";
		violencia = true;
		puntero = true;
		idioma = "espanol";
	}
}
