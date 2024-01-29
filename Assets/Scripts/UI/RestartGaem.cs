using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGaem : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Restart Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
