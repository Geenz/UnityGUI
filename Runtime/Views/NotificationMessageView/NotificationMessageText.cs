﻿using System.Collections;
using UnityEngine;
using TMPro;
using Gameframe.GUI.Pooling;
using Gameframe.GUI.Tween;

namespace Gameframe.GUI
{
  
  public class NotificationMessageText : PoolableGameObject
  {
    
    [SerializeField]
    private TextMeshProUGUI text;
  
    [SerializeField]
    private float moveDuration = 0.5f;
  
    [SerializeField]
    private float stayDuration = 1.5f;
  
    [SerializeField]
    private float fadeDelay = 0.75f;
  
    [SerializeField]
    private float fadeDuration = 0.5f;
  
    private int bumps;

    private RectTransform RectTransform => transform as RectTransform;
    
    private NotificationMessageView messageView;
    
    public void Message(string messageText)
    {
      this.Message(messageText, Color.white);
    }
    
    public void Message(string messageText, Color color)
    {
      text.text = messageText;
      RectTransform.anchoredPosition = new Vector2(0, -RectTransform.sizeDelta.y);
      RectTransform.DoAnchorPosY(0, moveDuration);
      text.color = new Color(1,1,1,0);
      text.DoColor(color, moveDuration);
      StartCoroutine(Fade());
    }
  
    public void BumpUp()
    {
      bumps += 1;
      RectTransform.DoAnchorPosY(bumps * RectTransform.sizeDelta.y, moveDuration);
    }
  
    private IEnumerator Fade()
    {
      float time = 0;
      while ( time < stayDuration && bumps <= 0 )
      {
        time += Time.deltaTime;
        yield return null;
      }
      yield return new WaitForSeconds(fadeDelay);
      text.DoColor(Color.clear, fadeDuration);
      BumpUp();
      yield return new WaitForSeconds(fadeDuration);
      Despawn();
    }
    
    public override void OnPoolableSpawned()
    {
      base.OnPoolableSpawned();
      messageView = GetComponentInParent<NotificationMessageView>();
    }
  
    public override void OnPoolableDespawn()
    {
      base.OnPoolableDespawn();
      bumps = 0;
      RectTransform.DoKillTweens();
      RectTransform.anchoredPosition = new Vector2(0, -80);
      if (messageView != null)
      {
        messageView.RemoveText(this);
      }
    }
  
  }
  
}

