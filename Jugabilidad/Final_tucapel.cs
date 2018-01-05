using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Final_tucapel : MonoBehaviour {
	private GameObject valdivia;
	public bool activo = false;
	public bool cambiar = false;
	public float timer = 2f;

	// Use this for initialization
	void Start () {
		valdivia = GameObject.Find("Valdivia final");
	}
	
	// Update is called once per frame
	void Update () {
		if (!valdivia.GetComponentInParent<CapsuleCollider> ())
			activo = true;

		if (activo)
			timer -= Time.deltaTime;
		
		if (timer <= 0) {
			cambiar = true;
			transform.position = valdivia.transform.position;
		}
		//print (timer);
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Arma" && Protagonista_comp.ataque) 
				Pantalla_carga.cargar_escena (SceneManager.GetActiveScene().name, "Menu_principal");
	}
}
