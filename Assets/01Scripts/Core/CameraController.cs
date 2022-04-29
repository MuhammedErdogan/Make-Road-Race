/**
 * Copyright (c) 2021-present Compactive Game Studio. All rights reserved.
 * 'CGameStudio' can not be copied and/or distributed without the express permission of Compactive Game Studio.
 */

using static Constants;
using CGameStudio;
using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraController : BaseCameraController, ILevelController
{
    DynamicCameras[] dynamicCameras;

    override protected void Start()
    {
        base.Start();

        dynamicCameras = GameObject.FindObjectsOfType<DynamicCameras>();
    }
    public void SetPriorityTo(CamerasType cameraType)
    {
        foreach (var camera in dynamicCameras)
        {
            if (camera.CameraType == cameraType)
                camera.SetPriory();
            else
                camera.ResetPriority();
        }
    }

    public GameObject GetCamera(CamerasType cameraType)
    {
        foreach (var camera in dynamicCameras)
        {
            if (camera.CameraType == cameraType)
                return camera.gameObject;
        }

        return null;
    }

    public void ShakeCamera(CamerasType cameraType, float duration)
    {
        foreach (var camera in dynamicCameras)
        {
            if (camera.CameraType == cameraType)
            {
                camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 1;
                camera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 4;
                StartCoroutine(ResetPerlinNoise(camera.gameObject, duration));
            }
        }
    }

    private IEnumerator ResetPerlinNoise(GameObject cam, float duration)
    {
        yield return new WaitForSeconds(duration);
        cam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
        cam.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0;
    }

    public void LevelCompleted()
    {

    }

    public void LevelFailed()
    {

    }

    public void LevelStarted()
    {

    }
}