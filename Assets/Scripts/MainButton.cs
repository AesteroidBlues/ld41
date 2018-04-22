using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour {

	public void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnMainMenuClick()
    {
        SceneManager.LoadScene("game");
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene("main");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.SetActiveScene(scene);
    }
}
