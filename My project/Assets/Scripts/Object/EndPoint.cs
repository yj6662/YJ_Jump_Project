using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour, IInteractable
{
    public string GetInteractPrompt()
    {
        string str = $"정상입니다.<br>게임을 끝내시겠습니까? <br> `E`를 눌러 게임 종료";
        return str;
    }
    public void OnInteract()
    {
        GameManager.Instance.QuitGame();
    }
}
