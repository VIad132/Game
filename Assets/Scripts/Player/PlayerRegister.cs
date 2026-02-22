using UnityEngine;

public class PlayerRegister : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.RegisterPlayer(transform);
    }
}
