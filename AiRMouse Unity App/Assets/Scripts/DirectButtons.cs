using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectButtons : MonoBehaviour
{
    public int buttonId;//0 for top, 1 for bottom, 2 for left and 3 for right
                        // Start is called before the first frame update

    public SpriteRenderer sp;
    public Sprite active;
    public Sprite pressed;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    private void OnMouseDown()
    {
        sp.sprite = pressed;
        switch (buttonId) {
            case 0:
                Database.dataBuffer[0] += 1;
                break;
            case 1:
                Database.dataBuffer[0] -= 1;
                break;
            case 2:
                Database.dataBuffer[1] -= 1;
                break;
            case 3:
                Database.dataBuffer[1] += 1;
                break;
        }

        Debug.Log("Dab raha sahi se: "+buttonId);

        
    }
    private void OnMouseUp()
    {
        sp.sprite = active;
        switch (buttonId)
        {
            case 0:
                Database.dataBuffer[0] -= 1;
                break;
            case 1:
                Database.dataBuffer[0] += 1;
                break;
            case 2:
                Database.dataBuffer[1] += 1;
                break;
            case 3:
                Database.dataBuffer[1] -= 1;
                break;
        }
    }
}
