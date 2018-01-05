using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DificultadHud : MonoBehaviour {
    
    [SerializeField]
    private GameObject norm, faci, difi;
    [SerializeField]
    private int d, a;
    [SerializeField]
    private Dropdown dificul;
    public int dificultadActual;
	// Use this for initialization
	void Start () {
        d = dificul.value;
        dificultadActual = d;
	}
	
	// Update is called once per frame
	void Update () {
        a = dificul.value;
        if (a != d)
        {
            
            hudCambio(a);
            d = a;
        }
	}
    void hudCambio(int d)
    {
        if (d == 0)
        {
            norm.SetActive(false);
            difi.SetActive(false);
            faci.SetActive(true);

        }
        if (d == 1)
        {
            norm.SetActive(true);
            difi.SetActive(false);
            faci.SetActive(false);

        }
        if (d ==2)
        {
            norm.SetActive(false);
            difi.SetActive(true);
            faci.SetActive(false);

        }


    }
}
