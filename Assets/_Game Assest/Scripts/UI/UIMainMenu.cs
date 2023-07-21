using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    public void OnPlayButtonClick()
    {
        GameManager.Instance.LevelStart();
    }
}
