using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestImpulse : MonoBehaviour
{
    [SerializeField]
    Cinemachine.CinemachineImpulseSource ImpulseSource;

    [ContextMenu("Impulse")]
    void Impulse()
    {
        ImpulseSource.GenerateImpulse();
    }
}
