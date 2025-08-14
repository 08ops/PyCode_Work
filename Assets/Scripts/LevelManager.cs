using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject Level;

    private bool hasStarted = false; // Prevent accidental multiple calls

    private void Start()
    {
        Debug.Log("START: Showing tutorial");
        Tutorial.SetActive(true);
        Level.SetActive(false);
    }

    public void Start_level()
    {
        if (hasStarted) return;
        hasStarted = true;

        Debug.Log("LEVEL STARTING...");
        Tutorial.SetActive(false);
        Level.SetActive(true);
    }
}
