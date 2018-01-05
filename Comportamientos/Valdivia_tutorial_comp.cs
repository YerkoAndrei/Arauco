using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class Valdivia_tutorial_comp : MonoBehaviour {

	public Transform punto_centro;

	public bool ataque_valdivia = false;
	public float vida_valdivia;
	public float vida_max_valdivia = 2000f;
	public float vel_cura_valdivia = 10f;			// Velocidad de regeneracion
	public float tiempo_reg_valdivia = 3f;			// Tiempo antes de regenerarse
	public Transform prota;

	public float timer_valdivia;
	public float dist_ataque;
	public float dist_caminar;
	public float dist_persec;							 //Distancia para perseguir
	public Transform objetivo;
	private GameObject[] lista_tropa_aliada;
	private Animator anim;

	// Daños por arma
	private float daño_espada = 15 -3;
	private float daño_lanza = 10 -3;
	private float daño_maza = 25 -3;

	// Sonidos
	public AudioSource fuente;
	public AudioClip quejido1, quejido2, quejido3, quejido4;
	public AudioClip golpe1, golpe2, golpe3, golpe4;
	public AudioClip correr;
	private float paso = 0.4f;

	// Efectos
	public ParticleSystem sangre;

	// Globales
	private string dificultad = "normal";	// eliminar iniciacion
	private bool violencia = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		violencia = Variables_globales.violencia;
		dificultad = Variables_globales.dificultad;

		dist_ataque = 1.5f;
		dist_caminar = 4f;
	}

	// Update is called once per frame
	void Update () {
		        
		//¿Está el protagonista cerca?
		if (Vector3.Distance (prota.position, this.transform.position) < 7f) {
			objetivo = prota;
			dist_persec = 7f;
		}
		else{
			objetivo = punto_centro; //si no, el objetivo es el punto centro
			dist_persec = 20f;
		}

		// Persigue
		perseguir();

		// Si lo dañan mientras ataca, el ataque es false
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Reaccion"))
			ataque_valdivia = false;

		// Correr / Caminar
		if (Vector3.Distance (objetivo.position, this.transform.position) > dist_caminar) {
			anim.SetBool ("Correr", true);
			paso -= Time.deltaTime;
			if(paso <= 0 && !anim.GetBool ("Quieto"))
				sonido (2);
		}
		else
			anim.SetBool ("Correr", false);
		
		// Distancia para bloquear
		if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque) {
			anim.SetBool ("Atacando", true);
			/*if (!ataque_valdivia)
				atacar ();*/
		}
		else
			anim.SetBool ("Atacando", false);dist_ataque = 1.6f;
		dist_caminar = 6f;

		// Si esta quieto, mira al prota
		if(objetivo == punto_centro && anim.GetCurrentAnimatorStateInfo(0).IsName("Quieto"))
			transform.LookAt (prota.position);
		//print();
	}
	// Perseguir
	void perseguir()
	{
		if (Vector3.Distance (objetivo.position, this.transform.position) < dist_persec) {//distancia para perseguir
			Vector3 direction = objetivo.position - this.transform.position;
			direction.y = 0;
			this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 1f);
			if (direction.magnitude > 20f) {
				this.transform.Translate (0, 0, 0.1f);
			}
		} 
	}
	// Daño por arma
	void OnTriggerEnter(Collider col)
	{
		if (col.tag != "Terreno" && col.transform.root != transform) {
			// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
			if (col.tag == "Arma") {	//si el daño lo hace el prota
				if (Protagonista_tutorial_comp.ataque) {
					switch (col.transform.name) {
					case "Espada":
						// ¡Me pegaste con la espada!
						anim.SetTrigger ("Reaccion maza");
						break;
					case "Lanza":
						// ¡Me pegaste con la lanza!
						anim.SetTrigger ("Reaccion lanza");
						break;
					case "Martillo":
						// ¡Me pegaste con el martillo!
						anim.SetTrigger ("Reaccion maza");
						break;
					}
					Protagonista_tutorial_comp.ataque = false;
					sonido(1);
					efecto (1);
				}
			}
		}
	}
	void atacar()
	{
		anim.SetTrigger ("Ataque espada");
	}
	// El animador llama a estos metodos  y define si el ataque esta activo o no
	public void Comienzo_anim (){
		ataque_valdivia = true;
	}
	public void Termino_anim(){
		ataque_valdivia = false;
	}
	void sonido(int son)
	{
		switch (son) 
		{
		case 1:
			//suena un quejido y un golpe aleatorio
			float aleatorio = Random.Range (0, 100);
			if(aleatorio >= 0 && aleatorio < 25)
				fuente.PlayOneShot(quejido1);
			else if (aleatorio >= 25 && aleatorio < 50)
				fuente.PlayOneShot(quejido2);
			else if (aleatorio >= 50 && aleatorio < 75)
				fuente.PlayOneShot(quejido3);
			else if (aleatorio >= 75 && aleatorio < 100)
				fuente.PlayOneShot(quejido4);

			float aleatorio2 = Random.Range (0, 100);
			if(aleatorio2 >= 0 && aleatorio2 < 25)
				fuente.PlayOneShot(golpe1);
			else if (aleatorio2 >= 25 && aleatorio2 < 50)
				fuente.PlayOneShot(golpe2);
			else if (aleatorio2 >= 50 && aleatorio2 < 75)
				fuente.PlayOneShot(golpe3);
			else if (aleatorio2 >= 75 && aleatorio2 < 100)
				fuente.PlayOneShot(golpe4);
			break;
		case 2:
			//sonido de correr, loop
			paso = 0.4f;
			fuente.Stop ();
			fuente.PlayOneShot(correr);
			break;
		}
	}
	void efecto(int efecto)
	{
		if (violencia) {
			switch (efecto) {
			case 1:
				sangre.Play ();
				break;
			}
		}
	}
}