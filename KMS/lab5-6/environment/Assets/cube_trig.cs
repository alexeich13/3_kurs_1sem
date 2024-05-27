using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube_trig : MonoBehaviour {

	public Animator cube; // переменная типа Animator для ссылки на анимацию

    void Start() {
        cube = GetComponent<Animator>(); // инициализация контроллера анимации
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))  {  cube.SetTrigger("hitten");    }
    }
}
