using UnityEngine;

public class BreakableProps : MonoBehaviour
{
    public float Health;

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        if (Health <= 0f)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }
}
