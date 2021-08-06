using UnityEngine;

public class Treasure : MonoBehaviour
{
    public void ActivateTreasure()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            if(enemy.CanTakeTreasure)
            {
                enemy.CanTakeTreasure = false;
                enemy.HaveTreasure.SetActive(true);
                Destroy(gameObject);
            }
        }
    }
}