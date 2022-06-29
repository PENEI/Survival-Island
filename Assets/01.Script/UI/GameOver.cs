using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Text m_Text;
    [SerializeField]
    Image m_BG;

    public float GameOverDuration = 2.0f;          //게임오버 페이드 시간

    public void SetGameOver(string sceneName = "TitleScene")
    {
        //세이브 데이터 삭제
        XmlManager.Instance.DeleteFile();
        this.gameObject.SetActive(true);
        UIManager.Instance.CursorOnOff(true);
        m_BG.DOFade(0.8f, GameOverDuration);
        m_Text.DOFade(1.0f, GameOverDuration).SetEase(Ease.OutSine)
            .OnComplete(() => {
                SceneLoader.Instance.LoadGameScene(sceneName, false);
            });
    }
}
