using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MicList : ConferenceWorldList
{
    private List<MicItem> itemList = new ();
    
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

    // 접속한 유저들의 마이크 상태 리스트를 불러옵니다.
    protected override void SetList()
    {
        int idx = 0;
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == player.ActorNumber)
                continue;

            PhotonView pv = PhotonManager.Instance.FindCharacter(player.ActorNumber).GetComponent<PhotonView>();
            
            if (itemList.Count > 0 && idx < itemList.Count)
            {
                itemList[idx].SetMic(pv);
                idx++;
                continue;
            }

            GameObject itemGo = Instantiate(item, contents);
            itemGo.SetActive(true);
            MicItem micItem = itemGo.GetComponent<MicItem>();
            micItem.SetMic(pv);
            itemList.Add(micItem);

            idx++;
        }

        if (idx < itemList.Count)
        {
            for (int i = idx; i < itemList.Count; i++)
            {
                Destroy(itemList[idx].gameObject);
                itemList.RemoveAt(idx);
            }
        }
    }
}
