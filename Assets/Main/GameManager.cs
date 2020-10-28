using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static List<ScoreFeedbackUI> availableScoreFeedbackUIs = new List<ScoreFeedbackUI>();
    public static void Recycle(ScoreFeedbackUI scoreFeedbackUI)
    {
        scoreFeedbackUI.gameObject.SetActive(false);
        availableScoreFeedbackUIs.Add(scoreFeedbackUI);
    }

    [SerializeField] Camera mainCamera;
    public static void SpawnScoreFeedbackUI(Vector3 worldPosition, sbyte tier)
    {

        int score = 
        Vector2 screenPosition = instance.mainCamera.WorldToScreenPoint(worldPosition);
        ScoreFeedbackUI scoreFeedbackUI = availableScoreFeedbackUIs[availableScoreFeedbackUIs.Count - 1];
        availableScoreFeedbackUIs.RemoveAt(availableScoreFeedbackUIs.Count - 1);
        scoreFeedbackUI.gameObject.SetActive(true);
        scoreFeedbackUI.Spawn();

        scoreFeedbackUI.gameObject.SetActive(false);
        availableScoreFeedbackUIs.Add(scoreFeedbackUI);
    }
}
