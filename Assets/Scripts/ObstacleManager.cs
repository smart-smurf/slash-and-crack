using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    void Update()
    {
        transform.Translate(
            Vector2.up * -GameManager.instance.speed * 0.02f * Time.deltaTime);

        if (transform.position.y < -12f)
            Destroy(gameObject);
    }
}
