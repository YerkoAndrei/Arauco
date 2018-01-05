using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aleatorio_anim : MonoBehaviour {
	private float aleatorio;
	private float aleatorio2;
	private Transform prota;
	private Animator anim;

	private bool aliado;
	private bool parar;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		aleatorio = Random.Range (0, 6);
		aleatorio2 = Random.Range (0, 100);
		if (gameObject.tag == "Aliado lanza" || gameObject.tag == "Aliado maza" || gameObject.tag == "Caupolican") {
			prota = GameObject.Find ("Protagonista").transform;
			aliado = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		// En la batalla final miran al prota
		if (aliado) {
			transform.LookAt (prota.position);
		}
		// Delay en las animaciones
		aleatorio -= Time.deltaTime;
		if (aleatorio <= 0 && !parar)
			set_anim ();
	}
	void set_anim()
	{
		parar = true;
		if(aleatorio2 <= 50)
			anim.SetBool ("Mov_1", true);
		else
			anim.SetBool ("Mov_2", true);
	}
}
