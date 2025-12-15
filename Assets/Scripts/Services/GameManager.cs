using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-99)]
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
    private void Start()
    {
        GameEnded(false, false);
    }

    public event Action<bool, bool> OnGameEnded; // ended? won?
    public void GameEnded(bool ended, bool won) 
    {
        OnGameEnded?.Invoke(ended, won);

        if (ended)
            GameServices.Input.TogglePlayerInput(false);
    }
    public void LoadScene(int index) 
    {
        SceneManager.LoadScene(index);
    }
}
