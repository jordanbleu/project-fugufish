using Assets.Source.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.TextWriter
{
    public class TextWriterShowerComponent : ComponentBase
    {
        
        public void ShowDialog(string xml) {
            var existing = FindObjectOfType<TextWriterComponent>();

            if (!UnityUtils.Exists(existing))
            {
                var textWriterQueuePrefab = GetRequiredResource<GameObject>("Prefabs/UI/TextWriterQueue");

                var stringLoader = new StringsLoader();
                stringLoader.Load(xml);

                var textWriterQueueInstance  = Instantiate(textWriterQueuePrefab, FindOrCreateCanvas().transform);
                var textQueueComp = GetRequiredComponent<TextWriterQueueComponent>(textWriterQueueInstance);
                textQueueComp.InitQueue(new Queue<string>(stringLoader.Value.Values));
            }

        }



    }
}
