using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class swapSprite : MonoBehaviour
{
    //SpriteRenderer sp;
    Image sp;
    public Sprite active;
    public Sprite pressed;
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<Image>();
    }

    public void swapper()
    {
        
            if (Database.isAirMouseOn)
                sp.sprite = active;
            else sp.sprite = pressed;
        
        
    }
}
