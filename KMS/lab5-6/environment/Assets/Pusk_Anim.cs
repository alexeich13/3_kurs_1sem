using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Pusk_Anim : MonoBehaviour {

	public Animator pusk; // переменная типа Animator для ссылки на анимацию
    public anim animsock;
    bool Play;
    public GameObject Cargo; // Ссылка на объект Cargo
    public GameObject Cross; // Ссылка на объект Cross
    public GameObject Text;
    [SerializeField]
    Text message;

    void Start() {
        pusk = GetComponent<Animator>(); // инициализация контроллера анимации
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
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            pusk.SetBool("pusk", true); 
            StartCoroutine(PerformActionsAfterE());
            message.text = "Отпустить кнопки Сброс и Пуск нажатием R.";
        }
        if (Input.GetKeyDown(KeyCode.R)) 
        {
            pusk.SetBool("pusk", false); 
            StartCoroutine(PerformActionsAfterR());
            message.text = "Зафиксировать итоговое время, показанное прибором.";
        }
        }
    }
    IEnumerator PerformActionsAfterE()
    {
        // Начальные значения
        float duration = 1.0f;
        Vector3 startPosCargo = Cargo.transform.position;
        Vector3 endPosCargo = startPosCargo + Vector3.up * 5.0f; // Поднимаем на 2 метра
        Quaternion startRotCross = Cross.transform.rotation;

        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            // Поднимаем груз
            Cargo.transform.position = Vector3.Lerp(startPosCargo, endPosCargo, t);

            // Вращаем Cross
            float angle = t * 270.0f; // 3 оборота в течение длительности
            Cross.transform.rotation = Quaternion.Euler(0, -90, angle);

            yield return null;
        }
    }

    IEnumerator PerformActionsAfterR()
    {
    message.text = "Дождитесь момента полного опускания груза на платформу.";
    float duration = 1.0f;
    Vector3 startPosCargo = Cargo.transform.position;
    Vector3 endPosCargo = startPosCargo - Vector3.up * 5.0f;

    Quaternion startRotCross = Cross.transform.rotation;

    float startTime = Time.time;

    while (Time.time - startTime < duration)
    {
        float t = (Time.time - startTime) / duration;

        // Поднимаем/опускаем груз
        Cargo.transform.position = Vector3.Lerp(startPosCargo, endPosCargo, t);

        // Вращаем Cross
        float angle = t * 270.0f;
        Cross.transform.rotation = Quaternion.Euler(0, -90, -angle);

        yield return null;
    }
    float totalTime = Time.time - startTime;
    string totalString = totalTime.ToString("F4");
    Text.GetComponent<TextMesh>().text = totalString;
    }
}
