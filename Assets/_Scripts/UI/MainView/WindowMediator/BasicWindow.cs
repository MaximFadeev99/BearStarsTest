using BearStarsTest.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BearStarsTest.UI.MainView.Windows
{
    [RequireComponent(typeof(Button))]
    internal class BasicWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _countField;
        [SerializeField] private TMP_Text _previousWindowField;

        private WindowData _windowData;
        private Button _button;
        private GameObject _gameObject;

        internal string Name => _windowData.Name;

        internal event Action<string> Pressed;

        internal void Initialize(WindowData windowData) 
        {
            _windowData = windowData;
            _gameObject = gameObject;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonPressed);
        }

        internal void DrawCountField(int count) 
        {
            string inflection = count == 2 || count == 3 || count == 4 ? "раза" : "раз";

            _countField.text = $"{_windowData.CountFieldPhrase} {count} {inflection}";
        }

        internal void DrawPreviousWindowField(string previousWindowName) 
        {
            if (previousWindowName == string.Empty) 
            {
                _previousWindowField.text = string.Empty;
                return;
            }

            _previousWindowField.text = 
                $"{_windowData.PreviousWindowFieldPhrase} {previousWindowName}";
        }

        internal void SetActive(bool isActive) 
        {
            _gameObject.SetActive(isActive);
        }

        internal void Dispose() 
        {
            _button.onClick.RemoveListener(OnButtonPressed);
            Destroy(_gameObject);
        }

        private void OnButtonPressed() 
        {
            Pressed?.Invoke(_windowData.Name);
        }
    }
}