using TMPro;
using UnityEngine;

namespace Physics.Scripts
{
    public class GoalUiElement : MonoBehaviour
    {
        public static GoalUiElement Instance;
        private int _score = 0;
        private TextMeshPro _textRef;

        private void Start()
        {
            _textRef = GetComponent<TextMeshPro>();
        }

        private void Awake()
        {
            Instance = this;
        }

        public void IncrementScore()
        {
            _score++;
            _textRef.text = $"Score<br> {_score}";
        }
    }
}
