using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonClass<T> where T : class, new()
{
    protected static T m_instance = null;

    protected virtual void SingletonInit() { }

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = new T();
                SingletonClass<T> tempinst = m_instance as SingletonClass<T>;
                tempinst.SingletonInit();
                if (m_instance == null)
                {
                    Debug.LogError(typeof(T) + "is none.");
                }
            }

            return m_instance;

        }
    }
}
