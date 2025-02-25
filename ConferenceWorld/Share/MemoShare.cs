using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemoShare : FileShare
{
    [SerializeField] public MemoList memoList;
    
    // 메모 리스트를 띄웁니다.
    public override void OnList()
    {
        memoList.gameObject.SetActive(true);
    }
}
