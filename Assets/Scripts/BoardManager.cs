using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public static BoardManager sharedInstance;

    //List of sprites candies
    public List<Sprite> prefabs = new List<Sprite>();
    //Selected candy
    public GameObject currentCandy;
    //matrix containing all candies
    private GameObject[,] candies;
    //Size of the board
    public int xSize, ySize;
    //Candy selected is change postion or not
    public bool ifShifting { get; set; }


    private Candy selectedCandy;
    // Start is called before the first frame update
    void Start()
    {
        //Singleton
       if(sharedInstance == null)
        {
            sharedInstance = this;
        }else
        {
            Destroy(gameObject);
        }

        //Space used by candy
        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    //Create matrix of candies
    private void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[xSize, ySize];
        float startX = this.transform.position.x;
        float startY = this.transform.position.y;
        int idx;

        //Create Board
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {                
                GameObject newCandy = Instantiate(currentCandy, 
                            new Vector3(startX + (offset.x * x),
                                        startY + (offset.y * y),
                                        0),
                                        currentCandy.transform.rotation
                                        );
                //Put name of candy in the matrix
                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);
                
                do
                {
                    idx = Random.Range(0, prefabs.Count);
                }while(( x > 0 && idx == candies[x - 1, y].GetComponent<Candy>().id) ||
                        (y > 0 && idx == candies[x, y - 1].GetComponent<Candy>().id)); 

                Sprite sprite = prefabs[idx];
                // Get random sprite from list candies
                //Sprite sprite = prefabs[Random.Range(0, prefabs.Count)];

                //Assign sprite to candy
                newCandy.GetComponent<SpriteRenderer>().sprite = sprite;
                //Assing id to candy
                newCandy.GetComponent<Candy>().id = idx;
                
                newCandy.transform.parent = this.transform;
                candies[x, y] = newCandy;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
