using UnityEngine;

public class FakeBrick : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Rb.bodyType = RigidbodyType2D.Kinematic;
            enemy.IsGrounded = true;
            enemy.IsFalling = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.Rb.bodyType = RigidbodyType2D.Dynamic;
            if (enemy.IsGrounded == false)
                enemy.IsFalling = true;
        }
    }
}
