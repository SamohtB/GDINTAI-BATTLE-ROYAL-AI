using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralUI : MonoBehaviour
{

    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene(1);
    }
}
