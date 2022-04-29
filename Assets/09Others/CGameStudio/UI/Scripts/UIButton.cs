/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;
using UnityEngine.UI;

namespace CGameStudio
{
    public class UIButton : MonoBehaviour
    {
        public bool isFailButton = false;

        void Start() => GetComponent<Button>().onClick.AddListener(() => OnButtonTapped());

        void OnButtonTapped()
        {
            if (isFailButton)
                GameManager.instance.RestartLevel();
            else
                GameManager.instance.PlayNextLevel();
        }
    }
}