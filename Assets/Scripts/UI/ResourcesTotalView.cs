using TMPro;
using UnityEngine;

namespace UI
{
    public class ResourcesTotalView : MonoBehaviour
    {
        [SerializeField] private string _prefix;
        [SerializeField] private TextMeshProUGUI _totalText;

        public void Set(int amount)
        {
            _totalText.text = string.Format(_prefix, amount);
        }
    }
}