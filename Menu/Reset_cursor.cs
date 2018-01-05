using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_cursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}
