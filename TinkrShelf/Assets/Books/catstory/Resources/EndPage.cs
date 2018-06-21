using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class EndPage : ShelfManager, IPointerClickHandler {

	public void OnClick(PointerEventData eventData)
	{    
		GameObject go = eventData.pointerCurrentRaycast.gameObject;

		if (go.name == "ReadAgain" )
		{
			SceneManager.LoadScene (bookscenePath+"/Scene01");
		}
		if (go.name == "Home") 
		{SceneManager.LoadScene ("scene2");
		}
}
}
