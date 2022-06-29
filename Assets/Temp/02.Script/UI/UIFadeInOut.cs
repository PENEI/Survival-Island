using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve fadeCurveType;
    private Image image;
    public float fadeSpeed;     //ȭ�� ��ȯ �ӵ�
    public float fadeTime;      //��ο��� ȭ�� ���� �ð�
    
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

    //���̵��ξƿ� 
    /*
     * start�� 0, end�� 1�̸� ���̵� ��
     * start�� 1, end�� 0�̸� ���̵� �ƿ�
     */
    IEnumerator IEFade(float start, float end)
    {
        float ntime = 0f;
        float percent = 0;
        while(percent < 1)
        {
            //���ǵ� �ð� ������ ȭ�� ��ȯ
            ntime += Time.deltaTime;
            percent = ntime / (fadeSpeed);
            //�˺��� ���� start->End���� ntime���� ��ȯ
            Color color = image.color;
            color.a = Mathf.Lerp(start, end, fadeCurveType.Evaluate(percent));
            image.color = color;
            yield return null;
        }
    }
}
