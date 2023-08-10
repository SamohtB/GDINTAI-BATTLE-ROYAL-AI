using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GeneralUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public void OnQuitPressed()
    {
        Application.Quit();
    }

    public void OnPlayPressed()
    {

        SceneManager.LoadScene(dropdown.value);
    }

    public void OnMainMenuPressed()
    {
        SceneManager.LoadScene(1);
    }
}
