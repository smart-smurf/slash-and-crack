using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObstacleManager : MonoBehaviour
{
    [SerializeField] private GameObject _normalMesh;
    [SerializeField] private GameObject _fracturedMesh;
    private Collider _collider;

    private int _level;
    private int _lives;

    private bool _takenHit;

    private void Awake()
    {
        _takenHit = false;
        _collider = GetComponent<Collider>();
        GameManager.instance.obstacles.Add(this);
    }

    void Update()
    {
        transform.Translate(
            Vector2.up * -GameManager.instance.speed * 0.02f * Time.deltaTime);

        if (transform.position.y < -12f)
            Die(direct: true);
    }

    public void Initialize(int level, Material material, Vector3 eulerAngles)
    {
        _level = level;
        _lives = level + 1;
        _normalMesh.GetComponent<MeshRenderer>().material = material;
        foreach (Transform child in _fracturedMesh.transform)
            child.GetComponent<MeshRenderer>().material = material;
        _normalMesh.transform.eulerAngles = eulerAngles;
        _fracturedMesh.transform.eulerAngles = eulerAngles;
    }

    public (bool, bool) TakeHit()
    {
        if (_takenHit) return (false, false);
        if (AudioManager.instance)
            AudioManager.instance.PlaySound(AudioClipId.Slash);

        _lives--;
        bool destroyed;
        if (_lives == 0)
        {
            destroyed = true;
            Die();
        }
        else
        {
            destroyed = false;
            transform.localScale *= 0.75f;
            Vector3 ea = new Vector3(
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f));
            _normalMesh.transform.eulerAngles = ea;
            _fracturedMesh.transform.eulerAngles = ea;
        }

        StartCoroutine(_TakingHit());
        return (true, destroyed);
    }

    public void Die(bool direct = false)
    {
        GameManager.instance.obstacles.Remove(this);
        _collider.enabled = false;
        if (!direct)
        {
            EventManager.TriggerEvent("ObstacleDestroyed", _level + 1);
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

    private IEnumerator _TakingHit()
    {
        _takenHit = true;
        yield return new WaitForSeconds(0.1f);
        _takenHit = false;
    }
}
