using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class greenRedSwapper : MonoBehaviour
{
    //SpriteRenderer sp;
    Image sp;
    public Sprite active;
    public Sprite pressed;
    public int psych;//0 for invert y axis; 1 for Is Left click enabled
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<Image>();
    }

    public void swapper()
    {   switch(psych)
        {
            case 0:
            if (Database.isYAxisInverted)
                sp.sprite = active;
            else sp.sprite = pressed;
                break;
            case 1:
            if (Database.isLeftClickEnabled)
                sp.sprite = active;
            else sp.sprite = pressed;
            break;

        }

    }
}
