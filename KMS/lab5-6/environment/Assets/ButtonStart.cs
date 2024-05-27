using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class ButtonStart : MonoBehaviour {

    float Speed = 0.01f;
	float offset = 0;
    public GameObject Cross;
    public GameObject Cargo;
    public GameObject Text;
    bool Play;
    Quaternion startCross;
    Vector3 startCargo;
	Vector3 startButton;
	Quaternion needCross;
    Vector3 needCargo;
	Vector3 needButton;

    public anim animsock;

    void Start()
    {
		startCargo = Cargo.transform.position;
        startCross = Cross.transform.rotation;
		needCargo = new Vector3(-7.65f, 12.7f, 1.95f);//0.4539657f, 58.3f, 11.53063f
        needCross = Quaternion.Euler(-90f, -90f, -90f);
    }

    void OnEnable()
    {
        anim.OnPlayInstChanged += HandlePlayInstChanged;
    }

    void OnDisable()
    {
        anim.OnPlayInstChanged -= HandlePlayInstChanged;
    }

    void HandlePlayInstChanged(bool newValue)
    {
        Play = newValue;
    }

    void Update()
    {
        if(Play == false)
        {
            Cargo.transform.position = startCargo;
            Cross.transform.rotation = startCross;
            Text.GetComponent<TextMesh>().text = "0";
        }
        if (Input.GetMouseButtonDown(0) && Play == true)
        {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.transform == transform)
        {
            offset += Speed;
            Cargo.transform.position = Vector3.Lerp(startCargo, needCargo, offset);
            Cross.transform.rotation = Quaternion.Slerp(startCross, needCross, offset);
            if (offset >= 1)
            {
                offset = 0;
            }
        }
    }
}
}
