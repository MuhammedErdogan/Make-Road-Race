/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using CGameStudio;
using UnityEngine;
using Cinemachine;
public class GameController : BaseGameController
{

    [SerializeField] private ColorController colorController;
    private void OnEnable()
    {
        // Do not remove
        GameManager.instance.SetActiveController(this);
    }

    public override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        if (IsGamePlayable()) return;
    }

    public void IsFail()
    {
        PlayerStack playerStack = GameManager.instance.GetPlayerStack();
        if (playerStack.stackList.Count > 0)
            playerStack.RemoveFromStack(playerStack.stackList[0]);
        playerController.isFail = true;

        playerController.characterSelected.GetComponent<Animator>().enabled = false;

        cameraController.GetComponent<CinemachineBrain>()
            .m_UpdateMethod = CinemachineBrain.UpdateMethod.SmartUpdate;

        cameraController.SetPriorityTo(Constants.CamerasType.FallCam);

        Invoke(nameof(Fail), 1.75f);
    }

    private void Fail()
    {
        GameManager.instance.GetActiveController().LevelFailed();
    }

    public void OnFinalCollision()
    {
        cameraController.SetPriorityTo(Constants.CamerasType.FinishCam);
        GetComponent<SetGradientColor>().SetFinishGradient();
        playerController.isFinish = true;
    }

    public void IsFinish()
    {
        cameraController.SetPriorityTo(Constants.CamerasType.FinishCam_2);
        Invoke(nameof(IsCanNotMove), 0.15f);
        Invoke(nameof(Finish), 3.3f);
    }

    private void IsCanNotMove()
    {
        playerController.isCanMove = false;
    }

    private void Finish()
    {
        LevelCompleted();
    }

    public ColorController GetColorController() => colorController;
}