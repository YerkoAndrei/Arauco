using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fondo_sangre : MonoBehaviour {
	private bool violencia = true;
	private Image fondo;
	private float transparencia;
	private Color color;
	private float formula_transparencia;	// Fórmula de transparencia según vida

	// Use this for initialization
	void Start () {
		violencia = Variables_globales.violencia;
		fondo = GetComponent<Image> ();
		color = fondo.color;
	}

	// Update is called once per frame
	void Update () {
		if (violencia) {
			if (Protagonista_comp.vida >= Protagonista_comp.vida_max)
				transparencia = 0f;
			else {
				formula_transparencia = (((((Protagonista_comp.vida * 100) / Protagonista_comp.vida_max) * 0.01f) - 1f) * -1f) - 0.1f;
				transparencia = formula_transparencia;
			}
			
			color.a = transparencia;	//color.a = alpha = transparencia
			fondo.color = color;
		}
	}
}
