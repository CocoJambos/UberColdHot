using ECM2;
using System;
using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    [SerializeField] private float m_MinCameraAngle;
    [SerializeField] private float m_MaxCameraAngle;
    
    private float m_CurrentRotation;

    private void Start()
    {
        m_CurrentRotation = m_CinemachineCamera.Follow.localRotation.eulerAngles.x;
    }

    private void Update()
    {
        m_CinemachineCamera.Follow.localRotation = Quaternion.Euler(Vector3.right * m_CurrentRotation);
    }

    public Vector3 RelativeToCamera(Vector3 input) => input.relativeTo(m_CinemachineCamera.Follow);

    public void AddCameraInput(float input)
    {
        m_CurrentRotation = Mathf.Clamp(m_CurrentRotation += input, m_MinCameraAngle, m_MaxCameraAngle);
    }
}
