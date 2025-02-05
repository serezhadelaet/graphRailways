using Basic;
using TMPro;
using UnityEngine;

namespace Level
{
    public class Mine : Node
    {
        [field: SerializeField] public float MineDurationMod { get; private set; } = 1;
        [SerializeField] private TextMeshProUGUI _inputResourcesModText;

        private void OnValidate()
        {
            _inputResourcesModText.text = MineDurationMod.ToString("0.#") + "x";
        }
        
        public bool IsFree { get; private set; } = true;

        public void Reserve()
        {
            IsFree = false;
        }

        public void CompleteMining()
        {
            IsFree = true;
        }
    }
}