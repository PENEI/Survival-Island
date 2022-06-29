using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve fadeCurveType;
    private Image image;
    public float fadeSpeed;     //화면 전환 속도
    public float fadeTime;      //어두워진 화면 유지 시간
    
    public void Awake()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        StartCoroutine(IEFadeInOut());
    }
    IEnumerator IEFadeInOut()
    {
        Player.Instance.Control.SetAllowAll(false);
        yield return StartCoroutine(IEFade(0, 1));
        yield return new WaitForSeconds(fadeTime);
        yield return StartCoroutine(IEFade(1, 0));
        Player.Instance.Control.SetAllowAll(true);
        gameObject.SetActive(false);
    }

    //페이드인아웃 
    /*
     * start가 0, end가 1이면 페이드 인
     * start가 1, end가 0이면 페이드 아웃
     */
    IEnumerator IEFade(float start, float end)
    {
        float ntime = 0f;
        float percent = 0;
        while(percent < 1)
        {
            //스피드 시간 동안의 화면 전환
            ntime += Time.deltaTime;
            percent = ntime / (fadeSpeed);
            //알베도 값을 start->End까지 ntime동안 변환
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurveType.Evaluate(percent));
            image.color = color;
            yield return null;
        }
    }
}
