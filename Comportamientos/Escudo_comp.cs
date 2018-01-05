using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo_comp : MonoBehaviour {

	private GameObject objeto_mano_izq;
	private Transform mano_izq;

	private GameObject objeto_inv;
	private Transform inv;

	private bool tipo_prota; 	//si el protagonista es del titurial o de batalla
	// Use this for initialization
	void Start () {
		objeto_mano_izq = GameObject.Find ("Equipo izquierda");
		mano_izq = objeto_mano_izq.GetComponent<Transform> ();

		objeto_inv = GameObject.Find ("Inventario izquierda");
		inv = objeto_inv.GetComponent<Transform> ();

		if (GameObject.Find ("Protagonista") != null)
			tipo_prota = true;
		if (GameObject.Find ("Protagonista tutorial") != null)
			tipo_prota = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (tipo_prota) {
			if (Protagonista_comp.arma == 1) {
				this.GetComponent<MeshRenderer> ().enabled = true;
				this.GetComponent<MeshCollider> ().enabled = true;
				this.transform.SetParent (mano_izq);
			} else {
				this.GetComponent<MeshRenderer> ().enabled = false;
				this.GetComponent<MeshCollider> ().enabled = false;
				this.transform.SetParent (inv);
			}
		}else{
			if (Protagonista_tutorial_comp.arma == 1) {
				this.GetComponent<MeshRenderer> ().enabled = true;
				this.GetComponent<MeshCollider> ().enabled = true;
				this.transform.SetParent (mano_izq);
			} else {
				this.GetComponent<MeshRenderer> ().enabled = false;
				this.GetComponent<MeshCollider> ().enabled = false;
				this.transform.SetParent (inv);
			}
		}
	}
}
