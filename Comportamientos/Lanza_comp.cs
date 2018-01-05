using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lanza_comp : MonoBehaviour {
	
	private GameObject objeto_mano_der;
	private Transform mano_der;

	private GameObject objeto_inv;
	private Transform inv;

	private bool tipo_prota; 	//si el protagonista es del titurial o de batalla
	//public static float daño_lanza = 10f;

	// Use this for initialization
	void Start () {
		objeto_mano_der = GameObject.Find ("Equipo derecha");
		mano_der = objeto_mano_der.GetComponent<Transform> ();

		objeto_inv = GameObject.Find ("Inventario derecha");
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
			if (Protagonista_comp.arma == 2) {
				this.GetComponent<MeshRenderer> ().enabled = true;
				this.GetComponent<CapsuleCollider> ().enabled = true;
				this.transform.SetParent (mano_der);
			} else {
				this.GetComponent<MeshRenderer> ().enabled = false;
				this.GetComponent<CapsuleCollider> ().enabled = false;
				this.transform.SetParent (inv);
			}
		}else{
			if (Protagonista_tutorial_comp.arma == 2) {
				this.GetComponent<MeshRenderer> ().enabled = true;
				this.GetComponent<CapsuleCollider> ().enabled = true;
				this.transform.SetParent (mano_der);
			} else {
				this.GetComponent<MeshRenderer> ().enabled = false;
				this.GetComponent<CapsuleCollider> ().enabled = false;
				this.transform.SetParent (inv);
			}
		}
	}

}
