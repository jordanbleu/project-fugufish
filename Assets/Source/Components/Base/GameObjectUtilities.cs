using Assets.Source.Components.Sound;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Base
{
    /// <summary>
    /// public exposed methods that provide additional functionality when hooking up unity events, animation events, etc
    /// </summary>
    public class GameObjectUtilities : ComponentBase
    {


        /// <summary>
        /// Outputs text into the console
        /// </summary>
        /// <param name="text"></param>
        public void Log(string text) {
            Debug.Log(text);
        }

        /// <summary>
        /// Destroys the game object this component is attached to
        /// </summary>
        public void KillSelf() {
            Destroy(gameObject);
        }

        /// <summary>
        /// Instantiates a game object at this object's position
        /// </summary>
        /// <param name="gameObj"></param>
        public void InstantiateAtMe(GameObject gameObj) {
            var inst = Instantiate(gameObj, transform.parent);
            inst.transform.position = transform.position;
        }

        /// <summary>
        /// Instantiates the object but leaves its position at the default prefab location
        /// </summary>
        /// <param name="gameObj"></param>
        public void InstantiateAtDefault(GameObject gameObj) {
            Instantiate(gameObj);
        }

        /// <summary>
        /// Plays audio on an external object.  Used to play audio after an object was destroyed. 
        /// </summary>
        /// <param name="clip"></param>
        public void PlayExternalAudio(AudioClip clip) {
            var oneTimeAudioPrefab = GetRequiredResource<GameObject>("Prefabs/System/OneTimeAudio");
            var inst = Instantiate(oneTimeAudioPrefab);
            inst.transform.position = transform.position;

            inst.name = $"AUDIO_CLIP: {clip.name}"; 

            var comp = GetRequiredComponent<OneTimeSoundSourceComponent>(inst);
            comp.PlaySound(clip);
        }

    }
}
