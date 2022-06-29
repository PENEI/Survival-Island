using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonDontDestroy<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;

    protected virtual void SingletonDontDestroyInit() { }
    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType(typeof(T)) as T;
                SingletonDontDestroy<T> tempinst = m_instance as SingletonDontDestroy<T>;
                if (m_instance == null)
                {
                    return null;
                }
                DontDestroyOnLoad(m_instance.gameObject);
                tempinst.SingletonDontDestroyInit();
            }

            return m_instance;

        }
    }
}
