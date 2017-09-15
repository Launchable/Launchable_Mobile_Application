using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactButton : MonoBehaviour
{
    public TargetTracker tracker;
    public int contactButtonIndex = 0;

    public Sprite[] defaultSprites;
    Image myImage;

    private void Start()
    {
        myImage = this.GetComponent<Image>();
    }

    public void SetButtonType(int index)
    {
        contactButtonIndex = index;
        myImage.sprite = defaultSprites[contactButtonIndex];
    }

    public void OpenContact()
    {
        switch (contactButtonIndex)
        {
            case 0:
                Application.OpenURL("mailto:" + tracker.GetTargetEmail());
                break;
            case 1:
                Application.OpenURL("tel://" + tracker.GetTargetPhone());
                break;
            case 2:
                Application.OpenURL(tracker.GetTargetWeb());
                break;
        }
    }
}
