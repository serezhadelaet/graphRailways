using Basic;
using TMPro;
using UnityEngine;

namespace Level
{
    public class Base : Node
    {
        [field: SerializeField] public int InputResourcesMod { get; private set; } = 1;
        
        [SerializeField] private TextMeshProUGUI _inputResourcesModText;

        private void OnValidate()
        {
            _inputResourcesModText.text = InputResourcesMod.ToString("0.#") + "x";
        }
    }
}