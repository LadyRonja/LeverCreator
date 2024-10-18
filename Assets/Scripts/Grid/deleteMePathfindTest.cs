using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class deleteMePathfindTest : MonoBehaviour
{
    public GameObject startTile;
    public GameObject endTile;


    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            TestPath();
        }

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    void TestPath()
    {
        (int qStart, int rStart, _) = GridTile.GetCoordsFromCoordString(startTile.name);
        (int qEnd, int rEnd, _) = GridTile.GetCoordsFromCoordString(endTile.name);

        List<GridTile> path = Pathfinder.FindPath(qStart, rStart, qEnd, rEnd);

        foreach (GridTile gt in path)
        {
            GameObject.Find(gt.coords).GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
}
