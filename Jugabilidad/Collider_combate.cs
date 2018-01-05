using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider_combate : MonoBehaviour {
	private GameObject[] lista_tropa_aliada_lanza;
	private GameObject[] lista_tropa_aliada_maza;
	private GameObject[] lista_tropa_enemiga_espada;
	private GameObject[] lista_tropa_enemiga_pica;
	private GameObject jefe_hacha;
	private GameObject jefe_mandoble;
	private GameObject caupolican;
	private GameObject valdivia;
	private GameObject valdivia_final;

	private float espera_español;
	private float espera_jefe;
	private bool espera_activo_español = false;
	private bool espera_activo_jefe = false;
	private bool activo = false;

	// Use this for initialization
	void Start () {
		lista_tropa_aliada_lanza = GameObject.FindGameObjectsWithTag("Aliado lanza");
		lista_tropa_aliada_maza = GameObject.FindGameObjectsWithTag("Aliado maza");
		lista_tropa_enemiga_espada = GameObject.FindGameObjectsWithTag("Enemigo espada");
		lista_tropa_enemiga_pica = GameObject.FindGameObjectsWithTag("Enemigo pica");
		jefe_hacha = GameObject.FindGameObjectWithTag("Jefe hacha");
		jefe_mandoble = GameObject.FindGameObjectWithTag("Jefe mandoble");
		caupolican = GameObject.FindGameObjectWithTag("Caupolican");
		valdivia = GameObject.FindGameObjectWithTag("Valdivia");
		valdivia_final = GameObject.Find("Valdivia final");
	}
	
	// Update is called once per frame
	void Update () {
		if (activo) {
			espera_español -= Time.deltaTime;
			espera_jefe -= Time.deltaTime;

			if (espera_español <= 0.0f)
				espera_activo_español = true;
			
			if (espera_jefe <= 0.0f)
				espera_activo_jefe = true;
		}
		mover_tropas_españolas ();
		mover_jefes ();
	}
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player") {
			espera_español = 4.0f;
			espera_jefe = 6.0f;
			activo = true;
			foreach (GameObject tropa in lista_tropa_aliada_lanza) {
				Animator anim1 = tropa.GetComponent<Animator> ();
				anim1.SetBool ("Quieto", false);
			}
			foreach (GameObject tropa in lista_tropa_aliada_maza) {
				Animator anim2 = tropa.GetComponent<Animator> ();
				anim2.SetBool ("Quieto", false);
			}
			if (valdivia_final != null) {
				Animator anim3 = valdivia_final.GetComponent<Animator> ();
				anim3.SetBool ("Quieto", false);
				Destroy (gameObject);
			}
			Destroy (GetComponent<BoxCollider>());
			Destroy (gameObject, 9.0f);
		}
	}
	void mover_tropas_españolas()		// Los españoles y caupolican comienzan a correr despues (cuando los "ven" acercarse)
	{
		if (espera_activo_español) {
			foreach (GameObject tropa in lista_tropa_enemiga_espada) {
				Animator anim1 = tropa.GetComponent<Animator> ();
				anim1.SetBool ("Quieto", false);
			}
			foreach (GameObject tropa in lista_tropa_enemiga_pica) {
				Animator anim2 = tropa.GetComponent<Animator> ();
				anim2.SetBool ("Quieto", false);
			}
			if (caupolican != null) {
				Animator anim3 = caupolican.GetComponent<Animator> ();
				anim3.SetBool ("Quieto", false);
			}
		}
	}
	void mover_jefes(){				// los jefes comienzan despues
		if (espera_activo_jefe) {
			if (jefe_hacha != null) {
				Animator anim1 = jefe_hacha.GetComponent<Animator> ();
				anim1.SetBool ("Quieto", false);
			}
			if (jefe_mandoble != null) {
				Animator anim2 = jefe_mandoble.GetComponent<Animator> ();
				anim2.SetBool ("Quieto", false);
			}
			if (valdivia != null) {
				Animator anim3 = valdivia.GetComponent<Animator> ();
				anim3.SetBool ("Quieto", false);
			}
		}
	}
}
