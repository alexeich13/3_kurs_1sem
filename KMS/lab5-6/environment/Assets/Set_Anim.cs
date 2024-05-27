using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Set_Anim : MonoBehaviour {

	public Animator set; // переменная типа Animator для ссылки на анимацию
    public anim animsock;    
    bool Play;

    void Start() {
        set = GetComponent<Animator>(); // инициализация контроллера анимации
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

    void Update() {
        if(Play == true){
        if (Input.GetKeyDown(KeyCode.E)) // если нажата клавиша q
        {
            set.SetBool("act", true); // переменная, отвечающая за переход, имеет значение true
        }
        if (Input.GetKeyDown(KeyCode.R)) // если нажата клавиша w
        {
            set.SetBool("act", false); // переменная, отвечающая за переход, имеет значение false
        }
        }
    }
}
