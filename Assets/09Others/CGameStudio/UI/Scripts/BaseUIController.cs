/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using TMPro;

namespace CGameStudio
{
    public class BaseUIController : MonoBehaviour, ILevelController
    {
        public GameObject uiFail;
        public GameObject uiLevel;
        public GameObject uiStart;
        public GameObject uiWin;
        public TextMeshProUGUI levelText;

        [Header("Configs")]
        public bool startUIActive = false;
        public string prefixLevelText = "Level  ";

        private void Awake() 
        {
            // Set the current level
            levelText.text = $"{prefixLevelText}{GameManager.instance.GetCurrentLevel().ToString()}";
            // Check the visibility of the 'Start UI'
            if (startUIActive) SetStartUIActive(true);
        }

        protected void SetStartUIActive(bool value)
        {
            uiStart.SetActive(value);
        }

        #region ILevelController
        public virtual void LevelStarted()
        {
            uiLevel.SetActive(true);
            
            if (startUIActive && uiStart.activeSelf)
                SetStartUIActive(false);
        }

        public void LevelCompleted()
        {
            uiLevel.SetActive(false);
            uiWin.SetActive(true);
        }

        public void LevelFailed()
        {
            uiLevel.SetActive(false);
            uiFail.SetActive(true);
        }
        #endregion
    }
}