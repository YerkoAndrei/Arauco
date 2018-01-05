using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caminar : MonoBehaviour {

    [SerializeField]
    private AudioSource caminar;

    private void OnTriggerEnter(Collider paso)
    {
        if (paso.gameObject.tag == "Terreno")
        {
            caminar.Play();
        }
    }
}
