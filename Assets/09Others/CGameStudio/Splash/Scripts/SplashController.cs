/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CGameStudio
{
    public class SplashController : MonoBehaviour
    {
        [Header("Game Name")]
        public string gameNameText;
        public State current;
        public enum State
        {
            tapToPlay, autoPlay
        }
        public Background selectBackground;
        public enum Background
        {
            open, close, specific
        }
        public int backgroundNumber = 0;

        [Header("Objects")]
        public Sprite specificBackground;
        public Sprite[] backGround;

        [Header("DON'T CHANGE HERE")]
        // [HideInInspector]
        public TextMeshProUGUI gameName;
        // [HideInInspector]
        public Image panel;
        // [HideInInspector]
        public Button playButton;
        // [HideInInspector]
        public GameObject fillBar;
        // [HideInInspector]
        bool fillTheImage = false;
        // [HideInInspector]
        public Image fillImage;

        void Start()
        {
            switch (current)
            {
                case State.tapToPlay:
                    playButton.gameObject.SetActive(true);
                    break;
                case State.autoPlay:
                    playButton.gameObject.SetActive(false);
                    fillTheImage = true;
                    Invoke("PlayLevel", 2.7f);
                    fillBar.SetActive(true);
                    break;
            }

            switch (selectBackground)
            {
                case Background.open:
                    panel.sprite = backGround[backgroundNumber];
                    break;
                case Background.close:
                    panel.color = new Color(255 / 255f, 153 / 255f, 0f, 0);
                    break;
                case Background.specific:
                    panel.sprite = specificBackground;
                    break;
            }

            gameName.text = gameNameText;
        }

        void Update()
        {
            if (fillTheImage)
                fillImage.fillAmount += Time.deltaTime / 2.7f;
        }

        void PlayLevel() => GameManager.instance.PlayCurrentLevel();
    }
}