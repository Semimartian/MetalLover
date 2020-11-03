using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    [SerializeField] private Camera camera;
    [SerializeField] private MainCamera mainCamera;

    [SerializeField] private ScoreFeedbackUI scoreFeedbackUIPreFab;
    [SerializeField] private Canvas canvas;
    [SerializeField] private FinalScoreText finalScoreText;
    [SerializeField] private Animator scaleAnimator;

    private static List<ScoreFeedbackUI> availableScoreFeedbackUIs = new List<ScoreFeedbackUI>();

    private void Awake()
    {
        instance = this;

        InitialiseScoreFeedbackUIs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            EndGame();
        }
    }
    private static void InitialiseScoreFeedbackUIs()
    {
        ExpandScoreFeedbackUIsPool();

    }

    private static void ExpandScoreFeedbackUIsPool()
    {
        for (int i = 0; i < 16; i++)
        {
            ScoreFeedbackUI feedbackUI = Instantiate(instance.scoreFeedbackUIPreFab);
            feedbackUI.transform.SetParent(instance.canvas.transform);
            feedbackUI.gameObject.SetActive(false);
            availableScoreFeedbackUIs.Add(feedbackUI);

        }
    }

    public static void Recycle(ScoreFeedbackUI scoreFeedbackUI)
    {
        scoreFeedbackUI.gameObject.SetActive(false);
        availableScoreFeedbackUIs.Add(scoreFeedbackUI);
    }

   public static void SpawnScoreFeedbackUI(Vector3 worldPosition, sbyte tier)
    {
        //Preperations:
        int score = 0;
        switch (tier)
        {
            case 0: score = 1; break;
            case 1: score = 10; break;
            case 2: score = 100; break;
        }
        Vector2 screenPosition = instance.camera.WorldToScreenPoint(worldPosition);

        if(availableScoreFeedbackUIs.Count == 0)
        {
            ExpandScoreFeedbackUIsPool();
        }

        ScoreFeedbackUI scoreFeedbackUI = availableScoreFeedbackUIs[availableScoreFeedbackUIs.Count - 1];
        availableScoreFeedbackUIs.RemoveAt(availableScoreFeedbackUIs.Count - 1);
        scoreFeedbackUI.gameObject.SetActive(true);
        scoreFeedbackUI.Spawn(screenPosition, score);
    }


    public void EndGame()
    {
        scaleAnimator.SetTrigger("Play");
        mainCamera.StartCoroutine(mainCamera.GoToFinalDestination());
        finalScoreText.PlayAnimation();
    }
}
