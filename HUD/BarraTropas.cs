using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraTropas : MonoBehaviour {
	public float fillAmout;

    public Image img_enemigo, img_aliado;

	private GameObject[] lista_tropa_enemiga;
	private GameObject[] lista_tropa_aliada;

	private int total_aliado;
	private int total_enemigo;
	private int total;

	private float formula_barra_tropa; // Fórmula para la barra de tropas. El cálculo se hace solo en la barra de las tropas enemigas

    // Use this for initialization
    void Start () {
		lista_tropa_enemiga = GameObject.FindGameObjectsWithTag("Enemigo");
		lista_tropa_aliada = GameObject.FindGameObjectsWithTag("Aliado");

		total_aliado = lista_tropa_aliada.Length;
		total_enemigo = lista_tropa_enemiga.Length;
		total = total_aliado + total_enemigo;
    }
	
	// Update is called once per frame
	void Update () {
		int e = 0;
		foreach (GameObject tropa in lista_tropa_enemiga) {
			if (!tropa.GetComponentInParent<CapsuleCollider> ()) {
				e--;
			}
		}
		int a = 0;
		foreach (GameObject tropa in lista_tropa_aliada) {
			if (!tropa.GetComponentInParent<CapsuleCollider> ()) {
				a--;
			}
		}

		formula_barra_tropa = (((total_enemigo + e) * 100) / (total + (e + a))) * 0.01f;
		img_enemigo.fillAmount = formula_barra_tropa;
    }
}
