using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject panelBg;

    private void OnEnable()
    {
        ScoreManager.onGameWon += ActivePanel;
    }
    private void OnDisable()
    {
        ScoreManager.onGameWon -= ActivePanel;
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
