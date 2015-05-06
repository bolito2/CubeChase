using UnityEngine;
using System.Collections;

public class LevelEditor : MonoBehaviour {

    Vector3 blockHitPos;
    RaycastHit blockHit;
    public bool isEditing;
    public Transform Level;
    public GameObject LevelPrefab;

    public GameObject CubePrefab;

    void Start()
    {
        GenerateGround();
    }

    void OnLevelWasLoaded()
    {
        GenerateGround();
    }

    void GenerateGround()
    {
        if(Level == null)
        {
            GameObject level = Instantiate(LevelPrefab) as GameObject;
            Level = level.transform;
        }
        for (int x = -5; x <= 6; x++)
        {
            for (int z = -5; z <= 6; z++)
            {
                GameObject cube = Instantiate(CubePrefab, new Vector3(x, -1, z), Quaternion.identity) as GameObject;
                cube.transform.parent = Level;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (isEditing)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(new Vector3(blockHitPos.x, blockHitPos.y, blockHitPos.z), Vector3.one);
        }
    }

    public GameObject cube;

    void Update()
    {
        if (isEditing)
        {
            if (CheckCollision(out blockHit))
            {
                blockHitPos = blockHit.transform.position + blockHit.normal;
                cube = blockHit.transform.gameObject;

                if (Input.GetMouseButtonDown(0))
                    Instantiate(CubePrefab, new Vector3(blockHitPos.x, blockHitPos.y, blockHitPos.z), Quaternion.identity);

            }
        }
        
    }

    public bool CheckCollision(out RaycastHit hitPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit noHit = new RaycastHit();
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit))
        {
            hitPoint = hit;
            return true;
        }
        else
        {
            hitPoint = noHit;
            return false;
        }
    }
}
