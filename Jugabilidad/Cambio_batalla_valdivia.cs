using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambio_batalla_valdivia : MonoBehaviour {
	private GameObject[] lista_tropa_enemiga;
	public bool activo = false;
	public bool cambiar = false;
	public float timer = 2f;

	// Use this for initialization
	void Start () {
		lista_tropa_enemiga = GameObject.FindGameObjectsWithTag("Enemigo");
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach (GameObject tropa in lista_tropa_enemiga) {
			if (!tropa.GetComponentInParent<CapsuleCollider> ())
				i++;
		}
		if (i == lista_tropa_enemiga.Length)
			activo = true;
		if (activo) 
			timer -= Time.deltaTime;
		
		if (timer <= 0){
			cambiar = true;
			GetComponent<SphereCollider> ().enabled = true;
		}
		//print (timer);
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") 
			if(cambiar)
				Pantalla_carga.cargar_escena (SceneManager.GetActiveScene().name, "Tucapel_final");
	}
}
