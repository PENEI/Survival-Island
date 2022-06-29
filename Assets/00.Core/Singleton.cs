using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour를 상속받은 클래스만 사용 가능하도록 제네릭 제약조건 설정
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T instance = null;
    // 초기에 실행할 함수
    protected virtual void SingletonInit()  {   }

    // Instance프로퍼티
    public static T Instance
    {
        get
        {
            // 아직 instance에 저장된게 없을 시
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;    // T타입 객체 찾기
                
                // instance가 Singleton<T>타입으로 형변환이 가능한지 체크
                Singleton<T> tempinst = instance as Singleton<T>;

                // 형변환이 안됬을 시 종료
                if (instance == null)
                {
                    return null;
                }

                // 형변환 가능 시 instance에 있는 SingletonInit함수 실행
                tempinst.SingletonInit();
            }
            return instance;
        }
    }
}
