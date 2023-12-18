using UnityEngine;
using Cinemachine;

[ExecuteInEditMode][SaveDuringPlay][AddComponentMenu("")]
public class LockCamera : CinemachineExtension
{
    [Tooltip("Lock the camera's X position to this value")]
    public float m_XPosition = 10;
    [Tooltip("Lock the camera's Y position to this value")]
    public float m_YPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.RawPosition;
            pos.x = m_XPosition;
            pos.y = m_YPosition;
            state.RawPosition = pos;
        }
    }
}
