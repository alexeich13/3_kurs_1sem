using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Textr2 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Блок используется для уменьшения нагрузки при намотке нити .";

    }
}
