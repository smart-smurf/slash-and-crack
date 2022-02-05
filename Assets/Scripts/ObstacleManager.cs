using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject _normalMesh;
    [SerializeField] private GameObject _fracturedMesh;

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

    public void SetRotation(Vector3 eulerAngles)
    {
        _normalMesh.transform.eulerAngles = eulerAngles;
        _fracturedMesh.transform.eulerAngles = eulerAngles;
    }

    public void Die(bool direct = false)
    {
        GameManager.instance.obstacles.Remove(this);
        if (!direct)
        {
            EventManager.TriggerEvent("ObstacleDestroyed");
            _fracturedMesh.SetActive(true);
            _normalMesh.SetActive(false);
            Invoke("_Destroy", 0.45f);
        }
        else
        {
            EventManager.TriggerEvent("ObstaclePassed");
            _Destroy();
        }
    }

    private void _Destroy()
    {
        Destroy(gameObject);
    }
}
