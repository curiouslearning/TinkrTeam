using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class test : MonoBehaviour {

	

    void Start()
    {
    }


    //public void CreateText(float x, float y, string textToPrint)
    //{
    //    GameObject UItextGO = new GameObject("Text_" + textToPrint);
    //    //UItextGO.transform.SetParent(parent.transform);

    //    Text text = UItextGO.AddComponent<Text>();

    //    text.text = textToPrint;
    //    text.fontSize = 20;
    //    text.transform.localScale = new Vector3(1, 1, 1);

    //    //used for fitting the text box to the size of text.
    //    ContentSizeFitter csf = UItextGO.AddComponent<ContentSizeFitter>();
    //    csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    //    csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

    //    VerticalLayoutGroup vlg = UItextGO.AddComponent<VerticalLayoutGroup>();

    //    vlg.childControlHeight = true;
    //    vlg.childControlWidth = true;


    //    RectTransform trans = UItextGO.GetComponent<RectTransform>();
    //    text.alignment = TextAnchor.UpperLeft;
    //    trans.anchoredPosition = new Vector3(x, y, 0);
    //    UItextGO.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
    //    UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(trans);

    //    trans.anchoredPosition = new Vector3(x + trans.rect.width / 2, y, 0);
    //    //Debug.log(trans.rect.width + ",," + x);

    //    UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(trans);

    //    width = width + trans.rect.width + 10f;

    //}


}

