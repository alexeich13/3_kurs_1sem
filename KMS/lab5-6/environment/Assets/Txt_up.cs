using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Txt_up : MonoBehaviour {

    [SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Для начала работы нужно подключить установку к источнику питания.";

    }
    public void OnExit()
    {  
        message.text = "После окончания работы не забудьте отключить устройство.";
    }
}
