using BearStarsTest.Data;
using BearStarsTest.Messages;
using BearStarsTest.Utilities;
using MessagePipe;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using VContainer;

namespace BearStarsTest.UI.MainView.Windows
{
    [Serializable]
    internal class WindowMediator
    {
        [SerializeField] private TMP_Text _headlineField;
        [SerializeField] private BasicWindow _simpleWindowPrefab;
        [SerializeField] private Transform _windowContainer;
        [SerializeField] private GameObject _gameObject;

        private readonly List<BasicWindow> _createdSimpleWindows = new();

        private BasicWindow _currentlyOpenWindow;
        private IPublisher<WindowPressedMessage> _windowClosedPublisher;

        internal void Initialize(IReadOnlyList<WindowData> windowData, IObjectResolver objectResolver) 
        {
            foreach (WindowData pieceOfData in windowData) 
            {
                BasicWindow newSimpleWindow = GameObject.Instantiate
                    (_simpleWindowPrefab, _windowContainer);

                newSimpleWindow.Initialize(pieceOfData);
                newSimpleWindow.SetActive(false);
                newSimpleWindow.Pressed += OnSimpleWindowPressed;
                _createdSimpleWindows.Add(newSimpleWindow);             
            }

            _windowClosedPublisher = objectResolver.Resolve<IPublisher<WindowPressedMessage>>();
        }

        internal void DrawSimpleWindow(string windowName, int openedCount, string previousWindowName) 
        {
            if (_currentlyOpenWindow != null)
                _currentlyOpenWindow.SetActive(false);
            
            BasicWindow targetWindow = _createdSimpleWindows
                .FirstOrDefault(window => window.Name == windowName);

            if (targetWindow == null) 
            {
                CustomLogger.Log(nameof(WindowMediator), $"You are trying to draw a {nameof(BasicWindow)} " +
                    $"named {windowName}, but no window of this type and with this name have been created!",
                    MessageTypes.Error);

                return;
            }

            _gameObject.SetActive(true);
            _headlineField.text = targetWindow.Name;
            targetWindow.DrawCountField(openedCount);
            targetWindow.DrawPreviousWindowField(previousWindowName);
            targetWindow.SetActive(true);
            _currentlyOpenWindow = targetWindow;
        }

        internal void CloseWindow(string windowName) 
        {
            BasicWindow targetWindow = _createdSimpleWindows.FirstOrDefault
                (window => window.Name == windowName);

            if (targetWindow == null)
            {
                CustomLogger.Log(nameof(WindowMediator), $"There is no window name {windowName} among " +
                    $"created windows, but you are trying to close it!", MessageTypes.Error);
                return;
            }

            targetWindow.SetActive(false);

            if (_currentlyOpenWindow == targetWindow)
                _currentlyOpenWindow = null;

            _gameObject.SetActive(false);
        }

        internal void Dispose() 
        {
            foreach (BasicWindow createdWindow in _createdSimpleWindows)
            {
                createdWindow.Pressed -= OnSimpleWindowPressed;
                createdWindow.Dispose();
            }

            _createdSimpleWindows.Clear();
        }

        private void OnSimpleWindowPressed(string pressedWindowName) 
        {
            _windowClosedPublisher.Publish(new WindowPressedMessage(pressedWindowName));
        }
    }
}