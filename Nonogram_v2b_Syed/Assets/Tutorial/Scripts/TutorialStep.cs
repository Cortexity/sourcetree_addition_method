using DTT.Nonogram.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TutorialStep : MonoBehaviour
{
    public Image[] circles;
    public GameObject[] highlights;
    public GameObject[] ticks;
    public int tileNo;
    public TileTemp[] tiles;

    public NonogramTutorialManager manager;

    public void ActivateTile()
    {
        for(int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].isCompulsory && !tiles[i].isActive) 
            {
                tiles[i].isActive = true;
                return;
            }
        }
    }
    public void OnClickTile(TileTemp tile)
    {
        if (tile.isCompulsory)
        {
            if (!tile.isFilled && tile.isActive)
            {
                tileNo++;
                GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueFound);
                tile.FillTile();
                if (manager.CheckRowCompletion(tiles))
                {
                    foreach (GameObject highlight in highlights)
                    {
                        highlight.SetActive(false);
                    }
                }
            }
        }
        else
        {
            GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.ClueWrong);
        }
    }
}
