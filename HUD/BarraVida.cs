using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour {
    public float fillAmout; //monto pa llenar barrita
    public Image vida;

    private float vidaInicial, vidaActual;

    // Use this for initialization
    //public float Val { set { fillAmout=Map( vidaInicial = Protagonista_comp.vida_max, 0, vidaInicial, 0, 1);} }
    void Start () {
        vidaInicial = Protagonista_comp.vida_max;
	}

    // Update is called once per frame
    void Update() {
        vidaActual = Protagonista_comp.vida ;
        if (vidaActual != vida.fillAmount)
        {  
        	HandleBar();
        }
    }
    private void HandleBar()
    {
        vida.fillAmount = Map(vidaActual, 0, vidaInicial, 0, 1);// fillAmout;// 
    }
    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
