using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
	[SerializeField] TMP_Text title_text;
	[SerializeField] TMP_Text toolTip_text;
	[SerializeField] float start_anchoredPos = 325.0f;
	[SerializeField] float end_anchoredPos;
	
	private CanvasGroup canvas;
	private RectTransform rect;

	private bool isOpen;
	
	private void Awake()
	{
		canvas = GetComponent<CanvasGroup>();
		rect = GetComponent<RectTransform>();
		//rect.anchoredPosition = new Vector2(start_anchoredPos, rect.anchoredPosition.y);
	}

	public void SetTitle(string title)
	{
		if (title_text.text != title)
			title_text.text = title;
	}

	public void SetTooltip(string text)
	{
		if (string.IsNullOrEmpty(text))
			Close();
		else
		{
			toolTip_text.text = text;
			Open();
		}
	}

	public void Open()
	{
		canvas.DOFade(1, 0.25f);
		rect.DOAnchorPosX(end_anchoredPos, 0.25f);
	}

	public void Close()
	{
		canvas.DOFade(0, 0.25f);
		rect.DOAnchorPosX(start_anchoredPos, 0.25f);
	}
}
