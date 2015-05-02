using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

    public Text text;

    public void ChangeText(string NextText)
    {
        text.text = NextText;
    }

    public void FirstTouch()
    {
        switch (Application.loadedLevelName)
        {
                default :
                {
                    Debug.LogError("Level named incorrectly");
                    break;
                }
            case "Lvl1":
                {
                    break;
                }
        }
    }

    public void hasWon()
    {
        switch (Application.loadedLevelName)
        {
            default:
                {
                    Debug.LogError("Level named incorrectly");
                    break;
                }
            case "Lvl1":
                {
                    text.text = "Congratulations! Now get ready for the next level.";
                    break;
                }
            case "Lvl2":
                {
                    text.text = "Perfect! Now get ready for new types of cubes.";
                    break;
                }
        }
    }
}
