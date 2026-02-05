using UnityEngine;

public class StageListArrow : MonoBehaviour
{
    public StageListData Data;
    public GameObject leftbutton;
    public GameObject rightbutton;
    void Start()
    {
        Debug.Log(Data.stagenum);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // 2. Fire a Raycast at that specific point
            // (Vector2.zero means we are checking exactly where the mouse is)
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            // 3. Shoot the ray and see if it hits anything
            if (hit.collider != null)
            {
                // 4. Check if the object we hit is the one in our variable
                if (hit.collider.gameObject == leftbutton)
                {Data.stagenum = Data.stagenum - 1;}
                if (hit.collider.gameObject == rightbutton)
                {Data.stagenum++;}
            }
        }
        if (Data.stagenum == 0)
        {
            leftbutton.SetActive(false);
            rightbutton.SetActive(true);
        }
        else if (Data.stagenum == Data.maxstagenum)
        {
            leftbutton.SetActive(true);
            rightbutton.SetActive(false);
        }
        else
        {
            leftbutton.SetActive(true);
            rightbutton.SetActive(true);
        }
    }
}
