using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAI : MonoBehaviour
{
    public GameObject player;
    public static float[,][] eTable;
    public int columns = 20;
    public int rows = 20;
    int episodes;
    bool explore;
    float epsilon;
    float delta;
    float lambda;

    int wins;
    public Text winsTxt;
    int eps;
    public Text epsTxt;

    // Start is called before the first frame update
    void Start()
    {
        lambda = .9f;
        eTable = new float[rows, columns][];
        episodes = 1200;
        epsilon = .95f;

        wins = 0;
        eps = 0;

        //start the AI in a coroutine so we can see the agent move very quickly rather than have everything update in 1 step
        StartCoroutine("AgentMovement");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator AgentMovement()
    {
        //wait a second so we can be sure the grid loaded in before we set the agent on it
        yield return new WaitForSeconds(1);

        //loop that repeats for each episode
        while (episodes > 0)
        {
            //agent starts at a random position on the board
            int rowPos = Random.Range(0, 20);
            int colPos = Random.Range(0, 20);
            int currRow = rowPos;
            int currCol = colPos;
            int nextRow = 0;
            int nextCol = 0;

            //load the agent (in this case it is named player) and create a reference to it so we can edit it without editing the prefab
            GameObject referencePlayer = (GameObject)Instantiate(Resources.Load("player"));
            referencePlayer.transform.position = GridGen.Grid[currRow, currCol].transform.position;
            GameObject orig = (GameObject)Instantiate(referencePlayer, transform);

            //instantiate values of e-table to all 0s (required each time the agent is reset)
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    float[] vals;
                    vals = new float[4];
                    for (int k = 0; k < 4; k++)
                    {
                        vals[k] = 0;
                    }
                    eTable[i, j] = vals;
                }
            }

            //repeats until state is terminal
            while (true)
            {
                //time between each agent movement (very fast but cool to look at)
                yield return new WaitForSeconds(.0001f);
                int move = 0;
                float currMoveScore = -10000;

                //epsilon greedy algorithm (if a random value p is less than or equal to epsilon, we explore, otherwise we exploit)
                float p = Random.value;
                if (p <= epsilon)
                {
                    explore = true;
                }
                else
                {
                    explore = false;
                }

                //exploration policy (the action the agent takes is chosen randomly)
                if (explore == true)
                {
                    move = Random.Range(0, 4);
                }

                //exploitation policy (the action the agent takes is chosen based on the move with the highest score)
                if (explore == false)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float moveScore = GridGen.GridState[currRow, currCol][i];

                        if (moveScore > currMoveScore)
                        {
                            move = i;
                            currMoveScore = moveScore;
                        }
                    }
                }

                //take action 0 which is move up
                if (move == 0)
                {
                    //if action is terminal we break the loop otherwise we move the agent there
                    if (currRow - 1 < 0)
                    {
                        Destroy(referencePlayer);
                        nextRow = currRow;
                        nextCol = currCol;
                        delta = getDelt(currRow, currCol, nextRow, nextCol, -1, move);
                        adjustTable(delta);
                        break;
                    }
                    //spawn agent at the move they are going to take (useful for watching the agent move)
                    referencePlayer.transform.position = GridGen.Grid[currRow - 1, currCol].transform.position;
                    GameObject piece = (GameObject)Instantiate(referencePlayer, transform);
                    nextRow = currRow - 1;
                    nextCol = currCol;
                    delta = getDelt(currRow, currCol, nextRow, nextCol, 0, move);
                    adjustTable(delta);
                }

                //take action 1 which is move righy
                if (move == 1)
                {
                    //if action is terminal we break the loop otherwise we move the agent there
                    if (currCol + 1 > 19)
                    {
                        Destroy(referencePlayer);
                        nextCol = currCol;
                        nextRow = currRow;
                        delta = getDelt(currRow, currCol, nextRow, nextCol, -1, move);
                        adjustTable(delta);
                        break;
                    }
                    //spawn agent at the move they are going to take (useful for watching the agent move)
                    referencePlayer.transform.position = GridGen.Grid[currRow, currCol + 1].transform.position;
                    GameObject piece = (GameObject)Instantiate(referencePlayer, transform);
                    nextCol = currCol + 1;
                    nextRow = currRow;
                    delta = getDelt(currRow, currCol, nextRow, nextCol, 0, move);
                    adjustTable(delta);
                }

                //take action 2 which is move down
                if (move == 2)
                {
                    //if action is terminal we break the loop otherwise we move the agent there
                    if (currRow + 1 > 19)
                    {
                        Destroy(referencePlayer);
                        nextRow = currRow;
                        nextCol = currCol;
                        delta = getDelt(currRow, currCol, nextRow, nextCol, -1, move);
                        adjustTable(delta);
                        break;
                    }
                    //spawn agent at the move they are going to take (useful for watching the agent move)
                    referencePlayer.transform.position = GridGen.Grid[currRow + 1, currCol].transform.position;
                    GameObject piece = (GameObject)Instantiate(referencePlayer, transform);
                    nextRow = currRow + 1;
                    nextCol = currCol;
                    delta = getDelt(currRow, currCol, nextRow, nextCol, 0, move);
                    adjustTable(delta);
                }

                //take action 3 which is move left
                if (move == 3)
                {
                    //if action is terminal we break the loop otherwise we move the agent there
                    if (currCol - 1 < 0)
                    {
                        Destroy(referencePlayer);
                        nextCol = currCol;
                        nextRow = currRow;
                        delta = getDelt(currRow, currCol, nextRow, nextCol, -1, move);
                        adjustTable(delta);
                        break;
                    }
                    //spawn agent at the move they are going to take (useful for watching the agent move)
                    referencePlayer.transform.position = GridGen.Grid[currRow, currCol - 1].transform.position;
                    GameObject piece = (GameObject)Instantiate(referencePlayer, transform);
                    nextCol = currCol - 1;
                    nextRow = currRow;
                    delta = getDelt(currRow, currCol, nextRow, nextCol, 0, move);
                    adjustTable(delta);
                }

                //terminal states of hitting a wall or winning also break the loop
                if (GridGen.Grid[nextRow, nextCol].tag == "wall")
                {
                    Destroy(referencePlayer);
                    delta = getDelt(currRow, currCol, nextRow, nextCol, -1, move);
                    adjustTable(delta);
                    break;
                }
                if (GridGen.Grid[nextRow, nextCol].tag == "win")
                {
                    Destroy(referencePlayer);
                    delta = getDelt(currRow, currCol, nextRow, nextCol, 1, move);
                    adjustTable(delta);
                    wins++;
                    break;
                }


                //set current row and column to next row and column (move the player to the next state)
                currCol = nextCol;
                currRow = nextRow;
            }
            //decrement episodes by 1
            episodes -= 1;
            epsTxt.text = eps.ToString();
            eps++;

            //epsilon decay so we exploit more over time
            if (epsilon > .05)
            {
                epsilon = epsilon - .00075f;
            }
            print("epsilon: " + epsilon);

            //methods to replace the arrows and delete the current agents path
            destroyArrows();
            placeArrows();
            destroyAgents();

            //edit number of wins text
            winsTxt.text = wins.ToString();
        }
        //end coroutine
        yield break;
    }


    //method to get the delta value
    public float getDelt(int currRow, int currCol, int nextRow, int nextCol, int reward, int move)
    {
        float gamma = .9f;
        //delta is modified by the reward, the gamma rate and the 2 state action pairs for the current move and the next move
        float delt = reward + gamma * (GridGen.GridState[nextRow, nextCol][move] - GridGen.GridState[currRow, currCol][move]);
        //eligibility trace for current move is set to 1
        eTable[currRow, currCol][move] = 1;

        return delt;
    }

    public void adjustTable(float delta)
    {
        //loop through all state action pairs
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                for (int action = 0; action < 4; action++)
                {
                    //adjust Q-Table
                    GridGen.GridState[row, col][action] += .1f * delta * eTable[row, col][action];
                    //decaying e-Table
                    eTable[row, col][action] = .9f * lambda * eTable[row, col][action];
                }
            }
        }
    }

    //method to place arrows on the board and size them according to their importance
    public void placeArrows()
    {
        //create reference for all 4 arrows
        GameObject UpArrow = (GameObject)Instantiate(Resources.Load("Uarrow"));
        GameObject RightArrow = (GameObject)Instantiate(Resources.Load("Rarrow"));
        GameObject DownArrow = (GameObject)Instantiate(Resources.Load("Darrow"));
        GameObject LeftArrow = (GameObject)Instantiate(Resources.Load("Larrow"));
        int arrType = 0;

        //loop throug all state action pairs
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float maxScore = -1000000;
                int move = 0;
                for (int action = 0; action < 4; action++)
                {
                    float score = GridGen.GridState[row, col][action];
                    //find max score for each state
                    if (score > maxScore)
                    {
                        maxScore = score;
                        arrType = action;
                        move = action;
                    }
                }

                //spawn arrow on grid and size in proportion to the value of the best action at that point
                if (arrType == 0)
                {
                    UpArrow.transform.position = GridGen.Grid[row, col].transform.position;
                    UpArrow.transform.localScale = new Vector3(.4f + GridGen.GridState[row, col][move] * .7f, .4f + GridGen.GridState[row, col][move] * .7f, 1);
                    GameObject orig = (GameObject)Instantiate(UpArrow, transform);
                }
                if (arrType == 1)
                {
                    RightArrow.transform.position = GridGen.Grid[row, col].transform.position;
                    RightArrow.transform.localScale = new Vector3(.4f + GridGen.GridState[row, col][move] * .7f, .4f + GridGen.GridState[row, col][move] * .7f, 1);
                    GameObject orig = (GameObject)Instantiate(RightArrow, transform);
                }
                if (arrType == 2)
                {
                    DownArrow.transform.position = GridGen.Grid[row, col].transform.position;
                    DownArrow.transform.localScale = new Vector3(.4f + GridGen.GridState[row, col][move] * .7f, .4f + GridGen.GridState[row, col][move] * .7f, 1);
                    GameObject orig = (GameObject)Instantiate(DownArrow, transform);
                }
                if (arrType == 3)
                {
                    LeftArrow.transform.position = GridGen.Grid[row, col].transform.position;
                    LeftArrow.transform.localScale = new Vector3(.4f + GridGen.GridState[row, col][move] * .7f, .4f + GridGen.GridState[row, col][move] * .7f, 1);
                    GameObject orig = (GameObject)Instantiate(LeftArrow, transform);
                }
            }
        }
    }

    //method to destroy arrows on board (needed so we can update arrow directions at the end of each agents lifespan
    public void destroyArrows()
    {
        GameObject[] allArrows = GameObject.FindGameObjectsWithTag("arrow");
        foreach (GameObject arr in allArrows)
        {
            Destroy(arr);
        }
    }

    //method to destroy agents on the board (needed so we can clear up the agents path at the end of their lifespan)
    public void destroyAgents()
    {
        GameObject[] allAgents = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject agent in allAgents)
        {
            Destroy(agent);
        }
    }
}
