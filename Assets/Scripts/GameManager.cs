using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float _INIT_SPEED = 60f;

    public static GameManager instance;

    public float speed;

    void Start()
    {
        if (instance == null)
            instance = this;

        speed = _INIT_SPEED;
    }

    private void Update()
    {
        speed = _INIT_SPEED + 2f * Time.time;
    }
}
