using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambios_pantalla : MonoBehaviour {
	private GameObject[] lista_tropa_enemiga;
	private bool activo = true;	//evita el loop

	// Use this for initialization
	void Start () {
		lista_tropa_enemiga = GameObject.FindGameObjectsWithTag("Enemigo");
	}
	// Update is called once per frame
	void Update () {
		int i  = 0;
		foreach (GameObject tropa in lista_tropa_enemiga){
			if (!tropa.GetComponentInParent<CapsuleCollider>()) 
				i++;
		} 

		if (i == lista_tropa_enemiga.Length && activo) {
			activo = false;

			if (this.gameObject.name == "Collider primer pasillo") {
				GameObject.Find ("puerta_abierta_1").GetComponent<MeshRenderer> ().enabled = true;

				GameObject.Find ("puerta_cerrada_1").GetComponent<MeshRenderer> ().enabled = false;
				GameObject.Find ("puerta_cerrada_1").GetComponent<MeshCollider> ().enabled = false;
			} 
			else if (this.gameObject.name == "Collider segundo pasillo") {
				GameObject.Find ("puerta_abierta_2").GetComponent<MeshRenderer> ().enabled = true;

				GameObject.Find ("puerta_cerrada_2").GetComponent<MeshRenderer> ().enabled = false;
				GameObject.Find ("puerta_cerrada_2").GetComponent<MeshCollider> ().enabled = false;
			} 
			else if (this.gameObject.name == "Collider final") {
				GameObject.Find ("puerta_abierta_3").GetComponent<MeshRenderer> ().enabled = true;
				GameObject.Find ("puerta_cerrada_3").GetComponent<MeshRenderer> ().enabled = false;
				GameObject.Find ("puerta_cerrada_3").GetComponent<MeshCollider> ().enabled = false;

				GameObject.Find ("puerta_abierta_4").GetComponent<MeshRenderer> ().enabled = true;
				GameObject.Find ("puerta_cerrada_4").GetComponent<MeshRenderer> ().enabled = false;
				GameObject.Find ("puerta_cerrada_4").GetComponent<MeshCollider> ().enabled = false;
			}
		}
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			if (this.gameObject.name == "Collider primer pasillo") {
				Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Tucapel_02");
			} else if (this.gameObject.name == "Collider segundo pasillo") {
				Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Tucapel_03");
			} else if (this.gameObject.name == "Collider final") {
				Pantalla_carga.cargar_escena(SceneManager.GetActiveScene().name, "Tucapel_04");
			}
		}
	}
}