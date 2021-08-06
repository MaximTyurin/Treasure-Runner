using UnityEngine;

public class Ropes : MonoBehaviour
{
    [SerializeField] private Transform _playerPositionOnRope;
    private float _gravityOnRope = 0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.IsClimbingOnRope = true;
            enemy.IsFalling = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            if (player.IsGrounded == true || (player.IsClimbingOnLadder == true && player.IsFalling == false))
            {
                MovePlayerOnRope(player);
            }

            if(player.IsFalling)
            {
                player.IsFalling = false;
                MovePlayerOnRope(player);
            }
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.IsClimbingOnRope = true;
            enemy.IsFalling = false;
            if (enemy.IsGrounded == true || (enemy.IsClimbingOnLadder == true && enemy.IsFalling == false))
            {
                MovePlayerOnRope(enemy);
            }

            if (enemy.IsFalling)
            {
                enemy.IsFalling = false;
                MovePlayerOnRope(enemy);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Player player))
        {
            player.IsClimbingOnRope = false;
            if (player.IsGrounded == false && player.IsClimbingOnLadder == false)
            {
                  player.IsFalling = true;
            }
        }

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.IsClimbingOnRope = false;
            if (enemy.IsGrounded == false && enemy.IsClimbingOnLadder == false)
            {
                enemy.IsFalling = true;
            }
        }
    }

    private void MovePlayerOnRope(Player player)
    {
        player.transform.position = new Vector2(player.transform.position.x, _playerPositionOnRope.transform.position.y);
        player.IsClimbingOnRope = true;
        player.GetComponent<Rigidbody2D>().gravityScale = _gravityOnRope;
    }

    private void MovePlayerOnRope(Enemy enemy)
    {
        enemy.transform.position = new Vector2(enemy.transform.position.x, _playerPositionOnRope.transform.position.y);
        enemy.IsClimbingOnRope = true;
        enemy.GetComponent<Rigidbody2D>().gravityScale = _gravityOnRope;
    }
}
