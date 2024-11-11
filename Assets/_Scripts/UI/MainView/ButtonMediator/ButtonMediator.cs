using BearStarsTest.Messages;
using BearStarsTest.UI.MainView.Buttons;
using BearStarsTest.Utilities;
using MessagePipe;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

namespace BearStarsTest.UI.MainView.Buttons
{
    [Serializable]
    internal class ButtonMediator
    {
        [SerializeField] private TMP_Text _performedActionField;
        [SerializeField] private Transform _buttonContainer;
        [SerializeField] private ControlButton _controlButtonPrefab;
        [SerializeField] private int _maxActiveButtonCount = 9;

        private readonly List<ControlButton> _activeButtons = new();

        private IPublisher<ButtonPressedMessage> _buttonPressedPublisher;

        internal void Initialize(IObjectResolver objectResolver, params string[] windowNames)
        {
            if (windowNames.Length > _maxActiveButtonCount) 
            {
                CustomLogger.Log(nameof(ButtonMediator), $"You are trying to create {windowNames.Length} " +
                    $"buttons, which is more than the maximum allowed number of {_maxActiveButtonCount}!",
                    MessageTypes.Warning);
            }

            foreach (string windowName in windowNames)
            {
                ControlButton newControlButton = GameObject.Instantiate
                    (_controlButtonPrefab, _buttonContainer);
                newControlButton.Initialize(windowName);
                newControlButton.Pressed += OnControlButtonPressed;
                _activeButtons.Add(newControlButton);
            }

            _buttonPressedPublisher = objectResolver.Resolve<IPublisher<ButtonPressedMessage>>();
        }

        internal void DrawPerformedActionField(string performedAction)
        {
            _performedActionField.text = performedAction;
        }

        internal void Dispose()
        {
            foreach (ControlButton controlButton in _activeButtons)
            {
                controlButton.Pressed -= OnControlButtonPressed;
                controlButton.Dispose();
            }

            _activeButtons.Clear();
        }

        private void OnControlButtonPressed(string windowName) 
        {
            _buttonPressedPublisher.Publish(new ButtonPressedMessage(windowName));
        }
    }
}