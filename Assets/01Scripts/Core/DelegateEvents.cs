/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using UnityEngine;

/// <summary>
/// Triggered on the first touch of the player.
/// </summary>
public delegate void InputManagerStartEvent();

/// <summary>
/// Getting a notification when a toucj action occurred.
/// </summary>
public delegate void InputManagerTouchEvent(bool touchState);

/// <summary>
/// Use to track the player position.
/// </summary>
public delegate void PlayerControllerPositionEvent(Vector3 transformPosition);