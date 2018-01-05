using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntero : MonoBehaviour {
	private Image fondo;
	private float transparencia;
	private Color color;

	public bool puntero = true;

	void Start () {
		puntero = Variables_globales.puntero;
		fondo = GetComponent<Image> ();
		color = fondo.color;
		transparencia = 0.0f;
	}
	// Update is called once per frame
	void Update () {
        puntero = Variables_globales.puntero;
        if (puntero) {
			if (Input.GetButtonDown ("Defensa") && Protagonista_comp.camara)
				transparencia = 0.6f;
		
			if (Input.GetButtonUp ("Defensa"))
				transparencia = 0.0f;
						
			if (ThirdPersonOrbitCam.targetCamOffset.z < -10f)
				transparencia = 0.0f;
		
			if (ThirdPersonOrbitCam.targetCamOffset.z > -10f && Input.GetButton ("Defensa"))
				transparencia = 0.6f;
		
		} else 
			transparencia = 0.0f;
		
		color.a = transparencia;
		fondo.color = color;
	}
}
