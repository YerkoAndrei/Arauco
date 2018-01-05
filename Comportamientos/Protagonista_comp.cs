using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Protagonista_comp : MonoBehaviour {

	public static float vida;
	public static float vida_max = 250f;
	public float vel_cura = 10f;	// Velocidad de regeneracion
	public float tiempo_reg = 3f;		// Tiempo antes de regenerarse

    // Sonidos
	public AudioSource fuente;
	public AudioClip espada, lanza, maza;
	public AudioClip golpe_directo1, golpe_directo2, golpe_directo3, golpe_directo4;
	public AudioClip golpe_defensa1, golpe_defensa2, golpe_defensa3, golpe_defensa4;
	public AudioClip cambio_arma, grito;
	public AudioClip muerte;

	// Efectos
	public ParticleSystem sangre;	// Los particle systems son publicos y deben ingresarse por el editor de unity. Estan dentro del protagonista
	public GameObject mancha_sangre;

	public static bool camara = true; // Para el cambio de camara, si esta cerca del prota es true
	private Animator anim;

	public static int arma = 1;
	public static bool ataque = false;		// ¿Estoy atacando? if true, hace daño. Se activa en las animaciones
	private float doble_click;				// para evitar que el doble click

	// Daños por arma
	private float daño_espada = 15;
	private float daño_pica = 10;
	private float daño_mandoble = 20;
	private float daño_hacha = 25;

	// Globales
	private string dificultad = "normal";	// Por defecto
	private bool violencia = true;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();

		dificultad = Variables_globales.dificultad;
		violencia = Variables_globales.violencia;

		//Arma predeterminada. Actualmente es la espada.
		arma = 1;
		anim.SetBool ("Una mano", true);

		// La dificultad multiplica la vida
		/*switch (dificultad) {
		case "facil":
			vida_max = vida_max * 1.1f;
			break;
		case "normal":
			vida_max = vida_max * 1.0f;
			break;
		case "dificil":
			vida_max = vida_max * 0.9f;
			break;
		}*/
		vida = vida_max;
	}

	// Update is called once per frame
	void Update () {        
		// Morir o regenerar vida
		if (vida < vida_max && vida > 0) {
			tiempo_reg -= Time.deltaTime;	// Comienza a regenerar vida despues del tiempo asignado
			if (tiempo_reg <= 0.0f)
				vida += vel_cura * Time.deltaTime;
		}		
		// Cambiar arma
		if (!anim.GetCurrentAnimatorStateInfo (0).IsTag ("Interrupt")) // Solo puede cambiar arma si no está atacando
		{
			if (Input.GetButtonDown ("Cambiar espada") && arma !=1) {
				anim.SetTrigger ("Cambio arma");
				arma = 1;
				anim.SetBool ("Una mano", true);
				sonido (4);
			}
			else if (Input.GetButtonDown ("Cambiar lanza") && arma !=2) {
				anim.SetTrigger ("Cambio arma");
				arma = 2;
				anim.SetBool ("Una mano", false);
				sonido (4);
			}
			else if (Input.GetButtonDown ("Cambiar martillo") && arma !=3) {
				anim.SetTrigger ("Cambio arma");
				arma = 3;
				anim.SetBool ("Una mano", false);
				sonido (4);
			}
		}

		// Defensa
		if (Input.GetButtonDown ("Defensa"))
			if(ThirdPersonCharacter.m_IsGrounded && !ThirdPersonCharacter.m_Crouching)
				anim.SetBool ("Defendiendo", true);
		
		if (Input.GetButtonUp ("Defensa"))
			anim.SetBool ("Defendiendo", false);

		// Si lo dañan mientras ataca, el ataque es false
		if (anim.GetCurrentAnimatorStateInfo (0).IsTag ("Reaccion"))
			ataque = false;
			
		// Cambio de la distancia de la camara
		if (Input.GetButtonDown ("Cambio camara")) {
			Vector3 cam;
			float min_inf;
			if (camara) {
				min_inf = -10f;
				cam = new Vector3(0.0f, 0.6f, -15.0f); //valor para cambiar la distancia del personaje
				ThirdPersonOrbitCam.Cambiar_camara_offset(cam, min_inf);
				camara = false;
			} else {
				min_inf = 18f;
				cam = new Vector3(0.1f, 0.6f, -2.8f); // falta cambiar con el mouseroll
				ThirdPersonOrbitCam.Cambiar_camara_offset(cam, min_inf); 
				camara = true;
			}
		}
		if (!anim.GetCurrentAnimatorStateInfo (0).IsTag ("Interrupt") && doble_click <= 0.0f) {	// Evita un ataque doble
			// Comandos tropas
			if (Input.GetButtonDown ("Detener tropas") || Input.GetButtonDown ("Acercar tropas")) {
				doble_click = 0.4f;
				anim.SetTrigger ("Llamado");
				sonido (5);
			}

			// Ataque
			if (!anim.GetCurrentAnimatorStateInfo (0).IsTag ("Reaccion")) {
				if (Input.GetButtonDown ("Ataque")) {
					doble_click = 0.4f;
					if (!ataque && vida > 0)
						atacar ();
				}
			}
		}
		doble_click -= Time.deltaTime;

		//print (ataque);
	}

	// Recibir daño
	void OnTriggerEnter(Collider col)
	{
		if (vida > 0 && col.tag != "Terreno" && col.transform.root != transform)
		{
			// Encuentra el nombre del dueño del arma (no usé .root porque root es la tropa de 5 aliados), y luego convierte en false su ataque
			if (col.tag == "Arma enemiga") {
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemigo_comp> ().ataque_enemigo) {	//daño por enemigo comun
					switch (col.transform.name) {
					case "Espada enemigo":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_espada * 0.4f);	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_espada * 0.9f); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_espada * 0.7f);
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_espada);
							anim.SetTrigger ("Reaccion espada");
							sonido (7);
							efecto (1);
						}
						break;
					case "Pica":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_pica * 0.4f);	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_pica * 0.9f); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_pica * 0.7f);
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_pica);
							anim.SetTrigger ("Reaccion pica");
							sonido (7);
							efecto (1);
						}
						break;
					}
					tiempo_reg = 3f;
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Enemigo_comp> ().ataque_enemigo = false;

					if (vida <= 0)
						morir ();
				}
			} else if (col.tag == "Arma jefe") {		// Daño por jefe
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Jefe_comp> ().ataque_jefe) {
					switch (col.transform.name) {
					case "Mandoble":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_espada * 0.4f);	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_espada * 0.9f); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_espada * 0.7f);
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_mandoble);
							anim.SetTrigger ("Reaccion espada");
							sonido (7);
							efecto (1);
						}
						break;
					case "Hacha":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_espada * 0.4f);	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_espada * 0.9f); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_espada * 0.7f);
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_hacha);
							anim.SetTrigger ("Reaccion espada");
							sonido (7);
							efecto (1);
						}
						break;
					}
					tiempo_reg = 3f;
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Jefe_comp> ().ataque_jefe = false;

					if (vida <= 0)
						morir ();
				}
			} else if (col.tag == "Arma valdivia") {		// Daño por valdivia
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_comp> ().ataque_valdivia) {
					switch (col.transform.name) {
					case "Espada valdivia":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_espada * 0.4f);	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_espada * 0.9f); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_espada * 0.7f);
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_mandoble);
							anim.SetTrigger ("Reaccion espada");
							sonido (7);
							efecto (1);
						}
						break;
					}
					tiempo_reg = 3f;
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_comp> ().ataque_valdivia = false;

					if (vida <= 0)
						morir ();
				}
			} else if (col.tag == "Arma valdivia final") {		// Daño por valdivia en la batalla final
				if (col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_final_comp> ().ataque_valdivia) {
					switch (col.transform.name) {
					case "Espada valdivia":
						if (anim.GetBool ("Defendiendo")) {
							switch (arma) {
							case 1:
								vida = vida - (daño_espada * (0.4f +0.3f));	// si esta bloqueando con el escudo, recibe el 40% del daño
								anim.SetTrigger ("Reaccion suave");
								sonido (6);
								break;
							case 2:
								vida = vida - (daño_espada * (0.9f +0.1f)); // si esta bloqueando con la lanza, el daño se reduce al 90%
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							case 3:
								vida = vida - (daño_espada * (0.7f +0.2f));
								anim.SetTrigger ("Reaccion suave dos manos");
								sonido (6);
								break;
							}
						} else {
							vida = vida - (daño_mandoble + 2f);
							anim.SetTrigger ("Reaccion espada");
							sonido (7);
							efecto (1);
						}
						break;
					}
					tiempo_reg = 3f;
					col.gameObject.transform.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.parent.GetComponent<Valdivia_final_comp> ().ataque_valdivia = false;

					if (vida <= 0)
						morir ();
				}
			}
			//ataque = false; // Si resibe daño mientras ataca, el ataque se vuelve false sin esperar al anim
		}
	}
	void atacar()
	{		
		switch (arma) {
		case 1:
			anim.SetTrigger ("Ataque espada");
			sonido (1);
            break;
		case 2:
			anim.SetTrigger ("Ataque lanza");
			sonido (2);
			break;
		case 3:
			anim.SetTrigger ("Ataque martillo");
			sonido (3);
			break;
		}
	}
	// El animador llama a estos metodos  y define si el ataque esta activo o no
	public void Comienzo_anim (){
		ataque = true;
	}
	public void Termino_anim(){
		ataque = false;
	}

	void morir(){
		anim.SetTrigger ("Muerte");
		anim.SetBool ("Muerto", true);
		sonido (8);
		prefab (1);
	}

	void sonido(int son)
	{
		switch (son) {
		case 1:
			fuente.PlayOneShot(espada);
			break;
		case 2:
			fuente.PlayOneShot (lanza);
			break;
		case 3:
			fuente.PlayOneShot (maza);
			break;
		case 4:
			fuente.PlayOneShot (cambio_arma);
			break;
		case 5:
			fuente.PlayOneShot (grito);
			break;
		case 6:
			float aleatorio = Random.Range (0, 100);
			if(aleatorio >= 0 && aleatorio < 25)
				fuente.PlayOneShot(golpe_defensa1);
			else if (aleatorio >= 25 && aleatorio < 50)
				fuente.PlayOneShot(golpe_defensa2);
			else if (aleatorio >= 50 && aleatorio < 75)
				fuente.PlayOneShot(golpe_defensa3);
			else if (aleatorio >= 75 && aleatorio < 100)
				fuente.PlayOneShot(golpe_defensa4);
			break;
		case 7:
			float aleatorio2 = Random.Range (0, 100);
			if(aleatorio2 >= 0 && aleatorio2 < 25)
				fuente.PlayOneShot(golpe_directo1);
			else if (aleatorio2 >= 25 && aleatorio2 < 50)
				fuente.PlayOneShot(golpe_directo2);
			else if (aleatorio2 >= 50 && aleatorio2 < 75)
				fuente.PlayOneShot(golpe_directo3);
			else if (aleatorio2 >= 75 && aleatorio2 < 100)
				fuente.PlayOneShot(golpe_directo4);
			break;
		case 8:
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