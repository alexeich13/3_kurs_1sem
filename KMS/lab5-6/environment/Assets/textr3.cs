using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textr3 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Блок используется для намотки нити.";

    }
}
