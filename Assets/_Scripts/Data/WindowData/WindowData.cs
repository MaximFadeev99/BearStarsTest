using UnityEngine;

namespace BearStarsTest.Data
{
    [CreateAssetMenu(fileName = "NewWindowData", menuName = "ProjectData/WindowData",
        order = 52)]
    public class WindowData : ScriptableObject
    {
        [field: SerializeField] public string Name {  get; private set; }
        [field: SerializeField] public string CountFieldPhrase { get; private set; }
        [field: SerializeField] public string PreviousWindowFieldPhrase { get; private set; }
    }
}