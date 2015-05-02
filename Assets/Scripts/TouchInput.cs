using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {

    public Vector2 startPos;
    public Vector2 endPos;
    public PlayerMovement player;

    void Update()
    {

        if (player == null)
            player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                endPos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (startPos.x - endPos.x > 100 && startPos.y - endPos.y < -50)
                {
                    player.MovePlayer("forward");
                }
                if (startPos.x - endPos.x < -100 && startPos.y - endPos.y > 50)
                {
                    player.MovePlayer("back");
                }
                if (startPos.x - endPos.x < -100 && startPos.y - endPos.y < -50)
                {
                    player.MovePlayer("right");
                }
                if (startPos.x - endPos.x > 100 && startPos.y - endPos.y > 50)
                {
                    player.MovePlayer("left");
                }
            }
        }

        else
        {

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startPos = touch.position;
                    endPos = touch.position;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    endPos = touch.position;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    if (startPos.x - endPos.x > 100 && startPos.y - endPos.y < -50)
                    {
                        player.MovePlayer("forward");
                    }
                    if (startPos.x - endPos.x < -100 && startPos.y - endPos.y > 50)
                    {
                        player.MovePlayer("back");
                    }
                    if (startPos.x - endPos.x < -100 && startPos.y - endPos.y < -50)
                    {
                        player.MovePlayer("right");
                    }
                    if (startPos.x - endPos.x > 100 && startPos.y - endPos.y > 50)
                    {
                        player.MovePlayer("left");
                    }
                }
            }

        }
    }
}
