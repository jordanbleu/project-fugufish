using Assets.Source.Input;
using Assets.Source.Strings;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Source.Components.UI
{

    [RequireComponent(typeof(Collider2D))]
    public class InfoMessageTriggerComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("This is the ID of the xml node containing the message to display.  These will be pulled from different xml files depending on the current control scheme. " +
            "The XMLs are located under the 'strings' folder under {languageCode}/Global)")]
        private string tutorialMessageCode;

        [SerializeField]
        private InfoMessageComponent infoMessageComponent;

        // LazyLoad kinda
        private StringsLoader stringsLoader;

        // Store the type so we only reload strings if the type changes
        private Type activeInputListenerType;

        public override void ComponentAwake()
        {
            if (!UnityUtils.Exists(infoMessageComponent)) {
                throw new UnityException("You went and forgot to drag the reference to the info message thing");
            }

            // Sadly, there is a noticable hitch in gameplay when this is called. 
            // Which actually makes sense because its loading and parsing hella xml.  
            // todo: I think we should add event handling to the input manager and use that to invoke this method, since there's a bit of a hitch there anyway
            ReloadStrings();

            base.ComponentAwake();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsReloadRequired()) {
                ReloadStrings();
            }            

            if (stringsLoader.Value.TryGetValue(tutorialMessageCode, out var msg)) {
                infoMessageComponent.ShowMessage(msg);
            }
            else {
                throw new UnityException($"Unable to find required string resource with key '{tutorialMessageCode}'.  Please double check the strings xml files.");
            }
        }

        private void ReloadStrings() {

            if (stringsLoader == null) { 
                stringsLoader = new StringsLoader();
            }

            // Using keyboard so load the keyboard strings
            if (activeInputListenerType == typeof(KeyboardInputListener))
            {
                stringsLoader.Load("Global/tutorialMessagesKeyboard.xml");
            }
            else {
                stringsLoader.Load("Global/tutorialMessagesXbone.xml");
            }

        }

        private bool IsReloadRequired() {
            
            if (stringsLoader == null || stringsLoader.Value == null || !stringsLoader.Value.Any()) {
                return true;
            }

            if (activeInputListenerType != Input.GetActiveListener()?.GetType()) {
                activeInputListenerType = Input.GetActiveListener().GetType();
                return true;
            }

            return false;
        }

    }
}
