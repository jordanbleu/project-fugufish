using Assets.Editor.Attributes;
using Assets.Source.Components;
using Assets.Source.Components.Timer;
using Assets.Source.Input.Constants;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    [RequireComponent(typeof(IntervalTimerComponent))]
    public class TextWriterComponent : ComponentBase
    {
        [SerializeField]
        private TextMeshProUGUI textMesh;
        
        private IntervalTimerComponent timer;

        [ReadOnly]
        [SerializeField]
        private string fullString = string.Empty;

        private StringBuilder displayString;

        public override void ComponentAwake()
        {
            if (!UnityUtils.Exists(textMesh)) {
                throw new UnityException("You didn't drag the text mesh pro object");
            }

            timer = GetRequiredComponent<IntervalTimerComponent>();
            timer.IsActive = true;

            SetText("hey paulie spaghetti tony burroni");
            base.ComponentAwake();
        }

        public override void ComponentUpdate()
        {
            if (displayString != null) {
                textMesh.SetText(displayString.ToString());
            }

            if (Input.IsKeyPressed(InputConstants.K_INTERACT))
            {
                if (displayString != null && displayString.Length < fullString.Length)
                {
                    displayString.Clear();
                    displayString.Append(fullString);
                }
                else {
                    Destroy(gameObject);
                }
            }
            base.ComponentUpdate();
        }

        public void TimerReached() 
        {
            if (displayString != null)
            {
                if (displayString.Length < fullString.Length)
                {
                    displayString.Append(fullString[displayString.Length]);
                }
            }
            // beep
        }

        public void SetText(string newText)
        {
            fullString = newText;
            displayString = new StringBuilder(fullString.Length);
            
        }
    }
}
