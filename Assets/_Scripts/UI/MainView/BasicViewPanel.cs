using BearStarsTest.Data;
using BearStarsTest.Messages;
using BearStarsTest.UI.MainView.Buttons;
using BearStarsTest.UI.MainView.Windows;
using MessagePipe;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VContainer;

namespace BearStarsTest.UI.MainView
{
    public class BasicViewPanel : MonoBehaviour, IViewPanel
    {
        [SerializeField] private ButtonMediator _buttonMediator;
        [SerializeField] private WindowMediator _windowMediator;

        private IDisposable _disposableForSubscriptions;

        [field: SerializeField] public GameObject GameObject { get; private set; }

        public void Initialize(IReadOnlyList<WindowData> windowData, IObjectResolver objectResolver)
        {
            _windowMediator.Initialize(windowData, objectResolver);
            _buttonMediator.Initialize(objectResolver, windowData.Select(data => data.Name).ToArray());

            SetSubscriptions(objectResolver);
        }

        public void Dispose()
        {
            if (GameObject == null)
                return;

            _disposableForSubscriptions.Dispose();
            _buttonMediator.Dispose();
            _windowMediator.Dispose();
            Destroy(GameObject);
        }

        private void SetSubscriptions(IObjectResolver objectResolver) 
        {
            DisposableBagBuilder bagBuilder = DisposableBag.CreateBuilder();
            ISubscriber<ChangeLayoutMessage> changeLayoudSubscriber =
                objectResolver.Resolve<ISubscriber<ChangeLayoutMessage>>();
            ISubscriber<CloseWindowMessage> closeWindowSubscriber =
                objectResolver.Resolve<ISubscriber<CloseWindowMessage>>();

            changeLayoudSubscriber.Subscribe(OnLayoutChangeMessage).AddTo(bagBuilder);
            closeWindowSubscriber.Subscribe(OnCloseWindowMessage).AddTo(bagBuilder);
            _disposableForSubscriptions = bagBuilder.Build();
        }

        private void OnLayoutChangeMessage(ChangeLayoutMessage message) 
        {
            _buttonMediator.DrawPerformedActionField(message.PerformedAction);

            if (message.WindowName != string.Empty) 
            {
                _windowMediator.DrawSimpleWindow(message.WindowName, message.OpenCount, 
                    message.PreviousWindowName);
            }
        }

        private void OnCloseWindowMessage(CloseWindowMessage message) 
        {
            _buttonMediator.DrawPerformedActionField(message.PerformedAction);
            _windowMediator.CloseWindow(message.WindowName);
        }
    }
}