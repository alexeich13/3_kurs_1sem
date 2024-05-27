using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindUp : MonoBehaviour {

	void Start()
    {
        Close(); 
        
    }

    public void Open()
    {
        gameObject.SetActive(true); 

    }

    public void Close()
    {
        gameObject.SetActive(false); 
    }
}
