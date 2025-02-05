using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourcesTotalView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _totalText;

        public void Set(int amount)
        {
            _totalText.text = amount.ToString();
        }
    }
}