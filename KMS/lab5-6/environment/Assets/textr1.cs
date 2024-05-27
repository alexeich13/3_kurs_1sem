using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textr1 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Данный прибор используется для вывода времени опускания груза.";

    }
}
