using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr : MonoBehaviour {

	public Button play;
	public Button quit;

	// Use this for initialization
	void Start () {
		play.onClick.AddListener (moveToGame);
		quit.onClick.AddListener (quitGame);
		
	}

	void moveToGame() {
		SceneManager.LoadSceneAsync (1);
	}

	void quitGame() {
		Application.Quit ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
