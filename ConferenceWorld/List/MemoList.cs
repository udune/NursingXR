using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoList : ConferenceWorldList
{
    private List<MemoItem> itemList = new ();

    // enable: input, 움직임을 비활성화합니다.
    public override void OnEnable()
    {
        base.OnEnable();
        InputManager.Instance.isLock = true;
        NursingManager.Instance.character.SetMoveState(false);
    }

    // disable: input, 움직임을 활성화합니다.
    public override void OnDisable()
    {
        base.OnDisable();
        InputManager.Instance.isLock = false;
        NursingManager.Instance.character.SetMoveState(true);
    }
    
    // 메모 리스트를 불러옵니다.
    protected override void SetList()
    {
        for (int i = 0; i < Scene.data.File_Data.File_Items_Memo.Count; i++)
        {
            if (itemList.Count > 0 && itemList.Count > i)
            {
                itemList[i].SetMemo(Scene.data.File_Data.File_Items_Memo[i].username, Scene.data.File_Data.File_Items_Memo[i].memo);
                continue;
            }
            
            GameObject itemGo = Instantiate(item, contents);
            MemoItem memoItem = itemGo.GetComponent<MemoItem>();
            memoItem.SetMemo(Scene.data.File_Data.File_Items_Memo[i].username, Scene.data.File_Data.File_Items_Memo[i].memo);
            itemGo.SetActive(true);
            itemList.Add(memoItem);
        }
    }
}
