using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Valdivia_comp : MonoBehaviour {

	public bool ataque_valdivia = false;
	public float vida_valdivia;
	public float vida_max_valdivia;
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
	private float daño_espada = 15 -2;
	private float daño_lanza = 10 -2;
	private float daño_maza = 25 -2;

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
		vida_max_valdivia = 2000f;
		anim = GetComponent<Animator> ();

		dificultad = Variables_globales.dificultad;
		violencia = Variables_globales.violencia;

		lista_tropa_aliada = GameObject.FindGameObjectsWithTag("Aliado");
		dist_ataque = 1.6f;
		dist_caminar = 6f;

		// La dificultad multiplica la vida
		switch (dificultad) {
		case "facil":
			vida_max_valdivia = vida_max_valdivia - 1800;	// Ultra fácil para presentación rapida en la tesis
			break;
		case "normal":
			vida_max_valdivia = vida_max_valdivia;
			break;
		case "dificil":
			vida_max_valdivia = vida_max_valdivia + 50;
			break;
		}
		vida_valdivia = vida_max_valdivia;
	}

	// Update is called once per frame
	void Update () {
		// Si no esta muerto, perseguir
		if (vida_valdivia > 0)
		{
			if (vida_valdivia < vida_max_valdivia) {
				tiempo_reg_valdivia -= Time.deltaTime;	// Comienza a regenerar vida despues del tiempo asignado
				if (tiempo_reg_valdivia <= 0.0f)
					vida_valdivia += vel_cura_valdivia * Time.deltaTime;
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
			
			// Distancia para atacar
			if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque) {
				anim.SetBool ("Atacando", true);
				if (!ataque_valdivia && vida_valdivia > 0)
					atacar ();
			}
			else
				anim.SetBool ("Atacando", false); //si esta atacando dejará de avanzar y se quedará quieto		
		}
		//print (ataque_valdivia);
	}
	// Perseguir
	void perseguir()
	{
		//¿Está el protagonista cerca?
		if (Vector3.Distance (prota.position, this.transform.position) < (dist_ataque + 2f)) {
			objetivo = prota;
			dist_persec = 6f;
		}
		else //entonces busca la tropa mas cercana
		{
			float dist_cerca = Mathf.Infinity; //distancia del objeto mas cercano
			foreach (GameObject tropa_aliada in lista_tropa_aliada) 
			{
				if (tropa_aliada.GetComponentInParent<CapsuleCollider>()) {//¿el objetivo tiene un collider?
					Vector3 diff = tropa_aliada.transform.position - this.transform.position; //Unity doc
					float curDistance = diff.sqrMagnitude;
					if (curDistance < dist_cerca) {
						dist_cerca = curDistance;

						objetivo = tropa_aliada.transform; //el objetivo es la tropa mas cercana encontrada
						dist_persec = 70f;
					}
				} else {
					continue;
				}
			}
		}
		if (Vector3.Distance (objetivo.position, this.transform.position) < dist_persec) {//distancia para perseguir
			Vector3 direction = objetivo.position - this.transform.position;
			direction.y = 0;
			this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.5f);
			if (direction.magnitude > 70f) {
				this.transform.Translate (0, 0, 0.1f);
			}
		} 
		else {
			// Si el prota esta a menos de 70 unidades de distancia, lo persigue
			if (Vector3.Distance (prota.position, this.transform.position) < 70f) {
				objetivo = prota;
				dist_persec = 6f;
				Vector3 direction = prota.position - this.transform.position;
				direction.y = 0;
				this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.5f);
			}
			else
			{
				// si no, persigue aleatoriamente, para no salir corriendo por ahi
				Vector3 aleatorio = new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f));
				this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (aleatorio), 0.1f);
			}
		} 
	}
	// Daño por arma
	void OnTriggerEnter(Collider col)
	{
		if (vida_valdivia > 0 && col.tag != "Terreno" && col.transform.root != transform) {
			// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
			if (col.tag == "Arma") {
				if (Protagonista_comp.ataque) {
					switch (col.transform.name) {
					case "Espada":
						vida_valdivia = vida_valdivia - daño_espada;
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					case "Lanza":
						vida_valdivia = vida_valdivia - daño_lanza;
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion lanza");
						break;
					case "Maza":
						vida_valdivia = vida_valdivia - daño_maza;
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					Protagonista_comp.ataque = false;
					tiempo_reg_valdivia = 3f;
					sonido (1);
					efecto (1);
				}
			} else if (col.tag == "Arma aliado") {
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Aliado_comp> ().ataque_aliado) {
					switch (col.transform.name) {
					case "Lanza":
						vida_valdivia = vida_valdivia - (daño_lanza -4);
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion lanza");
						break;
					case "Maza":
						vida_valdivia = vida_valdivia - (daño_maza -4);
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Aliado_comp> ().ataque_aliado = false;
					tiempo_reg_valdivia = 3f;
					sonido (1);
					efecto (1);
				}
			}
			else if (col.tag == "Arma caupolican") {	//si el daño lo hace caupolican
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Caupolican_comp> ().ataque_caupo) {
					switch (col.transform.name) {
					case "Maza caupolican":
						vida_valdivia = vida_valdivia - (daño_maza +4);
						if (vida_valdivia <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Caupolican_comp> ().ataque_caupo = false;
					tiempo_reg_valdivia = 3f;
					sonido(1);
					efecto (1);
				}
			}
			//ataque_valdivia = false; // Si recibe daño mientras ataca, el ataque se vuelve false sin esperar al anim
		}
	}
	void atacar()
	{
		anim.SetTrigger ("Ataque espada valdivia");
	}
	// El animador llama a estos metodos y define si el ataque esta activo o no
	public void Comienzo_anim (){
		if(vida_valdivia > 0)
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
		GetComponent<NavMeshAgent> ().baseOffset = -0.01f;
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