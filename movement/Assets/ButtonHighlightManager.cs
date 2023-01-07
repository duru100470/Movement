using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonHighlightManager : MonoBehaviour {
    [NonSerialized] public List<TileButton> tileButtons;
    public void Awake() {
        tileButtons = GetComponentsInChildren<TileButton>().ToList();
    }
    public void OnTileClicked(TileButton button) {
        foreach (var tileButton in tileButtons) 
            tileButton.frame.enabled = button == tileButton;
    }
}
