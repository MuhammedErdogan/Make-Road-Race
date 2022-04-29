using UnityEngine;
using Cinemachine;
using static Constants;

public class DynamicCameras : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;

    public CamerasType CameraType;

    private void Start() => virtualCamera = GetComponent<CinemachineVirtualCamera>();

    public void ResetPriority() => virtualCamera.m_Priority = 0;

    public void SetPriory() => virtualCamera.m_Priority = 100;
}
