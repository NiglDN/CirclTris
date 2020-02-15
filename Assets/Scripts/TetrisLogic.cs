using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TetrisLogic : MonoBehaviour
{
    // number of rows
    private int circleRow;
    // squares in one row
    private int squaresPerRow;
    // lowest Postion of Current Block
    private int lowestCurrentBlock;
    // Colomn Position of falling Block
    private int[] currentBlockColumn;
    // Row Postion of falling Block
    private int[] currentBlockRow;
    // Buffering so Blocks cant have weird interactions when moving while falling
    private bool buffering = false;
    // WaitforSeconds instances
    float time = 1F;
    private WaitForSeconds onesec;
    private WaitForSeconds zeropointtwosec = new WaitForSeconds(0.1F);
    //boolean fastspeed mode
    private bool fastspeed = false;
    // currentBlockID
    private int currentBlockID = 0;
    // rotatelvl has 4 lvls
    private int rotatelvl;
    // if true game is over
    private bool gameOver = false;
    // offset for moving
    private int globalOffset = 0;
    public GameObject currBlock;
    int currentLevel = 1;
    int IsMusic;
    //delaying controls
    bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        onesec = new WaitForSeconds(time);
        circleRow = transform.childCount - 1;
        squaresPerRow = transform.GetChild(0).GetComponent<CircleGrid>().getLength();
        lowestCurrentBlock = -1;
        currentBlockColumn = new int[4];
        currentBlockRow = new int[4] { -1, -1, -1, -1 };
        IsMusic = PlayerPrefs.GetInt("Music");
        if (IsMusic == 1)
        {
            AudioListener.volume = 1;
        }
        if (IsMusic == 0)
        {
            AudioListener.volume = 0;
        }
        StartCoroutine(fallBlock());
    }

  

    // Update is called once per frame
    void Update() {
        fastspeed = false;
        if (gameOver) {
            SceneManager.LoadScene(0);
        }

        if (currentBlockColumn == null) {
            currentBlockColumn = new int[4];
            restart();
        }

        //controls
        // Touch
        foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x > Screen.width / 3 && touch.position.x <(Screen.width / 3)*2){
                        if (touch.position.y < Screen.height / 2){
                            fastspeed = true;
                        } else if (touch.position.y > Screen.height / 2 && Input.GetTouch(0).phase == TouchPhase.Began) {
                            rotateBlock();
                        }
                    }
                }
        //PC
        if (Input.GetKey(KeyCode.DownArrow)) {
            fastspeed = true;
        } 
        
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
                rotateBlock();
            }
        
        if (!wait){
            if (!buffering) {
                // Touch
                foreach (Touch touch in Input.touches)
                {
                    if (touch.position.x < Screen.width / 3)
                    {
                        globalOffset++;
                        if (globalOffset == 10) {
                            globalOffset = 0;
                        }
                        //Debug.Log(squaresPerRow / 2 + currentBlockColumn[0] + globalOffset);
                        moveBlock(1, false);
                    }
                    else if (touch.position.x > (Screen.width / 3)*2)
                    {
                        globalOffset--;
                        if (globalOffset == -10) {
                            globalOffset = 0;
                        }
                        //Debug.Log(squaresPerRow / 2 + currentBlockColumn[0] + globalOffset);
                        moveBlock(-1, true);
                    }
                }
                // PC
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    globalOffset++;
                    if (globalOffset == 10) {
                        globalOffset = 0;
                    }
                    //Debug.Log(squaresPerRow / 2 + currentBlockColumn[0] + globalOffset);
                    moveBlock(1, false);
                } else if (Input.GetKey(KeyCode.RightArrow)) {
                    globalOffset--;
                    if (globalOffset == -10) {
                        globalOffset = 0;
                    }
                    //Debug.Log(squaresPerRow / 2 + currentBlockColumn[0] + globalOffset);
                    moveBlock(-1, true);
                }
            }
            StartCoroutine(delayRoutine());
        }
    }

    IEnumerator delayRoutine()
    {
        wait = true;
        yield return new WaitForSeconds(0.2F);
        wait = false;
    }

    public void restart() {

        for (int i = 0; i <= circleRow; i++) {
            for (int j = 0; j < squaresPerRow; j++) {
                //Debug.Log(j + " " + i);
                transform.GetChild(i).GetComponent<CircleGrid>().destruction(j);
            }
        }
        circleRow = transform.childCount - 1;
        squaresPerRow = transform.GetChild(0).GetComponent<CircleGrid>().getLength();
        lowestCurrentBlock = -1;
        currentBlockColumn = new int[4];
        currentBlockRow = new int[4] { -1, -1, -1, -1 };
        buffering = false;
        fastspeed = false;
        currentBlockID = 0;
        gameOver = false;
        globalOffset = 0;
        time = 1F;
        currentLevel = 1;
        StartCoroutine(fallBlock());
    }

    //call this before moving the new blocks
    private void destroyOldBlocks() {
        // destroy old blocks
        for (int i = 0; i < currentBlockRow.Length; i++) {
            if (currentBlockRow[i] >= 0) {
                transform.GetChild(currentBlockRow[i]).GetComponent<CircleGrid>().destruction(squaresPerRow / 2 + currentBlockColumn[i]);
            }
        }
    }

    //call this after moving the new blocks
    private void createNewBlocks(GameObject spawnee) {
        for (int i = 0; i < currentBlockRow.Length; i++) {
            if (currentBlockRow[i] >= 0) {
                //Debug.Log(currentBlockColumn[0] + globalOffset + squaresPerRow/2 );
                transform.GetChild(currentBlockRow[i]).GetComponent<CircleGrid>().spawnBlock(squaresPerRow / 2 + currentBlockColumn[i], false, spawnee, i);
            }
        }
    }

    //all blocks are safed in a 4x4 matrix, beginning from left above
    private void getBlock() {
        //Debug.Log("do a nu?");
        // first x offset, then y offset
        currentBlockID = UnityEngine.Random.Range(1, 7);
        rotatelvl = 1;
        switch (currentBlockID)
        {
            case 1:
                // I-Block
                Debug.Log("spawn I-Block");
                currentBlockColumn = new int[] { 1, 0, -1, -2 };
                currentBlockRow = new int[] { -1, -1, -1, -1 };
                break;
            case 2:
                // L-Block
                Debug.Log("spawn L-Block");
                currentBlockColumn = new int[] { 1, 1, 0, -1 };
                currentBlockRow = new int[] { -1, -2, -2, -2 };
                break;
            case 3:
                // J-Block
                Debug.Log("spawn J-Block");
                currentBlockColumn = new int[] { -1, 1, 0, -1 };
                currentBlockRow = new int[] { -1, -2, -2, -2 };
                break;
            case 4:
                // O-Block
                Debug.Log("spawn Square");
                currentBlockColumn = new int[] { 0, -1, 0, -1 };
                currentBlockRow = new int[] { -1, -1, -2, -2 };
                break;
            case 5:
                // S-Block
                Debug.Log("spawn S-Block");
                currentBlockColumn = new int[] { 1, 0, 0, -1 };
                currentBlockRow = new int[] { -1, -1, -2, -2 };
                break;
            case 6:
                // T-Block
                Debug.Log("spawn T-Block");
                currentBlockColumn = new int[] { 0, 1, 0, -1 };
                currentBlockRow = new int[] { -1, -2, -2, -2 };
                break;
            case 7:
                // Z-Block
                Debug.Log("spawn Z-Block");
                currentBlockColumn = new int[] { 0, -1, 1, 0 };
                currentBlockRow = new int[] { -1, -1, -2, -2 };
                break;
            default:
                Debug.Log("Random fail");
                break;
        }
    }

    // get the block for your block
    private GameObject currentBlockObject() {
        switch (currentBlockID)
        {
            case 1:
                // I-Block

                return GameObject.Find("stoneBlue");

            case 2:
                // L-Block

                return GameObject.Find("stoneGreen");

            case 3:
                // J-Block

                return GameObject.Find("stoneLightBlue");

            case 4:
                // O-Block

                return GameObject.Find("stoneOrange");

            case 5:
                // S-Block

                return GameObject.Find("stoneRed");

            case 6:
                // T-Block

                return GameObject.Find("stoneYellow");

            case 7:
                // Z-Block

                return GameObject.Find("stonePurple");

            default:
                Debug.Log("Random fail");
                return null;

        }
    }
    public void rotateBlock() {
        int[] tempRow = new int[4];
        int[] tempColumn = new int[4];
        System.Array.Copy(currentBlockRow, tempRow, 4);
        System.Array.Copy(currentBlockColumn, tempColumn, 4);
        destroyOldBlocks();
        rotatelvl++;
        if (rotatelvl > 4) {
            rotatelvl = 1;
        }
        switch (currentBlockID)
        {
            case 1:
                // I-Block
                if ((rotatelvl % 2) == 0) {
                    currentBlockRow[0] = currentBlockRow[0] + 2;
                    currentBlockRow[1]++;
                    currentBlockRow[3]--;
                    currentBlockColumn[0]--;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3] = currentBlockColumn[3] + 2;
                } else {
                    currentBlockRow[0] = currentBlockRow[0] - 2;
                    currentBlockRow[1]--;
                    currentBlockRow[3]++;
                    currentBlockColumn[0]++;
                    currentBlockColumn[2]--;
                    currentBlockColumn[3] = currentBlockColumn[3] - 2;
                } break;
            case 2:
                // L-Block
                if (rotatelvl == 2) {
                    currentBlockRow[2]--;
                    currentBlockRow[3]--;
                    currentBlockColumn[0]--;
                    currentBlockColumn[1]--;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3]++;
                } else if (rotatelvl == 3) {
                    currentBlockRow[0]--;
                    currentBlockRow[2]++;
                    currentBlockColumn[0]++; ;
                    currentBlockColumn[2] = currentBlockColumn[2] - 2;
                    currentBlockColumn[3]--;
                } else if (rotatelvl == 4) {
                    currentBlockRow[0]++;
                    currentBlockRow[1]++;
                    currentBlockColumn[0]--;
                    currentBlockColumn[1]--;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3]++;
                } else if (rotatelvl == 1) {
                    currentBlockRow[1]--;
                    currentBlockRow[3]++;
                    currentBlockColumn[0]++;
                    currentBlockColumn[1] = currentBlockColumn[1] + 2;
                    currentBlockColumn[3]--;
                } break;
            case 3:
                // J-Block
                if (rotatelvl == 2) {
                    currentBlockRow[1]++;
                    currentBlockRow[3]--;
                    currentBlockColumn[0] = currentBlockColumn[0] + 2;
                    currentBlockColumn[1]--;
                    currentBlockColumn[3]++;
                } else if (rotatelvl == 3) {
                    currentBlockRow[0]--;
                    currentBlockRow[1]--;
                    currentBlockColumn[2]--;
                    currentBlockColumn[3]++;
                } else if (rotatelvl == 4) {
                    currentBlockRow[0]++;
                    currentBlockRow[2]--;
                    currentBlockColumn[0]--;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3] = currentBlockColumn[3] - 2;
                } else if (rotatelvl == 1) {
                    currentBlockRow[2]++;
                    currentBlockRow[3]++;
                    currentBlockColumn[0]--;
                    currentBlockColumn[1]++;
                } break;
            case 4:
                // O-Block
                break;
            case 5:
                // S-Block
                if ((rotatelvl % 2) == 0) {
                    currentBlockRow[1]--;
                    currentBlockRow[3]--;
                    currentBlockColumn[0] = currentBlockColumn[0] - 2;
                    currentBlockColumn[2]--;
                    currentBlockColumn[3]++;
                } else {
                    currentBlockRow[1]++;
                    currentBlockRow[3]++;
                    currentBlockColumn[0] = currentBlockColumn[0] + 2;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3]--;
                } break;
            case 6:
                // T-Block
                if (rotatelvl == 2) {
                    currentBlockRow[3]--;
                    currentBlockColumn[3]++;
                } else if (rotatelvl == 3) {
                    currentBlockRow[0]--;
                    currentBlockColumn[0]++;
                    currentBlockColumn[1]--;
                    currentBlockColumn[2]--;
                } else if (rotatelvl == 4) {
                    currentBlockRow[0]++;
                    currentBlockColumn[0]--;
                } else if (rotatelvl == 1) {
                    currentBlockRow[3]++;
                    currentBlockColumn[1]++;
                    currentBlockColumn[2]++;
                    currentBlockColumn[3]--;
                } break;
            case 7:
                // Z-Block
                if ((rotatelvl % 2) == 0) {
                    currentBlockRow[1]--;
                    currentBlockRow[3]--;
                    currentBlockColumn[1]++;
                    currentBlockColumn[2] = currentBlockColumn[2] - 2;
                    currentBlockColumn[3]--;
                } else {
                    currentBlockRow[1]++;
                    currentBlockRow[3]++;
                    currentBlockColumn[1]--;
                    currentBlockColumn[2] = currentBlockColumn[2] + 2;
                    currentBlockColumn[3]++;
                } break;

            default:
                Debug.Log("No Block");
                break;
        }

        //check if rotation is legal
        for (int i = 0; i < currentBlockColumn.Length; i++) {
            // if illegal then reset rotation
            if (currentBlockRow[i] < 0 || currentBlockRow[i] > circleRow) {
                //Debug.Log("THIS IS HARAM");
                currentBlockRow = tempRow;
                currentBlockColumn = tempColumn;
                rotatelvl--;
            } else if (transform.GetChild(currentBlockRow[i]).GetComponent<CircleGrid>().isMovable(squaresPerRow / 2 + currentBlockColumn[i])) {
                //Debug.Log("THIS IS ALSO HARAM");
                currentBlockRow = tempRow;
                currentBlockColumn = tempColumn;
                rotatelvl--;
            }
        }
        createNewBlocks(currentBlockObject());
    }

        IEnumerator fallBlock()
        {

            if (!fastspeed) { yield return onesec; } else { yield return zeropointtwosec; }
            buffering = true;
            for (int i = 0; i < currentBlockRow.Length; i++) {
                if (currentBlockRow[i] > lowestCurrentBlock) {
                    lowestCurrentBlock = currentBlockRow[i];
                }
            }
            // spawns a random block at start
            if (lowestCurrentBlock < 0) {
                //Debug.Log("gehst do eina?");
                getBlock();
                for (int i = 0; i < currentBlockRow.Length; i++) {
                    if (currentBlockRow[i] > lowestCurrentBlock) {
                        lowestCurrentBlock = currentBlockRow[i];
                    }
                }
                //checks if game over
                for (int i = 0; i < currentBlockRow.Length; i++) {
                    if (currentBlockRow[i] + 1 >= 0 && transform.GetChild(currentBlockRow[i] + 1).GetComponent<CircleGrid>().isMovable(squaresPerRow / 2 + currentBlockColumn[i])) {
                        Debug.Log("Game is over");
                        SceneManager.LoadScene(0);
                        gameOver = true;
                    }
                }
            }
            if (!gameOver) {
                // checks if block in next row is occupied, if so stops fall
                for (int i = 0; i < currentBlockRow.Length; i++) {
                    if (lowestCurrentBlock < circleRow && currentBlockRow[i] >= 0 && transform.GetChild(currentBlockRow[i] + 1).GetComponent<CircleGrid>().isMovable(squaresPerRow / 2 + currentBlockColumn[i])) {
                        lowestCurrentBlock = circleRow;
                    }
                }
                if (lowestCurrentBlock < circleRow) {
                    destroyOldBlocks();
                    // spawn block on next row;
                    for (int i = 0; i < currentBlockRow.Length; i++) {
                        currentBlockRow[i]++;
                    }
                    createNewBlocks(currentBlockObject());

                    buffering = false;
                    StartCoroutine(fallBlock());
                } else {
                    // block cant fall so make it movable
                    for (int i = 0; i < currentBlockRow.Length; i++) {
                        if (currentBlockRow[i] >= 0) {
                            transform.GetChild(currentBlockRow[i]).GetComponent<CircleGrid>().setMovable(squaresPerRow / 2 + currentBlockColumn[i], true);
                        }
                    }
                    // check for tetris
                    checkLineClear();
                    if (currentLevel < LevelScript.levelValue) {
                        currentLevel = LevelScript.levelValue;
                        time = time - 0.1F;
                    }
                    lowestCurrentBlock = -1;
                    currentBlockRow = new int[4] { -1, -1, -1, -1 };
                    buffering = false;
                    StartCoroutine(fallBlock());
                }
            }
        }

        public void moveBlock(int offset, bool moveRight)
        {
            CircleGrid child;
            GameObject tempBlock;
            //check for index reset
            bool reset = false;
            //check if neighbour is occopied
            bool deadEnd = false;
            if (moveRight)
            {
                //checks if right neighbour is occopied
                for (int j = 0; j < currentBlockRow.Length; j++)
                {
                    if (currentBlockRow[j] >= 0 && transform.GetChild(currentBlockRow[j]).GetComponent<CircleGrid>().isMovable((squaresPerRow / 2) + currentBlockColumn[j] + 1))
                    {
                        deadEnd = true;
                    }
                }
            }
            else
            {
                for (int j = 0; j < currentBlockRow.Length; j++)
                {
                    //checks if left neighbour is occopied
                    if (currentBlockRow[j] >= 0 && transform.GetChild(currentBlockRow[j]).GetComponent<CircleGrid>().isMovable((squaresPerRow / 2) + currentBlockColumn[j] - 1))
                    {
                        deadEnd = true;

                    }
                }
            }
            // rotate right
            if (deadEnd) {
                if (moveRight) {
                    GameObject.Find("outputSector1500").GetComponent<Backgroundrotate>().rotateLeft();
                } else {
                    GameObject.Find("outputSector1500").GetComponent<Backgroundrotate>().rotateRight();
                }
            }
            for (int i = 0; i <= circleRow; i++)
            {
                child = transform.GetChild(i).GetComponent<CircleGrid>();
                tempBlock = child.getSquare(i);
                if (moveRight)
                {
                    if (!deadEnd)
                    {
                        for (int k = 0; k < squaresPerRow; k++)
                        {
                            if (child.isMovable(k))
                            {
                                tempBlock = child.getSquare(k);
                                if (child.destruction(k))
                                {
                                    if ((offset + k) < 0)
                                    {
                                        reset = true;
                                    }
                                    else
                                    {
                                        child.spawnBlock(k + offset, true, tempBlock, i);
                                    }
                                }
                            }
                        }
                        if (reset)
                        {
                            child.spawnBlock(squaresPerRow - 1, true, tempBlock, i);
                            reset = false;
                        }
                    }
                }
                else
                {
                    if (!deadEnd)
                    {
                        for (int k = squaresPerRow - 1; k >= 0; k--)
                        {
                            if (child.isMovable(k))
                            {
                                tempBlock = child.getSquare(k);
                                if (child.destruction(k))
                                {
                                    if ((k + offset) >= squaresPerRow)
                                    {
                                        reset = true;
                                    }
                                    else
                                    {
                                        child.spawnBlock(k + offset, true, tempBlock, i);
                                    }
                                }
                            }
                        }
                        if (reset)
                        {
                            child.spawnBlock(0, true, tempBlock, i);
                            reset = false;
                        }
                    }
                }
            }
        }

        public void checkLineClear() {
            int[] clearedfields;
            int allClearedRows = 0;
            GameObject tempBlock;
            for (int i = 0; i <= circleRow; i++) {
                clearedfields = transform.GetChild(i).GetComponent<CircleGrid>().checkLineClear(globalOffset);
                if (clearedfields != null) {
                    //Debug.Log("lets make them fall");
                    for (int j = i; j > 0; j--) {
                        for (int k = 0; k < clearedfields.Length; k++) {
                            //check if block above can fall
                            tempBlock = transform.GetChild(j - 1).GetComponent<CircleGrid>().getSquare(clearedfields[k]);
                            transform.GetChild(j).GetComponent<CircleGrid>().destruction(clearedfields[k]);
                            if (transform.GetChild(j - 1).GetComponent<CircleGrid>().isMovable(clearedfields[k])) {
                                transform.GetChild(j).GetComponent<CircleGrid>().spawnBlock(clearedfields[k], true, tempBlock, j);
                            }
                        }
                    }
                    // add score
                    allClearedRows++;
                    i--;
                }
            }
            ScoreScript.updatePoints(allClearedRows);
        }
    }

