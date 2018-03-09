using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Change from static if it no longer fits
public static class GameDirector
{
    //static bool once = false;


    public static void Restart()
    {
        Debug.Log("Reload Scene. (Currently game over)");
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
