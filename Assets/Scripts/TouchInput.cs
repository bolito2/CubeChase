using UnityEngine;
using System.Collections;

public class TouchInput : MonoBehaviour {

    public Vector2 startPos;
    public Vector2 endPos;
    public PlayerMovement[] cubes;
    bool isTouched;

    void Start()
    {
        cubes = (PlayerMovement[])FindObjectsOfType(typeof(PlayerMovement));
    }

    void Update()
    {

        if (cubes == null)
            cubes = (PlayerMovement[])FindObjectsOfType(typeof(PlayerMovement));

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
                endPos = Input.mousePosition;
                isTouched = false;
            }
            if (Input.GetMouseButton(0))
            {
                endPos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
              
                if (startPos.x - endPos.x > 100 && startPos.y - endPos.y < -50 && !isTouched)
                {
                    foreach (PlayerMovement cube in cubes)
                        cube.MovePlayer("forward");
                    isTouched = true;
                }
                if (startPos.x - endPos.x < -100 && startPos.y - endPos.y > 50 && !isTouched)
                {
                    foreach(PlayerMovement cube in cubes)
                    cube.MovePlayer("back");
                    isTouched = true;
                }
                if (startPos.x - endPos.x < -100 && startPos.y - endPos.y < -50 && !isTouched)
                {
                    foreach (PlayerMovement cube in cubes)
                        cube.MovePlayer("right");
                    isTouched = true;
                }
                if (startPos.x - endPos.x > 100 && startPos.y - endPos.y > 50 && !isTouched)
                {
                    foreach (PlayerMovement cube in cubes)
                        cube.MovePlayer("left");
                    isTouched = true;
                }
                if (startPos.y - endPos.y < -100 && !isTouched)
                {
                    foreach (PlayerMovement cube in cubes)
                        cube.MoveUp();
                    isTouched = true;
                }
                if (startPos.y - endPos.y > 100 && !isTouched)
                {
                    foreach (PlayerMovement cube in cubes)
                        cube.MoveDown();
                    isTouched = true;
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
                    isTouched = false;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    endPos = touch.position;
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    
                    if (startPos.x - endPos.x > 100 && startPos.y - endPos.y < -50 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MovePlayer("forward");
                        isTouched = true;
                    }
                    if (startPos.x - endPos.x < -100 && startPos.y - endPos.y > 50 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MovePlayer("back");
                        isTouched = true;
                    }
                    if (startPos.x - endPos.x < -100 && startPos.y - endPos.y < -50 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MovePlayer("right");
                        isTouched = true;
                    }
                    if (startPos.x - endPos.x > 100 && startPos.y - endPos.y > 50 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MovePlayer("left");
                        isTouched = true;
                    }
                    if (startPos.y - endPos.y < -100 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MoveUp();
                        isTouched = true;
                    }
                    if (startPos.y - endPos.y > 100 && !isTouched)
                    {
                        foreach (PlayerMovement cube in cubes)
                            cube.MoveDown();
                        isTouched = true;
                    }
                }
            }

        }
    }
    
}
