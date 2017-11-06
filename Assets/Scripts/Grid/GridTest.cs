using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour {

    public Grid _grid;
    public GameObject tile;

	// Use this for initialization
	void Start () {

            _grid.ForEachNode((node, x, y) =>
            {
                node = Instantiate(tile);
                node.transform.position = new Vector2(x*0.2f, y*0.2f);
                return node;
            }
        );
	}

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var n = _grid.GetNeighbours(1, 1);
            for (int i = 0; i < n.Length; i++)
            {
                n[i].GetComponent<SpriteRenderer>().material.color = Color.blue;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            _grid.ForEachNode((node, x, y) =>
            {
                Destroy(node);
                node = Instantiate(tile);
                node.transform.position = new Vector2(x * 0.2f, y * 0.2f);
                return node;
            });
        }
	}
}
