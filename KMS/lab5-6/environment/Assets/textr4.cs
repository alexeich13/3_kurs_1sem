using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textr4 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Содержит в себе датчик, фиксирующий груз при намотке.";

    }
}
