using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Cam : MonoBehaviour {

	bool move = false;
    float speed = 0.01f;
    float offset = 0;
    Vector3 startPosition;
    Vector3 needPosition;
    Quaternion startRotation;
    Quaternion needRotaton;

    public void MoveMain()         //функция для просмотра прибора
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-2f, 8f, -4f);
        needRotaton = Quaternion.AngleAxis(-45f, new Vector3(0, 1, 0));
    }
     public void Move2()         //функция для просмотра верхнего блока
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-3f, 14f, -0.4f);
        needRotaton = Quaternion.AngleAxis(-60f, new Vector3(0, 1, 0));
    }
     public void Move3()         //функция для просмотра нижнего блока
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-3f, 10.5f, -0.4f);
        needRotaton = Quaternion.AngleAxis(-70f, new Vector3(0, 1, 0));
    }
     public void Move4()         //функция для просмотра верхнего кронштейна
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-7f, 13f, 0.3f);
        needRotaton = Quaternion.AngleAxis(-40f, new Vector3(0, 1, 0));
    }
     public void Move5()         //функция для просмотра нижнего кронштейна
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-6.5f, 8.5f, 0.3f);
        needRotaton = Quaternion.AngleAxis(-70f, new Vector3(0, 1, 0));
    }
     public void Move6()         //функция для просмотра неподвижных грузов
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-2.5f, 10f, -0.4f);
        needRotaton = Quaternion.AngleAxis(-80f, new Vector3(0, 1, 0));
    }
     public void Move7()         //функция для просмотра груза на подставке
    {
        move = true;
        startPosition = transform.position;
        startRotation = transform.rotation;
        needPosition = new Vector3(-6f, 8f, 0.7f);
        needRotaton = Quaternion.AngleAxis(-70f, new Vector3(0, 1, 0));
    }
    void Update()
    {
        if (move)
        {
            offset += speed;
            transform.position = Vector3.Lerp(startPosition, needPosition, offset);
            transform.rotation = Quaternion.Slerp(startRotation, needRotaton, offset);
            if (offset >= 1)
            {
                move = false;
                offset = 0;
            }
        }
    }

}
