using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSpeed : MonoBehaviour {
    private int speedM;
    [SerializeField]
    private Slider slSpeed;
    [SerializeField]
    private Text valorSensitivity;

    float cam;
   
    void OnEnable()
    {
        cam = ThirdPersonOrbitCam.horizontalAimingSpeed;
        slSpeed.onValueChanged.AddListener(delegate { cambiarVelocidad(); }); 
        valorSensitivity.text = cam.ToString();
        slSpeed.value = cam; 
	}
	
	
   
    public void cambiarVelocidad()
    {

		ThirdPersonOrbitCam.horizontalAimingSpeed = slSpeed.value;
        ThirdPersonOrbitCam.verticalAimingSpeed = slSpeed.value;	
		Camara_frente.horizontalAimingSpeed = slSpeed.value;
		Camara_frente.verticalAimingSpeed = slSpeed.value; 
        valorSensitivity.text = slSpeed.value.ToString();
    }
    

}
