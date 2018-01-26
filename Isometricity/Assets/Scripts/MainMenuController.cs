using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	public void PlayGame() {
		SceneManager.LoadScene("IsoScene");
	}

	public void Options() {
		// Switch to Options Menu
	}

	public void QuitGame() {
		// Quit game 
	}
}
