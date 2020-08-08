using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRClicks : MonoBehaviour
{
    /// <summary>
    /// This script controls the Left and Right Clicks
    /// </summary>
    SpriteRenderer sp;
    public Sprite active;
    public Sprite pressed;
    public int clickId; //0 for Left Click, 1 for Right Click
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {

        if (clickId == 0 && Database.isLeftClickEnabled)
        {
            sp.sprite = pressed;
            Database.dataBuffer[4] = 1;
            Handheld.Vibrate();
        }
        else if (clickId == 1)
        { sp.sprite = pressed; Database.dataBuffer[5] = 1; }

        Debug.Log("Click hua: "+clickId);
    }
    private void OnMouseUp()
    {
        sp.sprite = active;
        if (clickId == 0 && Database.isLeftClickEnabled)
            Database.dataBuffer[4] = 0;
        else if (clickId == 1)
            Database.dataBuffer[5] = 0;
    }
}
