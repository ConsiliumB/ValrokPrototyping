using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class worldGen : MonoBehaviour {

    public GameObject grass;
    public bool debug;

	// Use this for initialization
	void Start () {
        Debug.Log("world gen");
        SpriteRenderer t = grass.GetComponent<SpriteRenderer>();
        Vector3 spriteSize = t.sprite.bounds.extents;

		for (int i=-9; i<10; i++)
        {
            for (int j=-9; j<10; j++)
            {   
                Instantiate(grass, TwoDToIso(new Vector2(i,j), spriteSize), new Quaternion());
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*Vector2 GetTilePos(Vector2 tilePos, Vector2 tileSize)
    {
        Vector2 tempPos = new Vector2();
        tempPos.x = tilePos.x / tileSize.x;
        tempPos.y = tilePos.y / tileSize.y;
        return tempPos;
    }*/

    Vector2 IsoTo2D(Vector2 isoPos, Vector2 tileSize)
    {
        /*
        map.x = (screen.x / TILE_WIDTH_HALF + screen.y / TILE_HEIGHT_HALF) / 2;
        map.y = (screen.y / TILE_HEIGHT_HALF - (screen.x / TILE_WIDTH_HALF)) / 2;
        */
        Vector2 cartPos = new Vector2();
        cartPos.x = (isoPos.x / tileSize.x + isoPos.y / tileSize.y) / 2;
        cartPos.y = (isoPos.y / tileSize.y - (isoPos.x / tileSize.x)) / 2;
        return cartPos;
    }

    Vector2 TwoDToIso(Vector2 cartPos, Vector2 tileSize)
    {
        Vector2 temPos = new Vector2();
        temPos.x = (cartPos.x - cartPos.y) * tileSize.x;
        temPos.y = (cartPos.x + cartPos.y) * tileSize.y;
        return temPos;
    }

    void testCordinates()
    {
        Vector2 test = TwoDToIso(new Vector2(5,5), new Vector2(1, 1));
        Vector2 backToIso = IsoTo2D(test, new Vector2(1, 1));
        Debug.Log("5 , 5 = " + backToIso);
    }
}
