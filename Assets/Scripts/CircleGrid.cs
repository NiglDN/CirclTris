using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleGrid : MonoBehaviour 
{
    
    public struct SquareStruct{
        public GameObject squares;
        public bool movable; 
        public SquareStruct (GameObject squares, bool movable){
            this.squares = squares;
            this.movable = movable;
        }
    }
    public SquareStruct[] squareArray;
    float squares;
    public float res;
    float x,y = 0;
    public float radius = 0;
    public Color color = Color.red;
    private Material materialColored;
    LineRenderer lineRenderer = new LineRenderer();
    
    private void Start() {
        /*lineRenderer = gameObject.GetComponent<LineRenderer>();
        Shader shader = Shader.Find("Diffuse");
        materialColored = new Material(shader);
        materialColored.color = color;
        lineRenderer.material = materialColored;
        lineRenderer.positionCount = (80 + 1);
        lineRenderer.useWorldSpace = false;
        lineRenderer.startWidth = 0.2f;*/
        this.res = 80;
        this.squares = 40;
        squareArray = new SquareStruct[(int)squares];
    
    /*    float change = 2 * Mathf.PI / res;
        float angle = change;

        for (int i = 0; i < (res + 1); i++)
        {
            x = Mathf.Sin(angle) * (radius);
            y = Mathf.Cos(angle) * (radius);
            lineRenderer.SetPosition(i, new Vector3(x, y, 0));
            angle += change;
        }
        lineRenderer.loop = true;*/
    }

    public void spawnBlock(int index, bool movable, GameObject spawnee, int scale){
        float change = 2 * Mathf.PI / res;
        float angle = 0;
        //fscale = 0.35F;
        for (float i = 0; i < (res + 1); i++)
        {
            x = Mathf.Sin(angle) * (radius);
            y = Mathf.Cos(angle) * (radius);
            if ((i / (res / squares)) == index){
                int j = (int) (i/(res/squares));
                squareArray[j] = new SquareStruct((GameObject)Instantiate(spawnee, new Vector3(x, y, 0), new Quaternion(0,0,0,1), GameObject.Find("CircleParent").transform), movable);
                //squareArray[j].squares.transform.localScale = new Vector3(fscale, fscale, fscale);
            }
            angle += change;
        }
    }

    public bool destruction(int index) {
        if (squareArray.Length > index){
            if (!squareArray[index].Equals(default(SquareStruct))){
                Destroy(squareArray[index].squares);
                squareArray[index] = default(SquareStruct);
                return true;
            }
        } return false;
    }

    public int getLength(){
        return squareArray.Length;
    }

    public bool isMovable(int index){
        if (!squareArray[index].Equals(default(SquareStruct))){
            return squareArray[index].movable;
        }
        return false;
    }

    public bool setMovable(int index, bool movable){
        if (!squareArray[index].Equals(default(SquareStruct))){
            squareArray[index].movable = movable;
            return true;
        }
        return false;
    }

    public GameObject getSquare(int index){
        if (!squareArray[index].Equals(default(SquareStruct))){
            return squareArray[index].squares;
        }
        return null;
    }


    //check if section can be cleared, returns the cleared section
    // 0 if no section was cleared
    public int[] checkLineClear(int offset){
        bool clearLine = true;
        int temp;
        for (int i = 0; i < squareArray.Length;i++){
            if (!squareArray[i].Equals(default(SquareStruct))){
                //Debug.Log(i);
            }
        }
        int[] clearedSections = new int[10];
        for(int j = 0; j < (squareArray.Length/10);j++){
            for(int i = 0; i < 10; i++){
                temp = i+(j*10)+offset;
                if (temp >= squareArray.Length){
                    temp = temp - squareArray.Length;
                }
                if (temp < 0){
                    temp = temp + squareArray.Length;
                }
                if(squareArray[temp].Equals(default(SquareStruct))){
                    clearLine = false;
                }
            }
            if (clearLine){
                // we got a full row
                Debug.Log("Tetris");
                for(int i = 0; i < 10; i++){
                    temp = i+(j*10)+offset;
                    if (temp >= squareArray.Length){
                        temp = temp - squareArray.Length;
                    }
                    clearedSections[i] = temp;
                    destruction(temp);
                }
                return clearedSections;
            }
            clearLine = true;   
        }
        return null;
    }
}
