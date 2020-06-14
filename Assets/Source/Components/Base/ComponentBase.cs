using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Base
{
    public class ComponentBase : MonoBehaviour
    {
        #region overrides
        private void Awake()
        {
            ComponentAwake();
        }
        private void Start() { ComponentStart(); }
        private void Update() { ComponentUpdate(); }
        private void OnDestroy() { ComponentOnDestroy(); }
        private void OnEnable() { ComponentOnEnable(); }


        /// <summary>
        /// Override this method to add functionality to the monobehavior's Awake Method. 
        /// <para>This should be used for things such as setting references to components, etc</para>
        /// </summary>
        public virtual void ComponentAwake() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Start method
        /// <para>Used for setting up an object in the scene after all items are built and ready.</para>
        /// </summary>
        public virtual void ComponentStart() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's Update method
        /// <para>This is used for updates that happen every frame</para>
        /// </summary>
        public virtual void ComponentUpdate() { }

        /// <summary>
        /// Override this method to add functionality to the monobehavior's OnDestroy() method
        /// <para>Used for freeing up resources, etc</para>
        /// </summary>
        public virtual void ComponentOnDestroy() { }

        /// <summary>
        /// Override this method to add functionality to monobehaviour's OnEnable() method
        /// <para>This code is executed when an object is toggled to "active"</para>
        /// </summary>
        public virtual void ComponentOnEnable() { }
        #endregion
    }
}
