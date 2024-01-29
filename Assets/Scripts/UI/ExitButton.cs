using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
