using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Valdivia_final_comp : MonoBehaviour {

	public bool ataque_valdivia = false;
	public static float vida_valdivia_final;
	public static float vida_max_valdivia_final;
	public float vel_cura_valdivia = 20f;			// Velocidad de regeneracion
	public float tiempo_reg_valdivia = 3f;			// Tiempo antes de regenerarse
	public Transform prota;

	public float timer_valdivia;
	public float dist_ataque;
	public float dist_caminar;
	public Transform objetivo;
	private GameObject[] lista_tropa_aliada;
	private Animator anim;

	// Daños por arma
	private float daño_espada = 15;
	private float daño_lanza = 2;
	private float daño_maza = 25;

	// Sonidos
	public AudioSource fuente;
	public AudioClip quejido1, quejido2, quejido3, quejido4;
	public AudioClip golpe1, golpe2, golpe3, golpe4;
	public AudioClip muerte;
	public AudioClip correr;
	private float paso = 0.4f;

	// Efectos
	public ParticleSystem sangre;
	public GameObject mancha_sangre;

	// Globales
	private string dificultad = "normal";	// Por defecto
	private bool violencia = true;

	// Use this for initialization
	void Start () {
		vida_max_valdivia_final = 600f;
		anim = GetComponent<Animator> ();

		dificultad = Variables_globales.dificultad;
		violencia = Variables_globales.violencia;

		lista_tropa_aliada = GameObject.FindGameObjectsWithTag("Aliado");
		dist_ataque = 1.6f;
		dist_caminar = 5f;

		// La dificultad multiplica la vida
		switch (dificultad) {
		case "facil":
			vida_max_valdivia_final = vida_max_valdivia_final - 550;
			break;
		case "normal":
			vida_max_valdivia_final = vida_max_valdivia_final;
			break;
		case "dificil":
			vida_max_valdivia_final = vida_max_valdivia_final + 100;
			break;
		}
		vida_valdivia_final = vida_max_valdivia_final;
		objetivo = prota;
	}

	// Update is called once per frame
	void Update () {
		// Si no esta muerto, perseguir
		if (vida_valdivia_final > 0)
		{
			if (vida_valdivia_final < vida_max_valdivia_final) {
				tiempo_reg_valdivia -= Time.deltaTime;	// Comienza a regenerar vida despues del tiempo asignado
				if (tiempo_reg_valdivia <= 0.0f)
					vida_valdivia_final += vel_cura_valdivia * Time.deltaTime;
			}

			if (Vector3.Distance (objetivo.position, this.transform.position) < 20) {//distancia para perseguir
				Vector3 direction = objetivo.position - this.transform.position;
				direction.y = 0;
				this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 1f);
				if (direction.magnitude > 40f) {
					this.transform.Translate (0, 0, 0.1f);
				}
			}

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
			
			// Distancia para atacar
			if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque) {
				anim.SetBool ("Atacando", true);
				if (!ataque_valdivia && vida_valdivia_final > 0)
					atacar ();
			}
			else
				anim.SetBool ("Atacando", false); //si esta atacando dejará de avanzar y se quedará quieto		
		}
		//print (vida_valdivia);
	}

	// Daño por arma
	void OnTriggerEnter(Collider col)
	{
		if (vida_valdivia_final > 0 && col.tag != "Terreno" && col.transform.root != transform) {
			// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
			if (col.tag == "Arma") {
				if (Protagonista_comp.ataque) {
					switch (col.transform.name) {
					case "Espada":
						vida_valdivia_final = vida_valdivia_final - daño_espada;
						if (vida_valdivia_final <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					case "Lanza":
						vida_valdivia_final = vida_valdivia_final - daño_lanza;
						if (vida_valdivia_final <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion lanza");
						break;
					case "Maza":
						vida_valdivia_final = vida_valdivia_final - daño_maza;
						if (vida_valdivia_final <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					Protagonista_comp.ataque = false;
					tiempo_reg_valdivia = 5f;
					sonido (1);
					efecto (1);
				}
			}
		}
	}
	void atacar()
	{
		anim.SetTrigger ("Ataque espada valdivia");
	}
	// El animador llama a estos metodos y define si el ataque esta activo o no
	public void Comienzo_anim (){
		if(vida_valdivia_final > 0)
			ataque_valdivia = true;
	}
	public void Termino_anim(){
		ataque_valdivia = false;
	}

	void morir(){
		sonido (3);
		prefab (1);
		ataque_valdivia = false;
		anim.SetBool ("Atacando", false);
		anim.SetBool ("Muerto", true);
		anim.SetTrigger ("Muerte");
		Destroy (GetComponent<CapsuleCollider> ());
		GetComponent<NavMeshAgent> ().radius = 0.0f;
		GetComponent<NavMeshAgent> ().height = 0.0f;
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
		case 3:
			fuente.Stop ();
			fuente.PlayOneShot(muerte);
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
	void prefab(int prefab)
	{
		if (violencia) {
			switch (prefab) {
			case 1:
				float alt = Terrain.activeTerrain.SampleHeight (transform.position) + 0.01f; // la mancha aparece segun la altura del terreno
				GameObject mancha = Instantiate (mancha_sangre);
				mancha.transform.position = new Vector3 (transform.position.x, alt, transform.position.z); // No funciona siempre
				mancha.transform.rotation = transform.rotation;
				break;
			}
		}
	}
}