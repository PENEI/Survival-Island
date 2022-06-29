using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Du3Project
{
	public class UIItemElement : MonoBehaviour
	{
        [Header("[자료들]")]
        public Image ItemIcon = null;

        [Header("[데이터용Index]")]
        public int ItemIndex = -1;

        public virtual void UpdateItem(int p_index)
        {
            ItemIndex = p_index;
            
			int buildid = MakingManager.Instance.BuildList[ItemIndex].ResultItemID;
            ItemIcon.sprite = UIManager.Instance.BuildPanel.GetBuildObjSprite(buildid); //이미지 세팅
        }

        public virtual void _On_ImgButton()
        {
            UIManager.Instance.BuildPanel.SetBuildInfoText(MakingManager.Instance.BuildList[ItemIndex]);    //건축 정보 세팅
        }
	}
}