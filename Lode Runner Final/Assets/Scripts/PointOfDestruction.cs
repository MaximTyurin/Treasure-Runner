using System.Collections;
using UnityEngine;

public class PointOfDestruction : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    private SceneController _sceneController;

    private float _yPosSpawnEnemy = 7.4f;
    private float _xMinPosSpawnEnemy = -15f;
    private float _xMaxPosSpawnEnemy = -9.5f;

    private void Start()
    {
        _sceneController = FindObjectOfType<SceneController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            _sceneController.GetComponent<SceneController>().Restartlevel();
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            float delayDestroy = 2f;
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject, delayDestroy);
            GameObject newEnemy = Instantiate(_enemy, new Vector2(Random.Range(_xMinPosSpawnEnemy, _xMaxPosSpawnEnemy), _yPosSpawnEnemy), Quaternion.identity);
            StartCoroutine(BlockEnemy(newEnemy, delayDestroy / 2));
        }
    }

    private IEnumerator BlockEnemy(GameObject enemy, float delay)
    {
        enemy.GetComponent<SAP2D.SAP2DAgent>().enabled = false;
        yield return new WaitForSeconds(delay);
        enemy.GetComponent<SAP2D.SAP2DAgent>().enabled = true;
    }
}
