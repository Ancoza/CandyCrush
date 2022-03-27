using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{

    private static Color selectedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    private static Candy previousSelected = null;

    private SpriteRenderer spriteRenderer;

    // To know if the candy is selected or not
    private bool isSelected = false;

    public int id;

    //Candy adjacent positions
    private Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void SelectedCandy(){
        isSelected = true;
        spriteRenderer.color = selectedColor;
        previousSelected = gameObject.GetComponent<Candy>();
    }

    private void DeselectedCandy(){
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelected = null;
    }

    private void OnMouseDown()
    {
        if(spriteRenderer.sprite == null || BoardManager.sharedInstance.ifShifting)
        {
            return;
        }
        
        if (isSelected)
        {
            DeselectedCandy();
        }
        else
        {
            if (previousSelected == null)
            {
                SelectedCandy();
            }
            else{
                if(CanSwipe())
                {
                    SwapSprite(previousSelected);
                    previousSelected.DeselectedCandy();
                }
                else{
                    previousSelected.DeselectedCandy();
                    SelectedCandy();
                }
            }
        
        }
    }

    public void SwapSprite(Candy newCandy){
        
        if(spriteRenderer.sprite == newCandy.GetComponent<SpriteRenderer>().sprite)
        {
            return;
        }

        //Swap sprite
        Sprite oldCandy = newCandy.spriteRenderer.sprite;
        newCandy.spriteRenderer.sprite = this.spriteRenderer.sprite;
        this.spriteRenderer.sprite = oldCandy;
        //Swap id
        int oldId = newCandy.id;
        newCandy.id = this.id;
        this.id = oldId;

    }

    private GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction);
        
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    private List<GameObject> GetNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();
        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    private bool CanSwipe(){
        return GetNeighbors().Contains(previousSelected.gameObject);
    }
}
