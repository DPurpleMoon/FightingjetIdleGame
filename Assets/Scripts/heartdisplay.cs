using UnityEngine;
using UnityEngine.UI;

public class heartdisplay :MonoBehaviour
{
    public int maxHearts;
    public int currentHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


void Update()
{
    for (int i = 0; i < hearts.Length; i++)
    {
        if (i < currentHearts)
        {
            hearts[i].sprite = fullHeart;
        }
        else
        {
            hearts[i].sprite = emptyHeart;
        }

        if (i < maxHearts)
        {
            hearts[i].enabled = true;
        }
        else
        {
            hearts[i].enabled = false;
        }
    }
}
}