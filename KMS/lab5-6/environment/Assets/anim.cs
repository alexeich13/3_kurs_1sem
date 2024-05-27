using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class anim : MonoBehaviour {

    public Animator sock; // переменная типа Animator для ссылки на анимацию
    public delegate void PlayInstChanged(bool newValue);
    public static event PlayInstChanged OnPlayInstChanged;
    [SerializeField]
    Text message;


    void Start() {
        sock = GetComponent<Animator>(); // инициализация контроллера анимации
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) // если нажата клавиша q
        {
            sock.SetBool("run", true); // переменная, отвечающая за переход, имеет значение true
            OnPlayInstChanged(true);
            message.text = "После необходимо нажать клавишу E и дождаться поднятия груза до уровня верхнего фиксатора.";
        }
        if (Input.GetKeyDown(KeyCode.W)) // если нажата клавиша w
        {
            sock.SetBool("run", false); // переменная, отвечающая за переход, имеет значение false
            OnPlayInstChanged(false);
        }
    }
}