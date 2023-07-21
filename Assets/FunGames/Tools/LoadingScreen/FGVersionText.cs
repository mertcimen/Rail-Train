using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FGVersionText : MonoBehaviour
{
    private void Awake()
    {
        Text text = GetComponent<Text>();
        text.text = FGMain.Instance.ModuleVersion;
    }
}
