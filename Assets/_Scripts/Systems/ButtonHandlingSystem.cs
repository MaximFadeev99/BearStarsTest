using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using VContainer;
using MessagePipe;
using BearStarsTest.Messages;
using System;
using Scellecs.Morpeh;

namespace BearStarsTest.Systems 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ButtonHandlingSystem))]
    public sealed class ButtonHandlingSystem : UpdateSystem
    {
        private IDisposable _disposableForSubscriptions;
        private bool _isUIInitalized;
        private string _pressedButtonLabel;

        [Inject]
        private void Construct(ISubscriber<UIInitializedMessage> initializedSusbscriber,
            ISubscriber<ButtonPressedMessage> buttonPressedSubscriber)
        {
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();

            initializedSusbscriber.Subscribe(OnUIInitialized).AddTo(bagBuilder);
            buttonPressedSubscriber.Subscribe(OnButtonPressed).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        public override void OnAwake() { }

        public override void OnUpdate(float deltaTime)
        {
            if (_isUIInitalized == false || _pressedButtonLabel == string.Empty)
                return;

            Entity newEntity = World.CreateEntity();
            PressedButtonComponent buttonComponent = new()
            {
                WindowName = _pressedButtonLabel
            };

            newEntity.SetComponent(buttonComponent);
            _pressedButtonLabel = string.Empty;
        }

        public override void Dispose()
        {
            _disposableForSubscriptions.Dispose();
        }

        private void OnUIInitialized(UIInitializedMessage _)
        {
            _isUIInitalized = true;
        }

        private void OnButtonPressed(ButtonPressedMessage message)
        {
            _pressedButtonLabel = message.ButtonLabel;
        }
    }
}