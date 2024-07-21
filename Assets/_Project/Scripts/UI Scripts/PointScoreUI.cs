using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [Header("Refrence")]
    [SerializeField] private Text scoreText;

    private void Start()
    {
        scoreText.text = "0 / " + scoreManager.GetTotalPoint();
    }

    private void OnEnable()
    {
        PlayerController.onPointBoxCollected += ReduceBox;
    }
    private void OnDisable()
    {
        PlayerController.onPointBoxCollected -= ReduceBox;
    }

    private void ReduceBox(int i)
    {
        scoreText.text = scoreManager.GetTotalPoint()-scoreManager.GetCurrentPoint()+" / "+scoreManager.GetTotalPoint();
    }

}
