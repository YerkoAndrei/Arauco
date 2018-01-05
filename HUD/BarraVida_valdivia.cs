using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida_valdivia : MonoBehaviour {
    public float fillAmout; //monto pa llenar barrita
    public Image vida;

    private float vidaInicial, vidaActual;

    // Use this for initialization
    //public float Val { set { fillAmout=Map( vidaInicial = Protagonista_comp.vida_max, 0, vidaInicial, 0, 1);} }
    void Start () {
		//vidaInicial = Valdivia_final_comp.vida_max_valdivia_final;
		switch (Variables_globales.dificultad) {
		case "facil":
			vidaInicial = 50;
			break;
		case "normal":
			vidaInicial = 600;
			break;
		case "dificil":
			vidaInicial = 700;
			break;
		}
	}

    // Update is called once per frame
    void Update() {
		vidaActual = Valdivia_final_comp.vida_valdivia_final;
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
