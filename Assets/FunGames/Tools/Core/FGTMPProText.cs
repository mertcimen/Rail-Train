using System;
using TMPro;
using UnityEngine;

namespace FunGames.Tools.Core
{
    [RequireComponent(typeof(TMP_Text))]
    public class FGTMPProText : MonoBehaviour
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            if (String.IsNullOrEmpty(FGMainSettings.settings.GameName))
            {
                Debug.LogWarning("Game Name is missing in FGMainSettings.");
                return;
            }

            _text.text = _text.text.Replace("THE GAME", FGMainSettings.settings.GameName);
        }
    }
}