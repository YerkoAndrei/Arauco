using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Pantalla_carga : MonoBehaviour {
	public static string salida;
	public static string destino;
    public string idiomaSeleccionado = "espanol";
	public static  string escena_actual;
	public static  bool activo = false;
    public Text conEsp;
	public Sprite img;
	public Sprite img0, img1, img2, img3, img4, img5, img6, img7, img8, img9, img10, img11, img12, img13;

	public static bool vid = false;

	// Use this for initialization
	void Start () {
		if (destino == null)
			destino = "Menu_principal";
		
		idiomaSeleccionado = Variables_globales.idioma;

		if (salida == "Escoger_idioma" || salida == "Menu_principal" || salida=="Intro") 
			GetComponentInChildren<Image> ().sprite = img0;
		else if (salida == "Video_final")
			GetComponentInChildren<Image> ().sprite = img12;
		else if (destino == "Tutorial" || salida == "Tutorial")
			GetComponentInChildren<Image> ().sprite = img13;
		else 
			GetComponentInChildren<Image> ().sprite = img_aleatoria ();
		
		GetComponentInChildren<Image> ().SetNativeSize();

		// Reproducción de los videos
		if (salida != "Intro" || salida != "Video_tuca_01" || salida != "Video_tuca_final" || salida != "Video_final" || salida != "Video_tuto") {
			if (salida == "Escoger_idioma" && destino == "Menu_principal") {
				vid = true;
				SceneManager.LoadSceneAsync ("Intro");
			}
			if (salida == "Menu_principal" && destino == "Tucapel_01") {
				vid = true;
				SceneManager.LoadSceneAsync ("Video_tuca_01");
			}
			if (salida == "Tucapel_04" && destino == "Tucapel_final") {
				vid = true;
				SceneManager.LoadSceneAsync ("Video_tuca_final");
			}
			if (salida == "Tucapel_final" && destino == "Menu_principal") {
				vid = true;
				SceneManager.LoadSceneAsync ("Video_final");
			}
			if (salida == "Menu_principal" && destino == "Tutorial") {
				vid = true;
				SceneManager.LoadSceneAsync ("Video_tuto");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (vid)
			if (salida == "Intro" || salida == "Video_tuca_01" || salida == "Video_tuca_final" || salida == "Video_final" || salida == "Video_tuto")
				vid = false;

		if (activo && SceneManager.GetActiveScene ().name == "Pantalla_carga") {
			activo = false;
			if(!vid)
				SceneManager.LoadSceneAsync (destino);
		}
	}

	public static void cargar_escena(string s, string d)
	{
		SceneManager.LoadScene ("Pantalla_carga");
		activo = true;
		salida = s;
		destino = d;
	}

	Sprite img_aleatoria(){
		float aleatorio = Random.Range (0, 120);
		if (aleatorio >= 0 && aleatorio < 10) {
			texto (1);
			return img1;
		}
		if (aleatorio >= 10 && aleatorio < 20) {
			texto (2);
			return img2;
		}
		if (aleatorio >= 20 && aleatorio < 30) {
			texto (3);
			return img3;
		}
		if (aleatorio >= 30 && aleatorio < 40) {
			texto (4);
			return img4;
		}
        if (aleatorio >= 40 && aleatorio < 50){
			texto (5);
			return img5;
        }
		if (aleatorio >= 50 && aleatorio < 60) {
			texto (6);
			return img6;
		}
		if (aleatorio >= 60 && aleatorio < 70) {
			texto (7);
			return img7;
		}
		if (aleatorio >= 70 && aleatorio < 80) {
			texto (8);
			return img8;
		}
		if (aleatorio >= 80 && aleatorio < 90) {
			texto (9);
			return img9;
		}
		if (aleatorio >= 90 && aleatorio < 100) {
			texto (10);
			return img10;
		}
		if (aleatorio >= 100 && aleatorio < 110) {
			texto (11);
			return img11;
		}
		if (aleatorio >= 110 && aleatorio < 120) {
			texto (12);
			return img12;
		} else {
			texto (13);
			return img0;
		}
	}
	void texto(int texto)
    {
		switch (texto) {
            case 1:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
	                conEsp.text = "Los jefes enemigos pueden curarse igual que tú, dáñalos constantemente.";
	            }else{
	                conEsp.text = "Enemy bosses can heal just like you, hurt them constantly.";
	            }
                break;
            case 2:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
	                conEsp.text = "Cada arma bloquea una distinta cantidad de daño, utilízalo a tu favor.";
	            }
	            else{
	                conEsp.text = "Each weapon blocks a different amount of damage, use this to your advantage.";
	            }
                break;
            case 3:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null)
	                 {
	                  conEsp.text = "No puedes ganar solo, lucha junto a tus tropas.";
				}else{
	                  conEsp.text = "You can not win alone, fight with your troops.";
	            }
                break;
            case 4:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
	                conEsp.text = "Lucha junto a tus tropas y ayudalos a ganar batallas por escuadrón.";
	            }
	            else{
					conEsp.text = "Fight with your troops and help them win battles by squadron.";
	            }
                break;
            case 5: 
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
	                conEsp.text = "La maza tiene un daño mucho mayor, utilízala cuando estés seguro.";
	            }else{
	                conEsp.text = "The club has much greater damage, use it when you are safe.";
	            }
                break;
            case 6:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
					conEsp.text = "La lanza tiene mayor alcance que las demás armas, utilízala cuanto tengas poca vida.";
                }else{
                    conEsp.text = "The spear has more range than other weapons, use it when you have low health.";
                }
                break;
            case 7:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
                    conEsp.text = "Posicionar bien a tus tropas puede significar la victoria, lucha junto a ellos.";
                }else{
                    conEsp.text = "Positioning your toops can mean victory, fight whit them.";
                }
                break;
			case 8:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null) {
					conEsp.text = "Cada tropa tiene distinta armadura y daño, aprende a usarlas correctamente.";
				}else{
					conEsp.text = "Each troop has different armor and damage, learn to use correctly.";
				}
				break;
            case 9:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
					conEsp.text = "Cada tropa tiene distinta armadura y daño, aprende a enfrentarlas correctamente.";
                } else{
					conEsp.text = "Each troop has different armor and damage, learn to face them correctly.";
                }
			break;
            case 10:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
                    conEsp.text = "Tú eres el lider, la victoria depende de ti.";
                }else{
                    conEsp.text = "You are the leader, victory depends on you.";
                }
                break;
            case 11:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
                    conEsp.text = "Utiliza los comandos para posicionar a tus tropas, pon atención al índice de rebeldía.";
                }else{
                    conEsp.text = "Use the commands to position your troops, pay attention to the rebelliousness index.";
                }
                break;
            case 12:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
                    conEsp.text = "En la guerra, la victoria lo es todo.";
                }else{
                    conEsp.text = "In war, victory is all.";
                }
                break;
            case 13:
				if (idiomaSeleccionado == "espanol" || idiomaSeleccionado == "mapu" || idiomaSeleccionado == null){
                    conEsp.text = "Cargando...";
                }else{
                    conEsp.text = "Loading...";
                }
                break;
        }
     }      
}
