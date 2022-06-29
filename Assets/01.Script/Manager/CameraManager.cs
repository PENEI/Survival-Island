using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    public CinemachineControl cinemachineControl;
    [SerializeField]
    Cinemachine.CinemachineImpulseSource ImpulseSource;

    protected override void SingletonInit()
    {
        base.SingletonInit();

        cinemachineControl = FindObjectOfType<CinemachineControl>();
        ImpulseSource = FindObjectOfType<Cinemachine.CinemachineImpulseSource>();
    }

    public void Impulse()
    {
        ImpulseSource.GenerateImpulse();
    }
}
