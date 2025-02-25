using DG.Tweening;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] public Transform targetTrn;
    [SerializeField] public GameObject item;

    public void MarkerShow()
    {
        targetTrn.localScale = Vector3.zero;
        targetTrn.DOScale(1.0f, 0.5f);
    }

    public void MarkerHide()
    {
        targetTrn.localScale = Vector3.one;
        targetTrn.DOScale(0.0f, 0.5f);
    }

    public void Mount()
    {
        item.SetActive(true);
        MarkerHide();
    }

    public void Remove()
    {
        item.SetActive(false);
    }
}

