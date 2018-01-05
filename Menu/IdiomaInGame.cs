using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class IdiomaInGame : MonoBehaviour
{
    private string idioma;
    //variable menu principal
    public Button btnContinuar, btnOpciones, btnGlosario,  btnSalir;
    public Text tituloPrincipal;

    //variables opciones
    public Button btnVolverOpciones, btnGeneral, btnGraficos, btnSonido;

    //Variables op generales
    public Button btnVolverGenerales, btnAplicarGenerales;
    public Dropdown dpDificultad, dpNivelViolencia, dpIdioma;
	public Text txtTituloOpcionesGenerales, txtDificultad, txtNivelViolencia, txtIdioma,txtSensibilidad, txtActivarPuntero, txtAdvertencia;


	//listas espanol
	List<string> dificultadEsp = new List<string>() { "Fácil", "Normal", "Difícil" };
	List<string> nivelViolenciaEsp = new List<string>() { "Activada", "Desactivada" };
	List<string> idiomaEsp = new List<string>() { "Español", "Inglés", "Mapudungun" };

	List<string> calidadTexturas = new List<string>() { "Alta", "Media", "Media Baja", "Baja" };
	List<string> sombras = new List<string>() { "Desactivar", "Sombreado fuerte", "Sombreado fuerte y suave" };
	List<string> resolucionSombras = new List<string>() { "Baja", "Media", "Alta", "Muy Alta" };


	//listas ingles
	List<string> dificultadIng = new List<string>() { "Easy","Normal","Hard" };
	List<string> nivelViolenciaIng = new List<string>() { "On","Off"};
	List<string> idiomaIng = new List<string>() { "Spanish", "English", "Mapudungun" };

	List<string> calidadTexturasIng = new List<string>() { "High", "Medium", "Medium low", "Low" };
	List<string> sombrasIng = new List<string>() { "Deactivated", "Hard Shadows only", "Hard and Soft Shadows" };
	List<string> resolucionSombrasIng = new List<string>() { "Low", "Medium", "High", "Very High" };


	//Mapudungun
	List<string> dificultadMapu = new List<string>() { "Zuguno", "Normal", "Ajwekeci" };
	List<string> nivelViolenciaMapu = new List<string>() { "Kvlfvn", "Desactivada" };
	List<string> idiomaMapu = new List<string>() { "Wigkazugun", "Inglés", "Mapudungun" };


    //Variable para cargar datos de glosario
    public Text glosarioCont, txtVolverMenuP,txtGlosarioTitulo;
    public GameObject glosarioEsp, glosarioIng;
    

    //variables para menu graficos
    public Text txtOpGraficos, txtPantallaCompleta, txtCalidadDeTexturas, txtSombras, txtResolucionSombras, txtAplicarConfi, txtVolverMenu,
                 txtResolucionPantalla;

    // variables de menu sonido
    public Text txtTituloSonido, txtVolumenGeneral, txtMusica, txtFx, txtGuardarConfSonido, txtVolverMenuPrincipal;

	// variables de menu salir
	public Text txtReiniciar, txtVolver, txtSalir, titulo_salir;

    //Variables grafico
    public Dropdown dpcalidadTexturas, dpsombras, dpresolucionSombras;

    //Menu muerte
    public Text txtHasmuerto, txtReintentar, txtMenuP;

    // Use this for initialization
    void Start()
    {
		idioma = Variables_globales.idioma;
		if (idioma == null)
			idioma = "espanol";
        traductor();
        CargarGlosario();
    }

    void traductor()
    {
		// idioma = Variables_globales.idioma;
        if (idioma=="espanol" || idioma==null)
        {
            //menu principal
            btnContinuar.GetComponentInChildren<Text>().text = "Continuar";
            btnOpciones.GetComponentInChildren<Text>().text = "Opciones";
            btnGlosario.GetComponentInChildren<Text>().text = "Glosario";
            btnSalir.GetComponentInChildren<Text>().text = "Salir";
            tituloPrincipal.text = "Pausa";

            //menu opciones
            btnVolverOpciones.GetComponentInChildren<Text>().text = "Volver";
            btnGeneral.GetComponentInChildren<Text>().text = "General";
            btnGraficos.GetComponentInChildren<Text>().text = "Gráficos";
            btnSonido.GetComponentInChildren<Text>().text = "Sonido";
			txtAdvertencia.GetComponentInChildren<Text>().text = "Algunas configuraciones necesitan reiniciar la partida";
            //Menu  generales
            txtTituloOpcionesGenerales.text = "Opciones Generales";
            txtDificultad.text = "Dificultad";
            txtNivelViolencia.text = "Nivel de Violencia";
            txtIdioma.text = "Idioma";
            btnVolverGenerales.GetComponentInChildren<Text>().text = "Volver";
            btnAplicarGenerales.GetComponentInChildren<Text>().text = "Aplicar";
            txtActivarPuntero.text = "Activar puntero";
            txtSensibilidad.text = "Sensibilidad del ratón";

            dpDificultad.ClearOptions(); //vaciar desplegable dificultad
            dpNivelViolencia.ClearOptions();//vacias desplegable violencia
            dpIdioma.ClearOptions(); //vaciar desplegable idioma
 

            //agregar opciones en español
            dpDificultad.AddOptions(dificultadEsp);
            dpNivelViolencia.AddOptions(nivelViolenciaEsp);
            dpIdioma.AddOptions(idiomaEsp);
            dpIdioma.value = 0;
             

            //cargar datos de glosario
            CargarGlosario();
            txtGlosarioTitulo.text = "Glosario";
            txtVolverMenuP.text = "Volver";

            //menu graficos
            txtOpGraficos.text = "Opciones de Gráficos";
            txtPantallaCompleta.text = "Pantalla Completa";
            txtResolucionPantalla.text = "Resolución de Pantalla";
            txtCalidadDeTexturas.text = "Calidad de Texturas";
            txtSombras.text = "Sombras";
            txtResolucionSombras.text = "Resolución de Sombras";
			txtAplicarConfi.text = "Aplicar";
            txtVolverMenu.text = "Volver";
            // limpiar dropdown
            dpcalidadTexturas.ClearOptions();
            dpsombras.ClearOptions();
            dpresolucionSombras.ClearOptions();
            //cargar dropdown
            dpcalidadTexturas.AddOptions(calidadTexturas);
            dpsombras.AddOptions(sombras);
            dpresolucionSombras.AddOptions(resolucionSombras);

            //menu sonido
            txtTituloSonido.text = "Opciones de Sonido";
            txtVolumenGeneral.text = "Volúmen General";
            txtMusica.text = "Música";
            txtFx.text = "Efectos de Sonido";
			txtGuardarConfSonido.text = "Aplicar";
            txtVolverMenuPrincipal.text = "Volver";

			//menu salir
			txtReiniciar.text = "Reiniciar";
			txtVolver.text = "Volver";
			txtSalir.text = "Salir";
			titulo_salir.text = "Salir";

            //menu muerte
            txtHasmuerto.text = "Has Muerto";
            txtReintentar.text = "Reintentar";
            txtMenuP.text = "Menú Principal";


        }
        if (idioma.Equals("ingles"))
        {
            //Menu principal ingles
            btnContinuar.GetComponentInChildren<Text>().text = "Continue";
            btnOpciones.GetComponentInChildren<Text>().text = "Options";
            btnGlosario.GetComponentInChildren<Text>().text = "Glossary";
            tituloPrincipal.text = "Pause";


            btnSalir.GetComponentInChildren<Text>().text = "Exit";
            //Menu opciones ingles
            btnVolverOpciones.GetComponentInChildren<Text>().text = "Back";
            btnGeneral.GetComponentInChildren<Text>().text = "General";
            btnGraficos.GetComponentInChildren<Text>().text = "Graphics";
            btnSonido.GetComponentInChildren<Text>().text = "Sound";
			txtAdvertencia.text = "Some configurations need restart the game";

            //Menu  generales
            txtTituloOpcionesGenerales.text = "General Options";
            txtDificultad.text = "Difficulty";
            txtNivelViolencia.text = "Violence level";
            txtIdioma.text = "Language";
            btnVolverGenerales.GetComponentInChildren<Text>().text = "Back";
            btnAplicarGenerales.GetComponentInChildren<Text>().text = "Apply";
            txtActivarPuntero.text = "Enable pointer";
            txtSensibilidad.text = "Mouse sensitivity";
            dpDificultad.ClearOptions(); //vaciar desplegable dificultad
            dpNivelViolencia.ClearOptions();//vacias desplegable violencia
            dpIdioma.ClearOptions(); //vaciar desplegable idioma

            
            //agregar opciones en ingles
            dpDificultad.AddOptions(dificultadIng);
            dpNivelViolencia.AddOptions(nivelViolenciaIng);
            dpIdioma.AddOptions(idiomaIng);
            dpIdioma.value = 1;
            
            //glosario en
            CargarGlosario();
            txtVolverMenuP.text = "Back";
            txtGlosarioTitulo.text = "Glossary";


            //menu graficos
            txtOpGraficos.text = "Graphics options";
            txtPantallaCompleta.text = "Fullscreen";
            txtResolucionPantalla.text = "Screen resolution";
            txtCalidadDeTexturas.text = "Texture quality";
            txtSombras.text = "Shadows";
            txtResolucionSombras.text = "Shadows resolution";
            txtAplicarConfi.text = "Apply";
            txtVolverMenu.text = "Back";
            // limpiar dropdown
            dpcalidadTexturas.ClearOptions();
            dpsombras.ClearOptions();
            dpresolucionSombras.ClearOptions();
            //cargar dropdown
            dpcalidadTexturas.AddOptions(calidadTexturasIng);
            dpsombras.AddOptions(sombrasIng);
            dpresolucionSombras.AddOptions(resolucionSombrasIng);

            //menus sonido en
            txtTituloSonido.text = "Sound options";
            txtVolumenGeneral.text = "General volume";
            txtMusica.text = "BGM";
            txtFx.text = "Sound FX";
            txtGuardarConfSonido.text = "Apply";
            txtVolverMenuPrincipal.text = "Back";

			//menu salir
			txtReiniciar.text = "Restart";
			txtVolver.text = "Back";
			txtSalir.text = "Exit";
			titulo_salir.text = "Exit";

            //menu muerte
            txtHasmuerto.text = "You died";
            txtReintentar.text = "Retry";
            txtMenuP.text = "Main menu";


        }
		if (idioma.Equals("mapu"))
        {
			//menu principal mapu
			btnContinuar.GetComponentInChildren<Text>().text = "Continuar";
			btnOpciones.GetComponentInChildren<Text>().text = "Opciones";
			btnGlosario.GetComponentInChildren<Text>().text = "Glosario";
			btnSalir.GetComponentInChildren<Text>().text = "Xipan";
			tituloPrincipal.text = "Pausa";

			//menu opciones mapu
			btnVolverOpciones.GetComponentInChildren<Text>().text = "Kontun";
			btnGeneral.GetComponentInChildren<Text>().text = "General";
			btnGraficos.GetComponentInChildren<Text>().text = "Gráficos";
			btnSonido.GetComponentInChildren<Text>().text = "Sonido";
			txtAdvertencia.GetComponentInChildren<Text>().text = "Algunas configuraciones necesitan reiniciar la partida";
			//Menu  generales mapu
			txtTituloOpcionesGenerales.text = "Opciones Generales";
			txtDificultad.text = "Dificultad";
			txtNivelViolencia.text = "Nivel de Violencia";
			txtIdioma.text = "Idioma";
			btnVolverGenerales.GetComponentInChildren<Text>().text = "Kontun";
			btnAplicarGenerales.GetComponentInChildren<Text>().text = "Elvn";
			txtActivarPuntero.text = "Activar puntero";
			txtSensibilidad.text = "Sensibilidad del ratón";

			dpDificultad.ClearOptions(); //vaciar desplegable dificultad
			dpNivelViolencia.ClearOptions();//vacias desplegable violencia
			dpIdioma.ClearOptions(); //vaciar desplegable idioma


			//agregar opciones en español mapu
			dpDificultad.AddOptions(dificultadEsp);
			dpNivelViolencia.AddOptions(nivelViolenciaEsp);
			dpIdioma.AddOptions(idiomaEsp);
			dpIdioma.value = 2;


			//cargar datos de glosario mapu
			CargarGlosario();
			txtGlosarioTitulo.text = "Hemvl";
			txtVolverMenuP.text = "Kontun";

			//menu graficos mapu
			txtOpGraficos.text = "Opciones de Gráficos";
			txtPantallaCompleta.text = "Pantalla Completa";
			txtResolucionPantalla.text = "Resolución de Pantalla";
			txtCalidadDeTexturas.text = "Calidad de Texturas";
			txtSombras.text = "Sombras";
			txtResolucionSombras.text = "Resolución de Sombras";
			txtAplicarConfi.text = "Elvn";
			txtVolverMenu.text = "Kontun";
			// limpiar dropdown mapu
			dpcalidadTexturas.ClearOptions();
			dpsombras.ClearOptions();
			dpresolucionSombras.ClearOptions();
			//cargar dropdown mapu
			dpcalidadTexturas.AddOptions(calidadTexturas);
			dpsombras.AddOptions(sombras);
			dpresolucionSombras.AddOptions(resolucionSombras);

			//menu sonido mapu
			txtTituloSonido.text = "Opciones de Sonido";
			txtVolumenGeneral.text = "Volúmen General";
			txtMusica.text = "Música";
			txtFx.text = "Efectos de Sonido";
			txtGuardarConfSonido.text = "Elvn";
			txtVolverMenuPrincipal.text = "Kontun";

			//menu salir mapu
			txtReiniciar.text = "Reiniciar";
			txtVolver.text = "Kontun";
			txtSalir.text = "Xipan";
			titulo_salir.text = "Xipan";

			//menu muerte mapu
			txtHasmuerto.text = "Has Muerto";
			txtReintentar.text = "Reintentar";
			txtMenuP.text="Menú Principal";
        }

    }

    // menu de opcines el dropdown de idioma al cambiar idioma se carga el idioma seleccionado
    void OnEnable()
    {
        dpIdioma.onValueChanged.AddListener(delegate { OnCambioIdioma(); });

    }
    public void OnCambioIdioma()
    {
        int seleccionIdioma = dpIdioma.value;
        if (seleccionIdioma == 0)
        {
            idioma = "espanol";
			Variables_globales.idioma = "espanol";

        }
        if (seleccionIdioma == 1)
        {
            idioma = "ingles";
			Variables_globales.idioma = "ingles";
        }
        if (seleccionIdioma == 2)
        {
			idioma = "mapu";
			Variables_globales.idioma = "mapu";
        }

    }
    // Update is called once per frame
    void Update()
    {
        traductor();
	}

    private StreamReader leer;

    void CargarGlosario()
    {

		if (idioma.Equals("espanol") | idioma.Equals("mapu"))
        {
            // leer = new StreamReader(Application.dataPath + "/Resources/Glosario/es_mp.txt");
            glosarioEsp.SetActive(true);
            glosarioIng.SetActive(false);

        }
        if (idioma.Equals("ingles"))
        {
            glosarioEsp.SetActive(false);
            glosarioIng.SetActive(true);
            //  leer = new StreamReader(Application.dataPath + "/Resources/Glosario/en_mp.txt");

        }


       // string s = leer.ReadLine();
       // string con = "";
      /*  while ((s = leer.ReadLine()) != null)
        {
            s = " " + leer.ReadLine() + " \n";

            con = con + s;

        }
        Debug.Log(con);
        glosarioCont.text = con;
        leer.Close();*/


    }
}
