using DTT.Nonogram;
using DTT.Nonogram.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialHint : MonoBehaviour
{
    public Text _text;
    public TileTemp _tile;
    public GameObject hint;
    public NonogramTutorialManager _manager;

    public void UseHint()
    {
        GridRenderer.instance.Audio.PlayOneShot(GridRenderer.instance.Hint);
        hint.SetActive(true);
        _tile.isActive = true;
        _text.text = "0";
        _manager.StopAnimateHand();
    }
}
