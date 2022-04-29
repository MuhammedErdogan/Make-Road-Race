/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using CGameStudio;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour, ILevelController
{
    public event PlayerControllerPositionEvent PositionEvent;

    [HideInInspector] public PlayerAnimatorController playerAnimatorController;
    [HideInInspector] public Transform stackFollowPoint;
    [HideInInspector] public GameObject characterSelected;
    [HideInInspector] public bool isFinish;
    [HideInInspector] public bool isFail;

    [Header("Configurations")]
    public float baseForwardSpeed = 12f;
    public bool isCanMove = true;

    private InverseKinematic inverseKinematic;
    private bool skeatingAnimApplied = true;
    private float lowerSpeed, upperSpeed;
    private bool manuelChanging = false;
    private Vector3 moveDirection;

    private void Start()
    {
        inverseKinematic = characterSelected.GetComponent<InverseKinematic>();
        lowerSpeed = baseForwardSpeed * 0.15f;
        upperSpeed = baseForwardSpeed;
    }

    private void Update()
    {
        if (!GameManager.instance.GetActiveController().IsGamePlayable()) return;

        if (!isCanMove)
            return;

        transform.Translate(moveDirection * Time.deltaTime * baseForwardSpeed);

        PositionEvent?.Invoke(transform.position);
    }

    #region InputManager Events

    private void HandleTouchInput(bool status)
    {
        if (status)
        {
            CancelInvoke(nameof(SetUpperSpeed));

            if (!isFinish)
                playerAnimatorController.SetSkateAnimator(1);

            baseForwardSpeed = lowerSpeed;

            if (skeatingAnimApplied)
                StopSkatingAnim();
        }
        else
        {
            playerAnimatorController.SetSkateAnimator(0);

            if (manuelChanging)
                return;

            Invoke(nameof(SetUpperSpeed), 0.35f);

            if (skeatingAnimApplied)
                return;

            Invoke(nameof(BackToForwardRotation), .5f);

            if (!isFinish)
                StartSkatingAnim();
        }
    }

    public void ChangeMoveDirection(Vector3 direction)
    {
        moveDirection = Vector3.Lerp(moveDirection, direction, Time.deltaTime * 4f);
    }

    private void SetUpperSpeed()
    {
        baseForwardSpeed = upperSpeed;
    }
    public void ManuelControlOfspeed(float manuelValue, float changeSpeed)
    {
        manuelChanging = true;
        baseForwardSpeed = Mathf.Lerp(baseForwardSpeed,
            manuelValue,
            Time.deltaTime * changeSpeed);
    }
    public void ManuelChangingEnd(float acceleration)
    {
        if (!manuelChanging)
            return;

        StartCoroutine(ManuelChangingAndDelay());
        baseForwardSpeed = Mathf.Lerp(baseForwardSpeed,
            upperSpeed,
            Time.deltaTime * acceleration);
    }
    private void StartSkatingAnim()
    {
        inverseKinematic.CancelRightFootKinematic();
        playerAnimatorController.SetCharacterAnimation(3);
        skeatingAnimApplied = true;
    }

    private void BackToForwardRotation()
    {
        Vector3 targetRot = isFinish ? new Vector3(0, 75, 0) : new Vector3(0, 90, 0);
        characterSelected.transform.DOLocalRotate(targetRot, .9f);
    }

    private void StopSkatingAnim()
    {
        characterSelected.transform.DOKill();
        skeatingAnimApplied = false;
        characterSelected.transform.DOLocalRotate(Vector3.up * 35, .6f);
    }

    #endregion

    private IEnumerator ManuelChangingAndDelay()
    {
        yield return new WaitForSeconds(1f);
        manuelChanging = false;
    }
    public float GetSpeed() => baseForwardSpeed;

    #region ILevelController
    public void LevelCompleted()
    {
        GetComponent<InputManager>().TouchInput -= HandleTouchInput;
    }

    public void LevelFailed()
    {
        GetComponent<InputManager>().TouchInput -= HandleTouchInput;
    }

    public void LevelStarted()
    {
        GetComponent<InputManager>().TouchInput += HandleTouchInput;
    }

    #endregion
}