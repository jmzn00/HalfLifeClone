using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button quitGameButton;

    private void Awake()
    {
        startGameButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });
        quitGameButton.onClick.AddListener(() => 
        {
            Application.Quit();        
        });
    }
}
