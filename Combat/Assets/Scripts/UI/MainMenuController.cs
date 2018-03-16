using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject mainMenuScreen;
    public GameObject loadingScreen;
    public GameObject optionsScreen;
    public Slider loadingBar;
    private AsyncOperation loader;

    public void PlayGame() {
        StartCoroutine("LoadGame");
	}

	public void Options() {
		// Switch to Options Menu
	}

	public void QuitGame() {
        Application.Quit();
	}

    IEnumerator LoadGame()
    {
        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);
        loader = SceneManager.LoadSceneAsync("MainScene");
        while (!loader.isDone)
        {
            loadingBar.value = loader.progress;
            yield return null;
        }
    }
}
