using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using BearStarsTest.UI.MainView;
using VContainer;
using System.Collections.Generic;
using BearStarsTest.Data;
using MessagePipe;
using BearStarsTest.Messages;
using System.Linq;

namespace BearStarsTest.Systems 
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(UIInitializer))]
    public sealed class UIInitializer : Initializer
    {
        [SerializeField] private List<WindowData> _windowData;

        private IObjectResolver _objectResolver;
        private IViewPanel _viewPanel;
        private Transform _canvasTransform;
        private IPublisher<UIInitializedMessage> _initializedPublisher;

        [Inject]
        private void Construct(IViewPanel viewPanelPrefab, Transform canvasTransform,
            IPublisher<UIInitializedMessage> initializedPublisher, IObjectResolver objectResolver)
        {
            _viewPanel = viewPanelPrefab;
            _canvasTransform = canvasTransform;
            _initializedPublisher = initializedPublisher;
            _objectResolver = objectResolver;
        }

        public override void OnAwake()
        {
            _viewPanel = GameObject
                .Instantiate(_viewPanel.GameObject, _canvasTransform)
                .GetComponent<IViewPanel>();
            _viewPanel.Initialize(_windowData, _objectResolver);
            _initializedPublisher.Publish(new UIInitializedMessage
                (_windowData.Select(data => data.Name).ToList()));
        }

        public override void Dispose()
        {
            _viewPanel.Dispose();
        }
    }
}