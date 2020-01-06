using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIndicator : MonoBehaviour
{
    public UnityEngine.UI.Image root;
    public UnityEngine.UI.Image indicator;
    public Transform player;

    private void Update()
    {
        if (player)
        {
            Vector2 wantedPos = Camera.main.WorldToScreenPoint(player.position);
            root.rectTransform.position = wantedPos;
        }
        
    }

    public void SetHealth(float value)
    {
        float val = Mathf.Clamp(value, 0, 100f);
        indicator.rectTransform.sizeDelta = new Vector2((val / 100) * root.rectTransform.sizeDelta.x, indicator.rectTransform.sizeDelta.y);
    }
   
}
