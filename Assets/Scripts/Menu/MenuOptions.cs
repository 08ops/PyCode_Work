using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject playMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject connection_ui;

    private void Start()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        connection_ui.SetActive(false);

    }
    public void ActivatePlayMenu()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(true);
        settingsMenu.SetActive(false);
        connection_ui.SetActive(false);
    }
    public void ToMainMenu()
    {
        mainMenu.SetActive(true);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        connection_ui.SetActive(false);

    }
    public void Connection()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        connection_ui.SetActive(false);
        //adding connection functionality here
    }
    public void SerringsMenu()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        settingsMenu.SetActive(true);
        connection_ui.SetActive(false);
    }

    public void connectin_ui()
    {
        mainMenu.SetActive(false);
        playMenu.SetActive(false);
        settingsMenu.SetActive(false);
        connection_ui.SetActive(true);
    }

}
