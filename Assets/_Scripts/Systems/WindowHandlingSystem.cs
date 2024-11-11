using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System;
using MessagePipe;
using VContainer;
using BearStarsTest.Messages;
using Scellecs.Morpeh;
using System.Collections.Generic;
using BearStarsTest.Utilities;

namespace BearStarsTest.Systems 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WindowHandlingSystem))]
    public sealed class WindowHandlingSystem : UpdateSystem
    {
        private readonly Dictionary<string, int> _windowCountDictionary = new();

        private Filter _targetEntities;
        private IPublisher<ChangeLayoutMessage> _changeLayoutPublisher;
        private IPublisher<CloseWindowMessage> _closeWindowPublisher;
        private IDisposable _disposableForSubscriptions;

        private bool _isUIInitalized;
        private string _latestWindowName;
        private string _previousWindowName;

        [Inject]
        private void Construct(ISubscriber<UIInitializedMessage> initializedSusbscriber,
            ISubscriber<WindowPressedMessage> windowPressedSusbscriber,
            IPublisher<ChangeLayoutMessage> changeLayoutPublisher,
            IPublisher<CloseWindowMessage> closeWindowPublisher)
        {
            _changeLayoutPublisher = changeLayoutPublisher;
            _closeWindowPublisher = closeWindowPublisher;

            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();

            initializedSusbscriber.Subscribe(OnUIInitialized).AddTo(bagBuilder);
            windowPressedSusbscriber.Subscribe(OnWindowPressed).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        public override void OnAwake()
        {
            _targetEntities = World.Filter.With<PressedButtonComponent>().Build();
            _previousWindowName = string.Empty;
            _latestWindowName = string.Empty;
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_isUIInitalized == false)
                return;

            foreach (Entity entity in _targetEntities)
            {
                PressedButtonComponent pressedButtonComponent = entity.GetComponent<PressedButtonComponent>();

                ProcessViewChange(pressedButtonComponent.WindowName);
                World.RemoveEntity(entity);
            }
        }

        public override void Dispose()
        {
            _disposableForSubscriptions.Dispose();
        }

        private void OnUIInitialized(UIInitializedMessage message)
        {
            _isUIInitalized = true;

            foreach (string windowName in message.CreatedWindows)
                _windowCountDictionary[windowName] = 0;

            _changeLayoutPublisher.Publish(new ChangeLayoutMessage
                ("Ничего не нажато", string.Empty, 0, string.Empty));
        }

        private void OnWindowPressed(WindowPressedMessage message)
        {
            _closeWindowPublisher.Publish(new CloseWindowMessage($"{message.PressedWindowName} закрыто",
                message.PressedWindowName));
            _latestWindowName = string.Empty;
            _previousWindowName = string.Empty;
        }

        private void ProcessViewChange(string nextWindowName)
        {
            if (_windowCountDictionary.ContainsKey(nextWindowName) == false)
            {
                CustomLogger.Log(nameof(WindowHandlingSystem), "The system has received an unknown window's " +
                    "name, so it can not process a layout change!", MessageTypes.Error);
                return;
            }

            if (_latestWindowName != nextWindowName && _latestWindowName != string.Empty)
                _previousWindowName = _latestWindowName;

            string performedAction = $"Нажато \"Открыть {nextWindowName}\"";
            string windowHeadline = nextWindowName;
            ChangeLayoutMessage changeLayoutMessage = new(performedAction, windowHeadline,
                ++_windowCountDictionary[nextWindowName], _previousWindowName);

            _changeLayoutPublisher.Publish(changeLayoutMessage);
            _latestWindowName = nextWindowName;
        }
    }
}