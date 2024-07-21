using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelBg;

    private void OnEnable()
    {
        PlayerController.onGameOver += ActivePanel;
    }
    private void OnDisable()
    {
        PlayerController.onGameOver -= ActivePanel;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ActivePanel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        panelBg.SetActive(true);

    }
}
