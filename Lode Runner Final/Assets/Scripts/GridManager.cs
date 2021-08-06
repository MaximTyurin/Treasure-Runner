using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Transform _pointForDestroyBricks;
    [SerializeField] private GameObject _pointOfDestructionObjectsStuckInBricks;
    [SerializeField] private GameObject _centerOfDestroyedBrick;
    [SerializeField] private GameObject _checkEnemyOnPit;
    [SerializeField] private Transform _rightCorner;
    [SerializeField] private Transform _leftCorner;
    [SerializeField] private float _coolDownRestoreBricks = 3f;

    private Tilemap _tileMap;
    private PlayerInput _destroyer;

    public float CoolDownRestoreBricksProp => _coolDownRestoreBricks;

    public Transform RightCorner => _rightCorner;

    public Transform LeftCorner => _leftCorner;

    public void DestroyBrick()
    {
        float direction = _destroyer.GridManager.Destroy.ReadValue<float>();
        _player.DestroyBricks(direction);
        Vector3 hitPosition = _pointForDestroyBricks.transform.position;
        Vector3 checkBricksForward = _player.transform.position + new Vector3(1f, 0f, 0f) * direction; // Позиция блока перед игроком
        if (_tileMap.HasTile(_tileMap.WorldToCell(hitPosition)) && _tileMap.HasTile(_tileMap.WorldToCell(checkBricksForward)) == false)
        {
            Tile recoverableBrick = _tileMap.GetTile<Tile>(_tileMap.WorldToCell(hitPosition));
            _tileMap.SetTile(_tileMap.WorldToCell(hitPosition), null);
            StartCoroutine(CoolDownRestoreBricks(recoverableBrick, hitPosition));
        }
    }

    public void DestroyFakeBrick(GameObject fakeBrick)
    {
        Destroy(fakeBrick, 2.5f);
    }

    private void Awake()
    {
        _destroyer = new PlayerInput();
        _tileMap = GetComponent<Tilemap>();
        _destroyer.GridManager.Destroy.performed += ctx => DestroyBrick();
    }

    private void OnEnable()
    {
        _destroyer.Enable();
    }

    private void OnDisable()
    {
        _destroyer.Disable();
    }

    private IEnumerator CoolDownRestoreBricks(Tile tile, Vector3 position)
    {
        Vector3Int tilemapPos = _tileMap.WorldToCell(position);
        GameObject centerOfDestroyedBrick = Instantiate(_centerOfDestroyedBrick, _tileMap.GetCellCenterWorld(tilemapPos), Quaternion.identity);

        GameObject checkEnemyOnPit = Instantiate(_checkEnemyOnPit, _tileMap.GetCellCenterWorld(tilemapPos) + new Vector3(0, 1, 0), Quaternion.identity);

        yield return new WaitForSeconds(_coolDownRestoreBricks);

        _tileMap.SetTile(_tileMap.WorldToCell(position), tile);
        GameObject pointOfDestructionObjectsStuckInBricks = Instantiate(_pointOfDestructionObjectsStuckInBricks, _tileMap.GetCellCenterWorld(tilemapPos), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Destroy(pointOfDestructionObjectsStuckInBricks);
        Destroy(centerOfDestroyedBrick);
        Destroy(checkEnemyOnPit);
    }
}
