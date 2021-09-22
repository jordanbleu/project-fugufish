using Assets.Source.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    /// <summary>
    /// Creates TextWriters until it runs out of texts and then kills self
    /// </summary>
    public class TextWriterQueueComponent : ComponentBase
    {

        [SerializeField]
        private GameObject textWriterPrefab;

        private Queue<string> texts;
        private bool isActivated = false;
        private GameObject textWriterInstance;

        public override void ComponentPreStart()
        {
            if (!UnityUtils.Exists(textWriterPrefab)) {
                throw new UnityException("Please drag the text writer prefab onto the text writer queue prefab");
            }
            base.ComponentPreStart();
        }

        public override void ComponentUpdate()
        {
            if (isActivated)
            {
                // if our instantiated text writer is destroyed, make another one
                if (!UnityUtils.Exists(textWriterInstance))
                {
                    // If our queue has any more texts
                    if (texts.Any())
                    {
                        var nextText = texts.Dequeue();
                        textWriterInstance = Instantiate(textWriterPrefab, transform.parent);
                        var textWriter = GetRequiredComponent<TextWriterComponent>(textWriterInstance);
                        textWriter.SetText(nextText);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
                
            }
            base.ComponentUpdate();
        }

        /// <summary>
        /// Sets the queue of messages to display and activates self (so it will auto-destroy itself properly)
        /// </summary>
        /// <param name="texts"></param>
        public void InitQueue(Queue<string> texts) {
            this.texts = texts;
            isActivated = true;
        }


    }
}
