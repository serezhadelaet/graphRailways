using System;
using TMPro;
using UnityEngine;

namespace Basic
{
    public class Line : MonoBehaviour
    {
        [field: SerializeField] public Node From { get; set; }
        [field: SerializeField] public Node To { get; set; }
        [field: SerializeField] public float Length { get; private set; } = 10;
        [field: SerializeField] public Transform Visual { get; private set; }

        [SerializeField] private TextMeshProUGUI _lengthText;

        private void OnValidate()
        {
            _lengthText.text = Length.ToString("0.#");
        }
    }
}