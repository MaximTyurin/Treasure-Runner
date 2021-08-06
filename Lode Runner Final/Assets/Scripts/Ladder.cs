using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform _topLadder;

    private float _gravityOnLadder = 0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Player player))
        {
            player.IsClimbingOnRope = false;
            player.IsClimbingOnLadder = true;
            player.IsFalling = false;
            player.GetComponent<Rigidbody2D>().gravityScale = _gravityOnLadder;
            if(player.transform.position.y > _topLadder.transform.position.y)
            {
                player.GetComponent<Transform>().localPosition = new Vector2(player.transform.position.x, _topLadder.transform.position.y);
            }
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.IsClimbingOnLadder = true;
            enemy.IsFalling = false;
            enemy.GetComponent<Rigidbody2D>().gravityScale = _gravityOnLadder;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.IsClimbingOnLadder = false;
            if (player.IsClimbingOnLadder == false && player.IsGrounded == false && player.IsClimbingOnRope == false)
            {
                player.IsFalling = true;
            }
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.IsClimbingOnLadder = false;
            if (enemy.IsClimbingOnLadder == false && enemy.IsGrounded == false && enemy.IsClimbingOnRope == false)
            {
                enemy.IsFalling = true;
            }
        }
    }
}
