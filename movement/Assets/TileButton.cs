using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileButton : MonoBehaviour, IPointerDownHandler {
    public ButtonHighlightManager buttonHighlightManager;
    [NonSerialized] public Image frame;

    public void Awake() {
        frame = transform.Find("Frame").GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        buttonHighlightManager.OnTileClicked(this);
    }
}
