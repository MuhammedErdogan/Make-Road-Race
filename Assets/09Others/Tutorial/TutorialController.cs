/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using CGameStudio;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class TutorialElements
{
    [SerializeField] public UnityEngine.Video.VideoClip video;
    [SerializeField] public string videoExplanation;
}

public class TutorialController : MonoBehaviour
{
    [Header("Video Record")]
    [SerializeField] bool videoRecordConfigration;
    [SerializeField] GameObject handImage, hitImage;
    [SerializeField] float hitOffsetX = -12, hitOffsetY = 60;

    [Space(10)]
    [Header("Tutorial Panel")]
    [SerializeField] TutorialElements[] tutorialElements;
    [SerializeField] UnityEngine.Video.VideoPlayer videoPlayer;
    [SerializeField] GameObject tutorialPanel;
    private byte tutorialCount;
    [SerializeField] TextMeshProUGUI videoInfoTXT, tutorialVideoCountTXT;
    [SerializeField] Button nextButton;
    [SerializeField] Button playButton;

    void Start()
    {
        if (videoRecordConfigration)
        {
            handImage.SetActive(true);
            GameManager.instance.GetActiveController().uiController.uiLevel.SetActive(false);
            tutorialPanel.SetActive(false);
            return;
        }

        //// if (PlayerPrefs.GetInt("TutorialDone") == 1) return;
        //// GameManager.instance.GetActiveController().isTutorialActive = true;
        if (tutorialCount < 2)
            tutorialVideoCountTXT.gameObject.SetActive(false);

        videoPlayer.clip = tutorialElements[tutorialCount].video;
        TextRefresh();
        ButtonDefinition();
        tutorialPanel.SetActive(true);
        playButton.onClick.AddListener(PlayButton);
        nextButton.onClick.AddListener(NextButton);
        Time.timeScale = 0;
    }

    void ButtonDefinition()
    {
        playButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);

        if (tutorialElements.Length == 1)
            playButton.gameObject.SetActive(true);
        else
            nextButton.gameObject.SetActive(true);
    }

    void Update()
    {
        if (videoRecordConfigration)
            ImagePosition();
    }

    void ImagePosition()
    {
        handImage.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        hitImage.transform.position = new Vector2(Input.mousePosition.x + hitOffsetX,
        Input.mousePosition.y + hitOffsetY);

        if (Input.GetMouseButtonDown(0))
            hitImage.SetActive(true);

        if (Input.GetMouseButtonUp(0))
            hitImage.SetActive(false);
    }

    void NextButton()
    {
        tutorialCount++;
        if (tutorialCount + 1 == tutorialElements.Length)
        {
            playButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }
        videoPlayer.clip = tutorialElements[tutorialCount].video;
        TextRefresh();
    }

    void TextRefresh()
    {
        videoInfoTXT.text = tutorialElements[tutorialCount].videoExplanation;
        tutorialVideoCountTXT.text = (tutorialCount + 1).ToString() + "/" + tutorialElements.Length.ToString();
    }

    void PlayButton()
    {
        tutorialPanel.SetActive(false);
        PlayerPrefs.SetInt("TutorialDone", 1);
        Time.timeScale = 1;
        //// GameManager.instance.GetActiveController().isTutorialActive = false;
    }
}
