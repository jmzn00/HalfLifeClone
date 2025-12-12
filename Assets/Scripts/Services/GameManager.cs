using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   
    private void Awake()
    {
        if (GameServices.GameManager != this)
            GameServices.GameManager = this;
    }
    private void OnDisable()
    {
        if(GameServices.GameManager == this)
            GameServices.GameManager = null;
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(0);
    }
}
