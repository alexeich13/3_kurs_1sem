using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textr8 : MonoBehaviour {

	[SerializeField]
    Text message;

    public void OnSettings()
    {  
        message.text = "Нажмите, чтобы открыть пошаговую инструкцию выполнения лабораторной.";
    }
}
