using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.instance.obstacles.Add(this);
    }

    void Update()
    {
        transform.Translate(
            Vector2.up * -GameManager.instance.speed * 0.02f * Time.deltaTime);

        if (transform.position.y < -12f)
            Die(direct: true);
    }

    public void Die(bool direct = false)
    {
        GameManager.instance.obstacles.Remove(this);
        Destroy(gameObject);
    }
}
