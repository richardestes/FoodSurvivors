using System;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerStats player;
    private CircleCollider2D playerCollector;
    public float PullSpeed;
    
    
    private void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        // An attempt at optimization. If this ends up causing issues, remove this check
        if (Math.Abs(playerCollector.radius - player.currentMagnet) > 0.01f) 
        {
            playerCollector.radius = player.currentMagnet;
        }
        // playerCollector.radius = player.currentMagnet;
    }
 
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out ICollectible collectible))
        {
            // Pull items toward player
            var rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 forceDirection = (transform.position - col.transform.position).normalized;
            rb.AddForce(forceDirection * PullSpeed); 
            
            collectible.Collect();
        }
    }
}
