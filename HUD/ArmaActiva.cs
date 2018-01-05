using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmaActiva : MonoBehaviour {
    [SerializeField]
    private Image activa,espada,lanza,maza; // activa servira para poner un marco al arma que se encuentre activa, las demas variables sirven para copiar su posicion a activa
    
	private bool tipo_prota; 	//si el protagonista es del titurial o de batalla

	void Start () {
		if (GameObject.Find ("Protagonista") != null)
			tipo_prota = true;
		if (GameObject.Find ("Protagonista tutorial") != null)
			tipo_prota = false;
	}

	void Update () {
      	//consulta que arma esta activa en el protagonista
		if (tipo_prota) {
			if (Protagonista_comp.arma == 1) {
				activa.transform.position = espada.GetComponent<RectTransform> ().position; // copia la posicion del icono y la asigna a la imagen
			}
			if (Protagonista_comp.arma == 2) {
				activa.transform.position = lanza.GetComponent<RectTransform> ().position;
			}
			if (Protagonista_comp.arma == 3) {
				activa.transform.position = maza.GetComponent<RectTransform> ().position;
			}
		} else {
			if (Protagonista_tutorial_comp.arma == 1) {
				activa.transform.position = espada.GetComponent<RectTransform> ().position; // copia la posicion del icono y la asigna a la imagen
			}
			if (Protagonista_tutorial_comp.arma == 2) {
				activa.transform.position = lanza.GetComponent<RectTransform> ().position;
			}
			if (Protagonista_tutorial_comp.arma == 3) {
				activa.transform.position = maza.GetComponent<RectTransform> ().position;
			}
		}
    }
}
