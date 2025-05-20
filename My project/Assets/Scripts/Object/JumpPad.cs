using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpPower = 200f;
    public Vector3 jumpDirection = Vector3.up;
    public ForceMode forceMode = ForceMode.Impulse;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRB = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRB != null)
            {
                playerRB.AddForce(jumpDirection * jumpPower, forceMode);
                Debug.Log("Jump!");
            }
        }
    }
}
