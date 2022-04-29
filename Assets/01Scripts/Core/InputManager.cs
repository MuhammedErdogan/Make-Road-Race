/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;

namespace CGameStudio
{
    public class InputManager : MonoBehaviour
    {
        public event InputManagerStartEvent StartInput;
        public event InputManagerTouchEvent TouchInput;

        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                // Do not remove
                StartInput?.Invoke();
                TouchInput?.Invoke(true);
                //lastFrameTouch = Input.mousePosition;
                return;
            }
            else
            {
                TouchInput?.Invoke(false);
                //lastFrameTouch = Input.mousePosition;
                return;
            }
        }
    }
}