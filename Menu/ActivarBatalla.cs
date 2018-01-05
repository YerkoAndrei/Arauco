using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarBatalla : MonoBehaviour {
    [SerializeField]
    private AudioClip batallaP, batallaM;
    [SerializeField]
    private AudioSource sonidoBatalla;

    private GameObject[] enemigos, aliados;


    private void OnTriggerEnter(Collider other)
    {
        enemigos = GameObject.FindGameObjectsWithTag("Enemigo");
        aliados = GameObject.FindGameObjectsWithTag("Aliado");
        /*
        if (enemigos.Length < 2 | aliados.Length < 2)
        {
            if (sonidoBatalla.isPlaying)
                sonidoBatalla.Stop();

            sonidoBatalla.PlayOneShot(batallaP);
            Debug.Log("sonido de batalla pequeña");
        }
        else if (enemigos.Length > 2 | aliados.Length > 2)
        {
             
            sonidoBatalla.PlayOneShot(batallaM);
            Debug.Log("sonido de batalla mediana");
        }
        else*/ if (enemigos.Length == 0 || aliados.Length == 0)
        {
            sonidoBatalla.mute = true;
        }
        enemigos = new GameObject[0];
        aliados = new GameObject[0];
    }
}
