using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : NetworkBehaviour
{
    public float force = 10;
    public int damageAmount = 5;

    // Start is called before the first frame update
    private void Update()
    {
        transform.position -= transform.forward * force * Time.deltaTime;
    }
    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(desotryafter());

    }
    private IEnumerator desotryafter()
    {
        yield return new WaitForSeconds(3);
        NetworkServer.Destroy(gameObject);

    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.transform.parent.GetComponentInParent<PlayerHealth>();

            playerHealth.TakeDamage(damageAmount);
            Destroy(other.gameObject);
        }
    }
}
