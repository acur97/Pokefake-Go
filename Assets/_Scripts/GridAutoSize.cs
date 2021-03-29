using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridAutoSize : MonoBehaviour {

    private GridLayoutGroup grid;
    private RectTransform rect;
    private ScrollRect Srect;

    private void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        float cellS = Screen.width / 4;
        float spa = Screen.width / 28;
        grid.cellSize = new Vector2(cellS, cellS);
        grid.spacing = new Vector2(spa, spa);
        grid.padding.left = Mathf.RoundToInt(cellS / 7.2f);
        grid.padding.top = Mathf.RoundToInt(cellS / 7.2f);
    }
}
