using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Idioma : MonoBehaviour {
    private string idioma;

    //variable menu principal
    public Button btnJugar, btnOpciones, btnGlosario, btnCreditos, btnSalir;

    //variables opciones
    public Button btnVolverOpciones, btnGeneral, btnGraficos, btnSonido;

    //Variables op generales
    public Button btnVolverGenerales, btnAplicarGenerales;
    public Dropdown dpDificultad, dpNivelViolencia, dpIdioma;
    public Text txtTituloOpcionesGenerales, txtDificultad, txtNivelViolencia, txtIdioma,txtSensibilidad, txtActivarPuntero;
    //menu extras
    public Text txtCultura, txtGlosario, txtPersonajes, txtVolverdeExtras;
    //Variables grafico
    public Dropdown dpcalidadTexturas, dpsombras, dpresolucionSombras;                                          

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
    public Text glosario, txtVolverMenuP;
    private string glos;

    //personajes
    public GameObject personajeEsp, personajeEng;

    //variables para menu graficos
    public Text txtOpGraficos, txtPantallaCompleta, txtCalidadDeTexturas, txtSombras, txtResolucionSombras, txtAplicarConfi, txtVolverMenu,
                 txtResolucionPantalla;

    // variables de menu sonido
    public Text txtTituloSonido, txtVolumenGeneral, txtMusica, txtFx, txtGuardarConfSonido, txtVolverMenuPrincipal;

    // variables de menu creditos
    public Text txtVolvermenu, txtCreditos;

	// variables de menu creditos
	public Text txtGlosario_t, txtPersonajes_t;


    // Use this for initialization
    void Start () {
		idioma = Variables_globales.idioma;
		if (idioma == null)
			idioma = "espanol";
        traductor();
        CargarGlosario();
    }
   

    void traductor()
    {
		if (idioma.Equals("espanol"))
        {
            //menu principal
            btnJugar.GetComponentInChildren<Text>().text = "Jugar demo";
            btnOpciones.GetComponentInChildren<Text>().text = "Opciones";
            btnGlosario.GetComponentInChildren<Text>().text = "Cultura";
            btnCreditos.GetComponentInChildren<Text>().text = "Créditos";
            btnSalir.GetComponentInChildren<Text>().text = "Salir";

            //menu opciones
            btnVolverOpciones.GetComponentInChildren<Text>().text ="Volver";
            btnGeneral.GetComponentInChildren<Text>().text = "General";
            btnGraficos.GetComponentInChildren<Text>().text = "Gráficos";
            btnSonido.GetComponentInChildren<Text>().text = "Sonido";
            //Menu  generales
            txtTituloOpcionesGenerales.text = "Opciones Generales";
            txtDificultad.text = "Dificultad";
            txtNivelViolencia.text = "Nivel de Violencia";
            txtIdioma.text = "Idioma";
            btnVolverGenerales.GetComponentInChildren<Text>().text = "Volver";
            btnAplicarGenerales.GetComponentInChildren<Text>().text = "Aplicar";
            txtActivarPuntero.text = "Activar puntero";
			txtSensibilidad.text = "Sensibilidad del ratón";
           // btnCambiarTeclas.GetComponentInChildren<Text>().text = "Cambiar Teclas";
            dpDificultad.ClearOptions(); //vaciar desplegable dificultad
            dpNivelViolencia.ClearOptions();//vacias desplegable violencia
            dpIdioma.ClearOptions(); //vaciar desplegable idioma

            //menu extras
            txtCultura.text = "Cultura"; 
            txtGlosario.text = "Glosario";
            txtPersonajes.text = "Personajes";
            txtVolverdeExtras.text = "Volver";

            //agregar opciones en español
            dpDificultad.AddOptions(dificultadEsp); 
            dpNivelViolencia.AddOptions(nivelViolenciaEsp); 
            dpIdioma.AddOptions(idiomaEsp);
            dpIdioma.value = 0;
           

            //cargar datos de glosario
            CargarGlosario();
            txtVolverMenuP.text = "Volver";

            //personaje
            CargarPersonaje();

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

            //menu creditos
            txtVolvermenu.text = "Volver";
			txtCreditos.text = "Demo creada por Meido Games:\nYerko Andrei Orellana Abello\nMaría Natalia Guajardo Muñoz\n\nTesis INACAP Sede Maipú 2017";

			//Glosario y personajes

        }
        if (idioma.Equals("ingles"))
        {
            //Menu principal ingles
            btnJugar.GetComponentInChildren<Text>().text = "Play demo";
            btnOpciones.GetComponentInChildren<Text>().text = "Options";
            btnGlosario.GetComponentInChildren<Text>().text = "Culture";
            btnCreditos.GetComponentInChildren<Text>().text = "Credits";
            btnSalir.GetComponentInChildren<Text>().text = "Exit";
            //Menu opciones ingles
            btnVolverOpciones.GetComponentInChildren<Text>().text = "Back";
            btnGeneral.GetComponentInChildren<Text>().text = "General";
            btnGraficos.GetComponentInChildren<Text>().text = "Graphics";
            btnSonido.GetComponentInChildren<Text>().text = "Sound";

            //Menu  generales
            txtTituloOpcionesGenerales.text = "General Options";
            txtDificultad.text = "Difficulty";
            txtNivelViolencia.text = "Violence level";
            txtIdioma.text = "Language";
            btnVolverGenerales.GetComponentInChildren<Text>().text = "Back";
            btnAplicarGenerales.GetComponentInChildren<Text>().text = "Apply";
            txtActivarPuntero.text = "Enable pointer";
            txtSensibilidad.text = "Mouse sensitivity";
            // btnCambiarTeclas.GetComponentInChildren<Text>().text = "Change keybinds";
            dpDificultad.ClearOptions(); //vaciar desplegable dificultad
            dpNivelViolencia.ClearOptions();//vacias desplegable violencia
            dpIdioma.ClearOptions(); //vaciar desplegable idioma

            //menu extras
            txtCultura.text = "Culture";
            txtGlosario.text = "Glossary";
            txtPersonajes.text = "Characters";
            txtVolverdeExtras.text = "Back";
            //agregar opciones en ingles
            dpDificultad.AddOptions(dificultadIng); 
            dpNivelViolencia.AddOptions(nivelViolenciaIng);
            dpIdioma.AddOptions(idiomaIng);
            dpIdioma.value=1;
         

            //glosario en
            CargarGlosario();
            txtVolverMenuP.text = "Back";
            //personajes
            CargarPersonaje();

            //menu graficos
            txtOpGraficos.text = "Graphics Options";
            txtPantallaCompleta.text = "Fullscreen";
            txtResolucionPantalla.text = "Screen Resolution";
            txtCalidadDeTexturas.text = "Texture quality";
            txtSombras.text = "Shadows";
            txtResolucionSombras.text = "Shadows Resolution";
            txtAplicarConfi.text = "Apply";
            txtVolverMenu.text = "Back to Menu";

            // limpiar dropdown
            dpcalidadTexturas.ClearOptions();
            dpsombras.ClearOptions();
            dpresolucionSombras.ClearOptions();
            //cargar dropdown
            dpcalidadTexturas.AddOptions(calidadTexturasIng);
            dpsombras.AddOptions(sombrasIng);
            dpresolucionSombras.AddOptions(resolucionSombrasIng);

            //menus sonido en
            txtTituloSonido.text = "Sound Options";
            txtVolumenGeneral.text = "General Volume";
            txtMusica.text = "BGM";
            txtFx.text = "Sound FX";
            txtGuardarConfSonido.text = "Apply";
            txtVolverMenuPrincipal.text = "Back";

            //menu creditos
            txtVolvermenu.text = "Back";
			txtCreditos.text = "Demo created by Meido Games:\nYerko Andrei Orellana Abello\nMaría Natalia Guajardo Muñoz\n\nThesis INACAP Maipú 2017";

			//Glosario y personajes

        }
		if (idioma.Equals("mapu"))
        {
			//menu principal mapu
			btnJugar.GetComponentInChildren<Text>().text = "Awkantun demo";
			btnOpciones.GetComponentInChildren<Text>().text = "Opciones";
			btnGlosario.GetComponentInChildren<Text>().text = "Cultura";
			btnCreditos.GetComponentInChildren<Text>().text = "Créditos";
			btnSalir.GetComponentInChildren<Text>().text = "Xipan";

			//menu opciones mapu
			btnVolverOpciones.GetComponentInChildren<Text>().text ="Kontun";
			btnGeneral.GetComponentInChildren<Text>().text = "General";
			btnGraficos.GetComponentInChildren<Text>().text = "Gráficos";
			btnSonido.GetComponentInChildren<Text>().text = "Sonido";
			//Menu  generales mapu
			txtTituloOpcionesGenerales.text = "Opciones Generales";
			txtDificultad.text = "Dificultad";
			txtNivelViolencia.text = "Nivel de Violencia";
			txtIdioma.text = "Idioma";
			btnVolverGenerales.GetComponentInChildren<Text>().text = "Kontun";
			btnAplicarGenerales.GetComponentInChildren<Text>().text = "Elvn";
			txtActivarPuntero.text = "Activar puntero";
			txtSensibilidad.text = "Sensibilidad del ratón";
			// btnCambiarTeclas.GetComponentInChildren<Text>().text = "Cambiar Teclas";
			dpDificultad.ClearOptions(); //vaciar desplegable dificultad
			dpNivelViolencia.ClearOptions();//vacias desplegable violencia
			dpIdioma.ClearOptions(); //vaciar desplegable idioma

			//menu extras mapu
			txtCultura.text = "Kvmpeñ"; 
			txtGlosario.text = "Hemvl";
			txtPersonajes.text = "Personajes";
			txtVolverdeExtras.text = "Kontun";

			//agregar opciones en español mapu
			dpDificultad.AddOptions(dificultadEsp); 
			dpNivelViolencia.AddOptions(nivelViolenciaEsp); 
			dpIdioma.AddOptions(idiomaEsp);
			dpIdioma.value = 2;


			//cargar datos de glosario mapu
			CargarGlosario();
			txtVolverMenuP.text = "Kontun";

			//personaje mapu
			CargarPersonaje();

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

			//menu creditos mapu
			txtVolvermenu.text = "Kontun";
			txtCreditos.text = "Demo creada por Meido Games:\nYerko Andrei Orellana Abello\nMaría Natalia Guajardo Muñoz\n\nTesis INACAP Sede Maipú 2017";

			//Glosario y personajes mapu

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
        if (seleccionIdioma==0)
        {
            idioma = "espanol";
        }
        if (seleccionIdioma == 1)
        {
            idioma = "ingles";
        }
        if (seleccionIdioma == 2)
        {
			idioma = "mapu";
        }

    }
    // Update is called once per frame
    void Update () {
        traductor();
    }

    void CargarPersonaje()
    {
		if (idioma.Equals("espanol") | idioma.Equals("mapu"))
        {
            personajeEng.SetActive(false);
            personajeEsp.SetActive(true);


        }
        if (idioma.Equals("ingles"))
        {
            personajeEng.SetActive(true);
            personajeEsp.SetActive(false);

        }


    }

   
    public GameObject glosarioEsp, glosarioIng;

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

       
    }
}
