using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemigo_comp : MonoBehaviour {

	public bool ataque_enemigo = false;
	public float vida_enemigo;
	public Transform prota;

	public float dist_ataque;
	public float dist_caminar;
	public float dist_persec; 				// Distancia para perseguir
	public Transform objetivo;
	private GameObject[] lista_tropa_aliada;
	private Animator anim;

	// Daños por arma
	private float daño_espada = 15;
	private float daño_lanza = 10;
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
		anim = GetComponent<Animator> ();
		lista_tropa_aliada = GameObject.FindGameObjectsWithTag("Aliado");

		dificultad = Variables_globales.dificultad;
		violencia = Variables_globales.violencia;

		// ¿Qué tropa esta usando este script?           true = espada, false = pica
		if (gameObject.tag == "Enemigo espada")
			anim.SetBool("Tipo tropa", true);
		else if (gameObject.tag == "Enemigo pica")
			anim.SetBool("Tipo tropa", false);
			
		// Ordena las variables de movimiento, ataque y  la inicializacion de la vida aleatoria
		if (anim.GetBool("Tipo tropa")) {//true = espada, false = pica
			vida_enemigo = Random.Range (110f, 130f);
			dist_ataque = 1.9f;
			dist_caminar = 6f;
		} 
		else {
			vida_enemigo = Random.Range (100f, 120f);
			dist_ataque = 3.1f;
			dist_caminar = 8f;
		}
		// La dificultad multiplica la vida
		switch (dificultad) {
		case "facil":
			vida_enemigo = vida_enemigo - 99;	// Ultra fácil para presentación rapida en la tesis
			break;
		case "normal":
			vida_enemigo = vida_enemigo;
			break;
		case "dificil":
			vida_enemigo = vida_enemigo + 10;
			break;
		}
	}

	// Update is called once per frame
	void Update () {
		// Si no esta muerto, perseguir
		if (vida_enemigo > 0) {
			// Persigue
			perseguir ();

			// Si lo dañan mientras ataca, el ataque es false
			if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Reaccion"))
				ataque_enemigo = false;

			// Correr / Caminar
			if (Vector3.Distance (objetivo.position, this.transform.position) > dist_caminar) {
				anim.SetBool ("Correr", true);
				paso -= Time.deltaTime;
				if(paso <= 0 && !anim.GetBool ("Quieto"))
					sonido (2);
			} else
				anim.SetBool ("Correr", false);
			
			// Distancia para atacar
			if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque) {
				anim.SetBool ("Atacando", true);
				if (!ataque_enemigo && vida_enemigo > 0)
					atacar ();
			} else
				anim.SetBool ("Atacando", false); //si esta atacando dejará de avanzar y se quedará quieto
		}
		//print (vida_enemigo);
	}
	// Perseguir
	void perseguir()
	{
		//¿Está el protagonista cerca?
		if (Vector3.Distance (prota.position, this.transform.position) < (dist_ataque + 0.5f)) { // el enemigo enfoca al protagonista segun su arma
			objetivo = prota;
			dist_persec = 6f;
		} else { //entonces busca la tropa mas cercana
			float dist_cerca = Mathf.Infinity; //distancia del objeto mas cercano
			foreach (GameObject tropa_aliada in lista_tropa_aliada) {
				if (tropa_aliada.GetComponentInParent<CapsuleCollider> ()) {//¿El objetivo tiene un collider?
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
			this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.2f); //0.2f es la velocidad con la que se voltea
			if (direction.magnitude > 70f) {
				this.transform.Translate (0, 0, 0.1f);
			}
		} else {
			// Si el prota esta a menos de 70 unidades de distancia, lo persigue
			if (Vector3.Distance (prota.position, this.transform.position) < 70f) {
				objetivo = prota;
				dist_persec = 6f;
				Vector3 direction = prota.position - this.transform.position;
				direction.y = 0;
				this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.2f);
			} else {
				// si no, persigue aleatoriamente, para no salir corriendo por ahi
				Vector3 aleatorio = new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f));
				this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (aleatorio), 0.1f);
			}

		}
	}
	// Daño por arma
	void OnTriggerEnter(Collider col)
	{
		if (vida_enemigo > 0 && col.tag != "Terreno" && col.transform.root != transform) {
			if (col.tag == "Arma") {	//si el daño lo hace el prota
				if (Protagonista_comp.ataque) {
					switch (col.transform.name) {
					case "Espada":
						vida_enemigo = vida_enemigo - (daño_espada);
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					case "Lanza":
						vida_enemigo = vida_enemigo - (daño_lanza);
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion lanza");
						break;
					case "Maza":
						vida_enemigo = vida_enemigo - (daño_maza);
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					Protagonista_comp.ataque = false;
					sonido(1);
					efecto (1);
				}
			} else if (col.tag == "Arma aliado") {	//si el daño lo hace un aliado
				// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Aliado_comp> ().ataque_aliado) {
					switch (col.transform.name) {
					case "Lanza":
						vida_enemigo = vida_enemigo - (daño_lanza -4); //las tropas le pegan menos que el prota
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion lanza");
						break;
					case "Maza":
						vida_enemigo = vida_enemigo - (daño_maza -4);
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Aliado_comp> ().ataque_aliado = false;
					// Si un enemigo golpea a un aliado o viceversa, le da +2 de vida
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Aliado_comp> ().vida_aliado += 1;
					sonido(1);
					efecto (1);
				}
			}else if (col.tag == "Arma caupolican") {	//si el daño lo hace caupolican
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Caupolican_comp> ().ataque_caupo) {
					switch (col.transform.name) {
					case "Maza caupolican":
						vida_enemigo = vida_enemigo - (daño_maza +4);
						if (vida_enemigo <= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion maza");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Caupolican_comp> ().ataque_caupo = false;
					sonido(1);
					efecto (1);
				}
			}
			//ataque_enemigo = false; // Si recibe daño mientras ataca, el ataque se vuelve false sin esperar al anim
		}
	}
	void atacar()
	{
		if (anim.GetBool ("Tipo tropa")) {    //true = espada, false = pica
			anim.SetTrigger ("Ataque espada");
		} else {
			anim.SetTrigger ("Ataque pica");
		}
	}
	// El animador llama a estos metodos y define si el ataque esta activo o no
	public void Comienzo_anim (){
		if(vida_enemigo > 0)
			ataque_enemigo = true;
	}
	public void Termino_anim(){
		ataque_enemigo = false;
	}

	void morir(){
		sonido (3);
		prefab (1);
		ataque_enemigo = false;
		anim.SetBool ("Atacando", false);
		anim.SetBool ("Muerto", true);
		anim.SetTrigger ("Muerte");
		Destroy (GetComponent<CapsuleCollider> ());
		GetComponent<NavMeshAgent> ().radius = 0.0f;
		GetComponent<NavMeshAgent> ().height = 0.0f;
		GetComponent<NavMeshAgent> ().baseOffset = -0.01f;
		if (!violencia){
			Transform child = transform.GetChild (0);
			foreach (Transform tra in child)
				Destroy (tra.GetComponent<SkinnedMeshRenderer>());
		}
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
				mancha.transform.position = new Vector3 (transform.position.x , alt, transform.position.z -0.5f);
				mancha.transform.rotation = transform.rotation;
				break;
			}
		}
    }
}