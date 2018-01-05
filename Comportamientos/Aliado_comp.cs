using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Aliado_comp : MonoBehaviour {
	
	public bool ataque_aliado = false;
	public float vida_aliado;
	public Transform prota; 			// Protagonista, se ingresa por el editor de Unity

	public float dist_ataque;
	public float dist_caminar;
	public float dist_persec; 			// Distancia para perseguir
	public Transform objetivo;
	private GameObject[] lista_tropa_enemiga;
	private Animator anim;
	private float doble_click;			// Para evitar que el doble click

	private bool comando_det; 
	private bool comando_per; 
	private bool festejo;
	private bool per_prota;

	// Daños por arma
	private float daño_espada = 15 +3;
	private float daño_pica = 10 +3;
	private float daño_mandoble = 20 +3;
	private float daño_hacha = 25 +3;

	// Sonidos
	public AudioSource fuente;
	public AudioClip quejido1, quejido2, quejido3, quejido4;
	public AudioClip golpe1, golpe2, golpe3, golpe4;
	public AudioClip muerte;
	public AudioClip festejo_clip;
	private float festejo_time= 0f;
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
		lista_tropa_enemiga = GameObject.FindGameObjectsWithTag("Enemigo");

		dificultad = Variables_globales.dificultad;
		violencia = Variables_globales.violencia;

		//¿Qué tropa esta usando este script?			true = maza, false = lanza
		if (gameObject.tag == "Aliado maza")
			anim.SetBool("Tipo tropa", true);
		else if (gameObject.tag == "Aliado lanza")
			anim.SetBool("Tipo tropa", false);
			
		//ordena las variables de movimiento, ataque y  la inicializacion de la vida aleatoria
		if (anim.GetBool("Tipo tropa")) {				// true = maza, false= lanza
			vida_aliado = Random.Range (100f, 110f);
			dist_ataque = 1.5f;
			dist_caminar = 5f;
		} 
		else{
			vida_aliado = Random.Range (90f, 100f);
			dist_ataque = 2.7f;
			dist_caminar = 7f;
		}
		// La dificultad multiplica la vida
		/*switch (dificultad) {
		case "facil":
			vida_aliado = vida_aliado * 1.1f;
			break;
		case "normal":
			vida_aliado = vida_aliado * 1.0f;
			break;
		case "dificil":
			vida_aliado = vida_aliado * 0.9f;
			break;
		}*/
	}

	// Update is called once per frame
	void Update () {
        // Si no esta muerto, perseguir
        if (vida_aliado > 0) {
			// Persigue
			perseguir();

			// Correr / Caminar
			if (Vector3.Distance (objetivo.position, this.transform.position) > dist_caminar) {
				anim.SetBool ("Correr", true);
				paso -= Time.deltaTime;
				if(paso <= 0 && !anim.GetBool ("Quieto"))
					sonido (2);
			} else {
				anim.SetBool ("Quieto", false);	// Si el enemigo esta, igual lo persigue
				anim.SetBool ("Correr", false);
			}

			// Si lo dañan mientras ataca, el ataque es false
			if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Reaccion"))
				ataque_aliado = false;

			// Distancia para atacar, si esta persiguiendo al protagonista, se queda quieto en vez de atacar
			if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque && objetivo != prota) { //ataca si el objetivo no es el prota
				anim.SetBool ("Atacando", true);
				if (!ataque_aliado && vida_aliado > 0)
					atacar ();
			}
			else
			{
				anim.SetBool ("Atacando", false); //si el objetivo es el prota y se acerca, se queda quieto
				if (objetivo == prota) {
					if (Vector3.Distance (objetivo.position, this.transform.position) <= dist_ataque + 2) {
						if (!festejo) {
							if (per_prota)
								anim.SetBool ("Quieto", true);
							else
								anim.SetBool ("Festejo", true);
						}
						else
							anim.SetBool ("Festejo", true);
					} else
						anim.SetBool ("Festejo", false);
				}
			}
			if (!objetivo.GetComponentInParent<CapsuleCollider> ())	//si el ultimo objetivo de la lista ya esta muerto, persigue al protagonista
				objetivo = prota;

			// Grito en el festejo
			if (anim.GetBool ("Festejo")) {
				festejo_time -= Time.deltaTime;
				if (festejo_time <= 0)
					sonido (4);
			}
			// Comandos
			if (doble_click <= 0.0f) {
				if (Input.GetButtonDown ("Detener tropas")) {
					doble_click = 0.4f;
					if (comando_det)
						comando_det = false;
					else {
						comando_det = true;
						comando_per = false;
						per_prota = false;
					}
					detener_tropas ();
					acercar_tropas ();
				}
				if (Input.GetButtonDown ("Acercar tropas")) {
					doble_click = 0.4f;
					if (comando_per) {
						comando_per = false;
						per_prota = false;
					}
					else {
						comando_per = true;
						comando_det = false;
					}
					detener_tropas ();
					acercar_tropas ();
				}
				if (!comando_per)
					per_prota = false;
			}
			// Si el comando de persecución al prota esta activo, recibe 50% menos de daño pero no ataca
			if (per_prota) {
				daño_espada = (15 + 3) * 0.5f;
				daño_pica = (10 + 3) * 0.5f;
				daño_mandoble = (20 + 3) * 0.5f;
				daño_hacha = (25 + 3) * 0.5f;
			}
		}
		doble_click -= Time.deltaTime;

		//print (ataque_aliado);
	}
	void detener_tropas()
	{
		float aleatorio = Random.Range (0, 100);
		if (!anim.GetBool ("Quieto") && comando_det)
		if (aleatorio <= 80) 						// Las tropas tienen un 80% de hacerte caso
			anim.SetBool ("Quieto", true);
		if(!comando_det)
			anim.SetBool ("Quieto", false);
	}
	void acercar_tropas()
	{
		float aleatorio = Random.Range (0, 100);
		if (aleatorio <= 20) { 						// Las tropas tienen un 20% de hacerte caso
			objetivo = prota;
			per_prota = true;
		}
		else
			per_prota = false;
	}
	// Perseguir
	void perseguir()
	{
		if (!per_prota) {
			float dist_cerca = Mathf.Infinity; //distancia del objeto mas cercano
			foreach (GameObject tropa_enemiga in lista_tropa_enemiga) {
				if (tropa_enemiga.GetComponentInParent<CapsuleCollider> ()) {//¿el objetivo tiene un collider?
					Vector3 diff = tropa_enemiga.transform.position - this.transform.position; // Unity doc
					float curDistance = diff.sqrMagnitude;
					if (curDistance < dist_cerca) {
						dist_cerca = curDistance;

						objetivo = tropa_enemiga.transform; //el objetivo es la tropa mas cercana encontrada
						dist_persec = 70f;
					}
				} else
					continue;
			}
		}
		if (objetivo != null) {
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
					anim.SetBool ("Correr", true);
					Vector3 direction = prota.position - this.transform.position;
					direction.y = 0;
					this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.2f);
				} else {
					// si no, persigue aleatoriamente, para no salir corriendo por ahi
					Vector3 aleatorio = new Vector3 (Random.Range (-1f, 1f), 0, Random.Range (-1f, 1f));
					this.transform.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (aleatorio), 0.1f);
					anim.SetBool ("Correr", false);
				}
			}
		} else {
			objetivo = prota; // Si no hay a quien perseguir, persigue al protagonista
			festejo = true;
		}
	}
	// Daño por arma
	void OnTriggerEnter(Collider col)
	{
		if (vida_aliado > 0 && col.tag != "Terreno" && col.transform.root != transform) {
			// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
			if (col.tag == "Arma enemiga") {	//daño por enemigo
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemigo_comp> ().ataque_enemigo) {
					switch (col.transform.name) {
					case "Espada enemigo":
						vida_aliado = vida_aliado - (daño_espada);
						if (vida_aliado<= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion espada");
						break;
					case "Pica":
						vida_aliado = vida_aliado - (daño_pica);
						if (vida_aliado<= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion pica");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemigo_comp> ().ataque_enemigo = false;
					// Si un enemigo golpea a un aliado o viceversa, le da +2 de vida
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemigo_comp> ().vida_enemigo += 1;
					sonido (1);
					efecto (1);
				}
			} else if (col.tag == "Arma jefe") {	//daño por jefe
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Jefe_comp> ().ataque_jefe) {
					switch (col.transform.name) {
					case "Mandoble":
						vida_aliado = vida_aliado - (daño_mandoble);
						if (vida_aliado<= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion espada");
						break;
					case "Hacha":
						vida_aliado = vida_aliado - (daño_hacha);
						if (vida_aliado<= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion espada");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Jefe_comp> ().ataque_jefe = false;
					sonido (1);
					efecto (1);
				}
			}else if (col.tag == "Arma valdivia") {	//daño por valdivia
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_comp> ().ataque_valdivia) {
					switch (col.transform.name) {
					case "Espada valdivia":
						vida_aliado = vida_aliado - (daño_mandoble);
						if (vida_aliado<= 0)
							morir ();
						else
							anim.SetTrigger ("Reaccion espada");
						break;
					}
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_comp> ().ataque_valdivia = false;
					sonido (1);
					efecto (1);
				}
			}
			//ataque_aliado = false; // Si recibe daño mientras ataca, el ataque se vuelve false sin esperar al anim
		}
	}
	void atacar()
	{
		if (anim.GetBool ("Tipo tropa")) { // true = maza, false= lanza
			anim.SetTrigger ("Ataque maza");
		}
		else {
			anim.SetTrigger ("Ataque lanza");
		}
	}
	// El animador llama a estos metodos y define si el ataque esta activo o no
	public void Comienzo_anim (){
		if(vida_aliado > 0)
			ataque_aliado = true;
	}
	public void Termino_anim(){
		ataque_aliado = false;
	}

	void morir(){
		sonido (3);
		prefab (1);
		ataque_aliado = false;
		anim.SetBool ("Atacando", false);
		anim.SetBool ("Muerto", true);
		anim.SetTrigger ("Muerte");
		Destroy (GetComponent<CapsuleCollider> ());		
		GetComponent<NavMeshAgent> ().radius = 0.0f;
		GetComponent<NavMeshAgent> ().height = 0.0f;
		GetComponent<NavMeshAgent> ().baseOffset = 0.01f;
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
		case 4:
			festejo_time = 2.5f;
			fuente.Stop ();
			fuente.PlayOneShot(festejo_clip);
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
				mancha.transform.position = new Vector3 (transform.position.x, alt, transform.position.z + 1.0f); // No funciona siempre
				mancha.transform.rotation = transform.rotation;
				break;
			}
		}
	}
}