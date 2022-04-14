using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBLock : MonoBehaviour
{
    public Vector3 rotationPoint;

    private float previousTime;
    public float fallTime = 1.0f;

    public static int point = 0;
    public static int height = 20;
    public static int width = 10;
    private static Transform[,] grid = new Transform[width, height];

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(-1, 0, 0);
            }

        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(1, 0, 0);
            }
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            while (ValidMove())
            {
                transform.position += new Vector3(0, -1, 0);
            }
            check();
        }
        else if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                check();
            }
            previousTime = Time.time;
        }
    }

    void check()
    {
        transform.position -= new Vector3(0, -1, 0);
        AddToGrid();
        CheckForLines();
        this.enabled = false;
        if (transform.position == new Vector3(5, 17, 0))
        {
            return;
        }
        FindObjectOfType<spawnertetris>().NewTetris();
    }
    
    bool HasLine(int i) 
    {
        for (int j = 0; j < width; j++)  
        {
            if (grid[j, i] == null)  
                return false;    
        }
        return true;  
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
        point++;
    }

    void RowDown(int i)  
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null) 
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i)) 
            {
                DeleteLine(i);
                RowDown(i);  
            }
        }
    }
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundX, roundY] = children;
        }
    }

    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundX = Mathf.RoundToInt(children.transform.position.x);
            int roundY = Mathf.RoundToInt(children.transform.position.y);
            
            if (roundX < 0 || roundX >= width || roundY < 0 || roundY >= height)
            {
                return false;
            }

            if (grid[roundX, roundY] != null)
                return false;
        }

        return true;
    }
}