using System.Collections;
using UnityEngine;

public class PointOfDestruction : MonoBehaviour
{
    [SerializeField] private GameObject _enemy;

    private SceneController _sceneController;
    private GridManager _gridManager;

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
            collision.gameObject.SetActive(false);
            Destroy(collision.gameObject, 2f);
            GameObject newEnemy = Instantiate(_enemy, new Vector2(Random.Range(-15f, 9.5f), 7.4f), Quaternion.identity);
            StartCoroutine(BlockEnemy(newEnemy));
        }
    }

    private IEnumerator BlockEnemy(GameObject enemy)
    {
        enemy.GetComponent<SAP2D.SAP2DAgent>().enabled = false;
        yield return new WaitForSeconds(1f);
        enemy.GetComponent<SAP2D.SAP2DAgent>().enabled = true;
    }
}
