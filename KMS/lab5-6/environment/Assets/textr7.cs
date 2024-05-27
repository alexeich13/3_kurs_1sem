using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textr7 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Груз, используемый как основной предмет, поднимаемый крестовиной.";
    }
}
