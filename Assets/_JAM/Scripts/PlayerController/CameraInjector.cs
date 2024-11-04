using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraInjector : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private CinemachineBrain m_CinemachineBrain;
    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    [SerializeField] private Transform m_CameraFollowTransform;

    private void Start()
    {
        PrioritySettings prioritySettings = new() { Enabled = true, Value = 0 };
        m_CinemachineCamera.Priority = prioritySettings;
        m_CinemachineCamera.Follow = m_CameraFollowTransform;
    }
}
