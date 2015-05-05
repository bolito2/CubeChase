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
    public bool[] yellowCollisions = new bool[4];
    public TextManager textManager;

    public Material wonCube;


    //Input
    public KeyCode Up, Down, Right, Left;
    public float tArrowsMax;
    public float tArrows;
    public bool isPlaced;
    RaycastHit Dhit;

    //Collider Arrays
    Collider[] Rcolliders, Dcolliders, LColliders, FColliders, BColliders, UColliders;

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

    IEnumerator MoveRight()
    {
        if (!canMove)
            yield break;
        startPos = transform.position;
        endPos = new Vector3(startPos.x + movement, transform.position.y, startPos.z);
        finalRot = Quaternion.Euler(0, 0, -90);

        foreach (Collider collider in Rcolliders)
        {
            if (collider.transform.tag == "Cube")
            {
                if (Physics.OverlapSphere(new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), 0.2f).Length > 0)
                {
                    yield break;
                }
                endPos.y++;
                foreach(Collider collider2 in UColliders)
                {
                    if(collider2.transform.tag == "Cube")
                    {
                        endPos.y--;
                        yield break;
                    }
                }
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.x - transform.position.x == 1)
                        yield break;
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

        t = 0;
    }

    IEnumerator MoveLeft()
    {
        if (!canMove)
            yield break;
        startPos = transform.position;
        endPos = new Vector3(startPos.x - movement, transform.position.y, startPos.z);
        finalRot = Quaternion.Euler(0, 0, 90);

        foreach (Collider collider in LColliders)
        {
            if (collider.transform.tag == "Cube")
            {
                endPos.y++;
                
                if(Physics.OverlapSphere(new Vector3(transform.position.x - 1, transform.position.y + 1, transform.position.z), 0.2f).Length > 0)
                {
                    yield break;
                }

                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube")
                    {
                        endPos.y--;
                        yield break;
                    }
                }
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    Debug.Log(collider.transform.position.x - transform.position.x);
                    if (collider.transform.position.x - transform.position.x == -1)
                        yield break;
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

        t = 0;
    }

    IEnumerator MoveForward()
    {
        if (!canMove)
            yield break;
        startPos = transform.position;
        endPos = new Vector3(startPos.x, transform.position.y, startPos.z + movement);
        finalRot = Quaternion.Euler(90, 0, 0);

        foreach (Collider collider in FColliders)
        {
            if (collider.transform.tag == "Cube")
            {
                if (Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 1), 0.1f).Length > 0)
                {
                    yield break;
                }
                endPos.y++;
                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube")
                    {
                        endPos.y--;
                        yield break;
                    }
                }
            }
            else
            {
                if(collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.z - transform.position.z == 1)
                        yield break;
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

        t = 0;
    }

    IEnumerator MoveBack()
    {
        if (!canMove)
            yield break;
        startPos = transform.position;
        endPos = new Vector3(startPos.x, transform.position.y, startPos.z - movement);
        finalRot = Quaternion.Euler(-90, 0, 0);

        foreach (Collider collider in BColliders)
        {
            if (collider.transform.tag == "Cube")
            {

                if (Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 1), 0.2f).Length > 0)
                {
                    yield break;
                }
                endPos.y++;
                foreach (Collider collider2 in UColliders)
                {
                    if (collider2.transform.tag == "Cube")
                    {
                        yield break;
                    }
                }
            }
            else
            {
                if (collider.transform.tag == "WallCube")
                {
                    if (collider.transform.position.z - transform.position.z == -1)
                        yield break;
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
