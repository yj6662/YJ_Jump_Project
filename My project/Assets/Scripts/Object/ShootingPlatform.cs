using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingPlatform : MonoBehaviour, IInteractable
{
    public float shootPower = 500f;
    public float shootTime = 3f;

    private Rigidbody playerRb;
    private bool isOnPlatform = false;
    private float lastShootTime = 0f;
    private Transform cameraTransform;

    private void Update()
    {
        if (isOnPlatform && Time.time >= lastShootTime + shootTime)
        {
            Shoot();
            lastShootTime = Time.time; 
            isOnPlatform = false; 
            playerRb = null;
        }
    }

    public string GetInteractPrompt()
    {
        string str = $"밟으면 플레이어가 바라보는 방향으로 <br> {shootTime} 초후에 발사되는 발사대입니다.";
        return str;
    }
    
    public void OnInteract()
    {}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnPlatform = true;
            playerRb = collision.gameObject.GetComponent<Rigidbody>(); 
            cameraTransform = collision.gameObject.GetComponent<PlayerController>().cameraContainer;
            lastShootTime = Time.time;
            Debug.Log("발사 준비 완료: " + shootTime + "초 후 발사됩니다.");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isOnPlatform = false;
            playerRb = null;
            cameraTransform = null;
            Debug.Log("플랫폼에서 벗어났습니다.");
        }
    }

    private void Shoot()
    {
        if (playerRb != null)
        {
            playerRb.AddForce(cameraTransform.forward * shootPower, ForceMode.Impulse);
            Debug.Log("발사!");
        }
        else
        {
            Debug.LogWarning("Rigidbody 컴포넌트를 찾을 수 없습니다.");
        }
    }
}