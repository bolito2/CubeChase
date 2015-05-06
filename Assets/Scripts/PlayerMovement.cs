using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

   //Movement
    float t;
    Vector3 endPos;
    Vector3 startPos;
    public float speed;
    Quaternion finalRot;
    public float velocidadCaida;
    bool hasFalled;
    public float cubeDetection;
    public bool isChecked;
    bool won;
    int movement = 1;
    public bool canMove = true;
    public bool isMoving;
    public TextManager textManager;

    public Material wonCube;


    //Input
    public KeyCode Up, Down, Right, Left;
    public float tArrowsMax;
    public float tArrows;
    public bool isPlaced;
    RaycastHit Dhit;

    //Collider Arrays
    Collider[] Rcolliders, Dcolliders, LColliders, FColliders, BColliders, UColliders = new Collider[3];

    void Awake()
    {
     if(transform.tag == "YellowCube")
        {
            movement = 2;
        }
    }

    public void MovePlayer(string direction)
    {
        if(direction == "forward")
        {
            StartCoroutine(MoveForward());
        }
        if (direction == "back")
        {
            StartCoroutine(MoveBack());
        }
        if (direction == "right")
        {
            StartCoroutine(MoveRight());
        }
        if (direction == "left")
        {
            StartCoroutine(MoveLeft());
        }

    }

    void FixedUpdate()
    {
        Dcolliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), 0.2f);
        Rcolliders = Physics.OverlapSphere(new Vector3(transform.position.x + movement, transform.position.y, transform.position.z), 0.2f);
        LColliders = Physics.OverlapSphere(new Vector3(transform.position.x - movement, transform.position.y, transform.position.z), 0.2f);
        FColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + movement), 0.2f);
        BColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z - movement), 0.2f);
        UColliders = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + movement, transform.position.z), 0.2f);

        hasFalled = true;

        foreach (Collider collider in Dcolliders)
        {
            if (collider.transform.tag == "Ground" || collider.transform.tag == "Cube"|| collider.transform.tag == "WallCube" || collider.transform.tag == "Player")
            {
                hasFalled = false;
            }
            else
            {
                hasFalled = true;
            }
        }
        if (hasFalled && !won && transform.tag != "FloatingCube")
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
        }
        else
        {
            if (won)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                transform.tag = "wonCube";
                if (transform.position.x == otherCollision.transform.position.x && transform.position.z == otherCollision.transform.position.z)
                {
                    GetComponent<Renderer>().material = wonCube;
                    textManager.hasWon();
                    GameManager.instance.LevelCompleted();
                }
            }
        }
    }
    void Update()
    {
        if (!hasFalled && !won)
        {

            if (tArrows > 0)
                tArrows -= Time.deltaTime;

            if (Input.GetKey(Right))
            {
                if (tArrows <= 0)
                {
                    StartCoroutine(MoveRight());
                    tArrows = tArrowsMax;
                }
            }

            if (Input.GetKey(Left))
            {
                if (tArrows <= 0)
                {
                    StartCoroutine(MoveLeft());
                    tArrows = tArrowsMax;
                }
            }

            if (Input.GetKey(Up))
            {
                if (tArrows <= 0)
                {
                    StartCoroutine(MoveForward());
                    tArrows = tArrowsMax;
                }
            }

            if (Input.GetKey(Down))
            {
                if (tArrows <= 0)
                {
                    StartCoroutine(MoveBack());
                    tArrows = tArrowsMax;
                }
            }
        }

    }

    public bool isAgainstWall(string direction, GameObject Objective)
    {
        switch (direction)
        {
            default:
                {
                    Debug.LogError("Wrong direction in againstwall");
                    return false;
                    break;
                }
            case "left":
                {
                    if (Physics.OverlapSphere(new Vector3(Objective.transform.position.x - 1, Objective.transform.position.y + 1, Objective.transform.position.z), 0.2f).Length > 0)
                        return true;
                    else
                        return false;
                    break;
                }
            case "right":
                {
                    if (Physics.OverlapSphere(new Vector3(Objective.transform.position.x + 1, Objective.transform.position.y + 1, Objective.transform.position.z), 0.2f).Length > 0)
                        return true;
                    else
                        return false;
                    break;
                }
            case "forward":
                {
                    if (Physics.OverlapSphere(new Vector3(Objective.transform.position.x, Objective.transform.position.y + 1, Objective.transform.position.z + 1), 0.2f).Length > 0)
                        return true;
                    else
                        return false;
                    break;
                }
            case "back":
                {
                    if (Physics.OverlapSphere(new Vector3(Objective.transform.position.x, Objective.transform.position.y + 1, Objective.transform.position.z - 1), 0.2f).Length > 0)
                        return true;
                    else
                        return false;
                    break;
                }
        }

    }

    IEnumerator MoveRight()
    {
        isMoving = true;
        canMove = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x + movement, transform.position.y, startPos.z);
        finalRot = Quaternion.Euler(0, 0, -90);

        foreach (Collider collider in Rcolliders)
        {
            if (collider.transform.tag == "Cube" || collider.transform.tag == "Player" || transform.tag == "FloatingCube")
            {

                if (isAgainstWall("right", gameObject))
                {
                    canMove = false;
                    isMoving = false;
                    yield break;
                }

                if (collider.transform.tag == "Player" || collider.transform.tag == "YellowCube" || collider.transform.tag == "FloatingCube")
                {
                    if (isAgainstWall("right", collider.transform.gameObject))
                        endPos.y++;
                }
                else endPos.y++;

                foreach (Collider collider2 in UColliders)
                {
                    if(collider2.transform.tag == "Cube" || collider2.transform.tag == "FloatingCube" || collider2.transform.tag == "Player" )
                    {
                        endPos.y--;
                        isMoving = false;
                        yield break;
                    }
                }
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.x - transform.position.x == 1)
                    {
                        isMoving = false;
                        yield break;
                    }
                    else
                        endPos.x--;
                }
            }

        }
        if(transform.tag == "YellowCube")
        {
            Collider[] RColliders2 = Physics.OverlapSphere(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), 0.2f); 
            foreach(Collider YCollider in RColliders2)
            {
                if(YCollider.transform.tag == "Player")
                {
                    if (YCollider.transform.position.y == transform.position.y)
                    {
                        endPos.y++;
                    }
                }
                if (YCollider.transform.tag == "WallCube")
                {
                    isMoving = false;
                    yield break;
                }
            }
        }


        while (endPos != transform.position)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.identity, finalRot, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        canMove = true;
        isMoving = false;
        t = 0;
    }

    IEnumerator MoveLeft()
    {
        isMoving = true;
        canMove = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x - movement, transform.position.y, startPos.z);
        finalRot = Quaternion.Euler(0, 0, 90);

        foreach (Collider collider in LColliders)
        {
            if (collider.transform.tag == "Cube" || collider.transform.tag == "Player" || transform.tag == "FloatingCube")
            {

                if (isAgainstWall("left", gameObject))
                {
                    canMove = false;
                    isMoving = false;
                    yield break;
                }
                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube" || collider2.transform.tag == "FloatingCube" || collider2.transform.tag == "Player")
                    {
                        endPos.y--;
                        isMoving = false;
                        yield break;
                    }
                }
                if (collider.transform.tag == "Player" || collider.transform.tag == "YellowCube" || collider.transform.tag == "FloatingCube")
                {
                        if (isAgainstWall("left", collider.transform.gameObject))
                            endPos.y++;
                }
                else endPos.y++;
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    Debug.Log(collider.transform.position.x - transform.position.x);
                    if (collider.transform.position.x - transform.position.x == -1)
                    {
                        isMoving = false;
                        yield break;
                    }
                    else
                        endPos.x++;
                }
            }
        }

        if (transform.tag == "YellowCube")
        {
            Collider[] LColliders2 = Physics.OverlapSphere(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), 0.2f);
            foreach (Collider YCollider in LColliders2)
            {
               
                if (YCollider.transform.tag == "Player")
                {
                    if (YCollider.transform.position.y == transform.position.y)
                    {
                        endPos.y++;
                    } 
                }

                if (YCollider.transform.tag == "WallCube")
                {
                    isMoving = false;
                    yield break;
                }
            }
        }

        while (endPos != transform.position)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.identity, finalRot, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        canMove = true;
        isMoving = false;
        t = 0;
    }

    IEnumerator MoveForward()
    {
        isMoving = true;
        canMove = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x, transform.position.y, startPos.z + movement);
        finalRot = Quaternion.Euler(90, 0, 0);

        foreach (Collider collider in FColliders)
        {
            if (collider.transform.tag == "Cube" || collider.transform.tag == "Player" || transform.tag == "FloatingCube")
            {
                if (isAgainstWall("forward", gameObject))
                {
                    canMove = false;
                    isMoving = false;
                    yield break;
                }
                if (collider.transform.tag == "Player" || collider.transform.tag == "YellowCube" || collider.transform.tag == "FloatingCube")
                {
                    if (isAgainstWall("forward", collider.transform.gameObject))
                        endPos.y++;
                }
                else endPos.y++;
                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube" || collider2.transform.tag == "FloatingCube")
                    {
                        endPos.y--;
                        isMoving = false;
                        yield break;
                    }
                }
            }
            else
            {
                if(collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.z - transform.position.z == 1)
                    {
                        isMoving = false;
                        yield break;
                    }
                    else
                        endPos.z--;
                }
            }
            
        }

        if (transform.tag == "YellowCube")
        {
            Collider[] FColliders2 = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1), 0.2f);
            foreach (Collider YCollider in FColliders2)
            {
                if (YCollider.transform.tag == "Player")
                {
                    if (YCollider.transform.position.y == transform.position.y)
                    {
                        endPos.y++;
                    }
                }
                if (YCollider.transform.tag == "WallCube")
                {
                    isMoving = false;
                    yield break;
                }
            }
        }

        while (endPos != transform.position)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.identity, finalRot, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        canMove = true;
        isMoving = false;
        t = 0;
    }

    IEnumerator MoveBack()
    {
        isMoving = true;
        canMove = true;
        startPos = transform.position;
        endPos = new Vector3(startPos.x, transform.position.y, startPos.z - movement);
        finalRot = Quaternion.Euler(-90, 0, 0);

        foreach (Collider collider in BColliders)
        {
            if (collider.transform.tag == "Cube" || collider.transform.tag == "Player" || collider.transform.tag == "YellowCube" || collider.transform.tag == "FloatingCube")
            {

                if (isAgainstWall("back", gameObject))
                {
                    isMoving = false;
                    canMove = false;
                    yield break;
                }
                if (collider.transform.tag == "Player" || collider.transform.tag == "YellowCube" || collider.transform.tag == "FloatingCube")
                {
                    if (isAgainstWall("back", collider.transform.gameObject))
                          endPos.y++;        
                }
                else endPos.y++;
                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube" || collider2.transform.tag == "FloatingCube")
                    {
                        isMoving = false;
                        yield break;
                    }
                }
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.z - transform.position.z == -1)
                    {
                        isMoving = false;
                        yield break;
                    }
                    else
                        endPos.z++;
                }
            }
        }

        if (transform.tag == "YellowCube")
        {
            Collider[] BColliders2 = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1), 0.2f);
            foreach (Collider YCollider in BColliders2)
            {
                if (YCollider.transform.tag == "Player")
                {
                    if (YCollider.transform.position.y == transform.position.y)
                    {
                        endPos.y++;
                    }
                }
                if (YCollider.transform.tag == "WallCube")
                {
                    isMoving = false;
                    yield break;
                }
            }
        }

        while (endPos != transform.position)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.identity, finalRot, t);
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        canMove = true;
        isMoving = false;
        t = 0;
    }

    Collision otherCollision;

    void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "LevelEnd" && transform.tag != "YellowCube")
        {
            won = true;
            otherCollision = other;
        }
    }

    public void MoveUp()
    {
        if (transform.tag == "FloatingCube")
        {
            foreach(Collider collider in UColliders)
            {
                if (collider.tag == "Cube" || collider.tag == "Ground" || collider.tag == "Player" || transform.tag == "YellowCube")
                {
                    return;
                }
            }
            foreach (Collider collider in Dcolliders)
            {
                if (collider.tag == "Cube" || collider.tag == "Ground")
                {
                        StartCoroutine(moveUp());
                        break;
                }
            }
        }
    }

    IEnumerator moveUp()
    {
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        while(transform.position != endPos)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        t = 0;
    }

    public void MoveDown()
    {
        if(transform.tag == "FloatingCube")
        {
            foreach (Collider collider in Dcolliders)
            {
                if (collider.tag == "Cube" || collider.tag == "Ground")
                {
                    return;
                }
            }
            StartCoroutine(moveDown());
        }
    }

    Vector3 groundPos;
    IEnumerator moveDown()
    {
        startPos = transform.position;
        if (Physics.Raycast(transform.position, -Vector3.up, out Dhit))
        {
            groundPos = new Vector3(Dhit.transform.position.x, Dhit.transform.position.y + 1, Dhit.transform.position.z);
        }
        else
            Debug.Log("No Ground");

        startPos = transform.position;
        endPos = new Vector3(transform.position.x, groundPos.y, transform.position.z);

        while(transform.position != endPos)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime * speed;
            yield return null;
        }
        t = 0;
    }
}
