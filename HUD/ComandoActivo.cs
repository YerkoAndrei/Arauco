using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComandoActivo : MonoBehaviour {
    // Use this for initialization
	[SerializeField]
	private Image comActivo, seguirProtagonista, mantenerPosicion;
	private float doble_click;				// para evitar que el doble click
    private bool prendido1 = false, prendido=false;

    // Update is called once per frame
    private void Update()
	{
		if (doble_click <= 0.0f) {
			if (Input.GetButtonDown ("Detener tropas")) {
				doble_click = 0.4f;
				if (prendido == false) {
					comActivo.GetComponent<Image> ().color = new Color (255, 255, 255, 255);
					comActivo.transform.position = mantenerPosicion.GetComponent<RectTransform> ().position;
					prendido = true;
					prendido1 = false;
				} else if (prendido == true) {
					prendido = false;
					prendido1 = false;
					comActivo.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
				}
			}
			if (Input.GetButtonDown ("Acercar tropas")) {
				doble_click = 0.4f;
				if (prendido1 == false) {
					comActivo.GetComponent<Image> ().color = new Color (255, 255, 255, 255);
					comActivo.transform.position = seguirProtagonista.GetComponent<RectTransform> ().position;
					prendido1 = true;
					prendido = false;
				} else if (prendido1 == true) {
					prendido1 = false;
					prendido = false;
					comActivo.GetComponent<Image> ().color = new Color (0, 0, 0, 0);
				}
			}
		}
		doble_click -= Time.deltaTime;
	}
}
