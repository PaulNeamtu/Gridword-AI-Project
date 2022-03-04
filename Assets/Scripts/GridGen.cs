using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGen : MonoBehaviour
{
    public static float[,][] GridState;
    public static GameObject[,] Grid;
    public int columns = 20;
    public int rows = 20;
    [SerializeField]
    private float size = 1f;

    // Start is called before the first frame update
    void Start()
    {
        GridState = new float[rows, columns][];
        Grid = new GameObject[rows, columns];
        GenerateGridWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //method to generate a grid
    void GenerateGridWorld()
    {
        //reference grid piece objects
        GameObject referencePiece = (GameObject)Instantiate(Resources.Load("piece"));
        GameObject referenceWall = (GameObject)Instantiate(Resources.Load("wall"));
        GameObject referenceWin = (GameObject)Instantiate(Resources.Load("win"));

        //loops through however many rows and columns there are
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                //gen the win condition/goal
                if(row == 9 && column == 9)
                {
                    GameObject piece = (GameObject)Instantiate(referenceWin, transform);

                    float xPos = column * size;
                    float yPos = row * -size;

                    piece.transform.position = new Vector2(xPos, yPos);
                    float[] randVals = GenerateRands();
                    GridState[row, column] = randVals;
                    Grid[row, column] = piece;
                }

                //gen square wall
                else if(row == 4 && column == 5 || row == 5 && column == 5 || row == 4 && column == 6 
                    || row == 5 && column == 6 || row == 4 && column == 4 || row == 5 && column == 4 )
                {
                    GameObject piece = (GameObject)Instantiate(referenceWall, transform);

                    float xPos = column * size;
                    float yPos = row * -size;

                    piece.transform.position = new Vector2(xPos, yPos);
                    float[] randVals = GenerateRands();
                    GridState[row, column] = randVals;
                    Grid[row, column] = piece;
                }

                //gen straight wall
                else if (row == 8 && column == 16 || row == 9 && column == 16 || row == 10 && column == 16 
                    || row == 11 && column == 16 || row == 12 && column == 16)
                {
                    GameObject piece = (GameObject)Instantiate(referenceWall, transform);

                    float xPos = column * size;
                    float yPos = row * -size;

                    piece.transform.position = new Vector2(xPos, yPos);
                    float[] randVals = GenerateRands();
                    GridState[row, column] = randVals;
                    Grid[row, column] = piece;
                }

                //gen L-wall
                else if (row == 14 && column == 7 || row == 15 && column == 7 || row == 16 && column == 7
                    || row == 16 && column == 8 || row == 16 && column == 9 || row == 16 && column == 10)
                {
                    GameObject piece = (GameObject)Instantiate(referenceWall, transform);

                    float xPos = column * size;
                    float yPos = row * -size;

                    piece.transform.position = new Vector2(xPos, yPos);
                    float[] randVals = GenerateRands();
                    GridState[row, column] = randVals;
                    Grid[row, column] = piece;
                }

                //gen the blank grid piece
                else
                {
                    GameObject piece = (GameObject)Instantiate(referencePiece, transform);

                    float xPos = column * size;
                    float yPos = row * -size;

                    piece.transform.position = new Vector2(xPos, yPos);
                    float[] randVals = GenerateRands();
                    GridState[row, column] = randVals;
                    Grid[row, column] = piece;
                }           
            }
        }

        //clean up memory
        Destroy(referencePiece);
        Destroy(referenceWall);
        Destroy(referenceWin);

        float gridWidth = columns * size;
        float gridHight = rows * size;

        //set the board into a central position
        transform.position = new Vector2(-gridWidth / 2 + size / 2, gridHight / 2 - size / 2);
    }

    //method to instantiate 4 very small random values for each possible action at every given state
    public float[] GenerateRands()
    {
        float[] randVals;
        randVals = new float[4];
        for (int i = 0; i < 4; i++)
        {
            float rand = Random.Range(1, 999);
            rand = rand * .0001f;
            randVals[i] = rand;
        }
        return randVals;
    }
}
