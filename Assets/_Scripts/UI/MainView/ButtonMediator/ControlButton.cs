using BearStarsTest.Utilities;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BearStarsTest.UI.MainView.Buttons
{
    [RequireComponent(typeof(Button))]
    internal class ControlButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text _labelField;

        private Button _button;
        private GameObject _gameObject;

        internal event Action<string> Pressed; 

        internal void Initialize(string windowName)
        {
            if (string.IsNullOrEmpty(_labelField.text) == false)
            {
                CustomLogger.Log(gameObject.name, "This button has aleady been initialized for a window." +
                    "A new window name can not be assigned!", MessageTypes.Warning);
                return;
            }

            _labelField.text = windowName;
            _gameObject = gameObject;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonPressed);
        }

        internal void Dispose() 
        {
            _button.onClick.RemoveListener(OnButtonPressed);
            Destroy(_gameObject);
        }

        private void OnButtonPressed() 
        {
            Pressed?.Invoke(_labelField.text);
        }
    }
}
