﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameObjectClass  {
    public int id;
    public float posX;
    public float posY;
    public float scaleX;
    public float scaleY;
    public int orderInLayer;
    public string imageName;
    public bool inText;
    public string label;
    public bool draggable;
    public Anim[] anim;

}