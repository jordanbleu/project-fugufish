using Assets.Source.Components.Level;
using Assets.Source.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Source.Components
{
    public class ComponentBase : MonoBehaviour
    {
        // all components maintain a cache of objects that they care about.
        private Dictionary<string, GameObject> objectCache = new Dictionary<string, GameObject>();
        private GameObject levelObject;
        private GameObject canvasObject; 

        #region overrides
        private void Awake()
        {
            levelObject = GetRequiredObject("Level"); 
            ComponentAwake();
        }
        private void Start() { ComponentStart(); }
        private void Update() { ComponentUpdate(); }
        private void OnDestroy() { ComponentOnDestroy(); }
        private void OnEnable() { ComponentOnEnable(); }

        private void FixedUpdate() { ComponentFixedUpdate(); }
        private void LateUpdate() { ComponentLateUpdate();  }

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

        public virtual void ComponentFixedUpdate() { }

        public virtual void ComponentLateUpdate() { }
        #endregion

        #region helpers
        /// <summary>
        /// Instantiates a prefab, maintaining the prefab's object name (dropping unity's "(Clone)" suffix).  The
        /// prefab will be instantiated in the prefab's default position
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <returns>The Instance</returns>
        public static GameObject InstantiatePrefab(GameObject prefab)
        {
            GameObject instance = Instantiate(prefab);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// Instantiates a prefab, maintainins the prefab's object name (dropping unity's "(Clone)" sufix).
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="parentTransform">The parent object in the hierarchy</param>
        /// <returns>The Instance</returns>
        public static GameObject InstantiatePrefab(GameObject prefab, Transform parentTransform)
        {
            GameObject instance = Instantiate(prefab, parentTransform);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// Instantiates a prefab, maintainins the prefab's object name (dropping unity's "(Clone)" sufix).
        /// Re-positions the object to the specified <paramref name="position"/> after instantiation
        /// </summary>
        /// <param name="prefab">Prefab to instantiate</param>
        /// <param name="position">The position to relocate the instance to</param>
        /// <param name="parentTransform">The parent object in the hierarchy</param>
        /// <returns>The Instance</returns>
        /// 
        public static GameObject InstantiatePrefab(GameObject prefab, Vector3 position, Transform parentTransform = null)
        {
            GameObject instance = Instantiate(prefab, parentTransform);
            instance.name = prefab.name;
            instance.transform.position = position;
            return instance;
        }

        /// <summary>
        /// Loads a component from the current gameObject, throwing an exception if one isn't found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        protected T GetRequiredComponent<T>() where T : Component
        {
            return GetRequiredComponent<T>(gameObject);
        }

        /// <summary>
        /// Loads a component off of another game object, throwing an exception if one isn't found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="otherObject"></param>
        public static T GetRequiredComponent<T>(GameObject otherObject) where T: Component
        {
            T component;
            try
            {
                component = otherObject.GetComponent<T>();
            }
            catch (MissingComponentException)
            {
                throw new MissingRequiredComponentException(otherObject, typeof(T));
            }
            return component;
        }

        /// <summary>
        /// Searches child game objects of the current game object for 
        /// the requested component.  Returns the first instance found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        protected T GetRequiredComponentInChildren<T>() where T : Component
        {
            return GetRequiredComponentInChildren<T>(gameObject);
        }

        /// <summary>
        /// Searches child game objects of the specified game object for 
        /// the requested component.  Returns the first instance found.
        /// </summary>
        /// <typeparam name="T">Component Type</typeparam>
        public static T GetRequiredComponentInChildren<T>(GameObject otherObject) where T : Component
        {
            T component;

            try
            {
                component = otherObject.GetComponentInChildren<T>(true);
            }
            catch (MissingComponentException)
            {
                throw new MissingRequiredComponentException(otherObject, typeof(T));
            }
            return component;
        }

        /// <summary>
        /// Loads a resource from unity's resources directory, or throws an exception if it is not found
        /// </summary>
        public static T GetRequiredResource<T>(string path) where T : UnityEngine.Object
        {
            T resource = Resources.Load<T>(path) as T
                ?? throw new MissingResourceException(path);

            return resource;
        }

        public List<GameObject> GetAllObjectsOnlyInScene()
        {
            // https://docs.unity3d.com/ScriptReference/Resources.FindObjectsOfTypeAll.html
            List<GameObject> objectsInScene = new List<GameObject>();

            foreach (GameObject obj in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
            {
                if (!EditorUtility.IsPersistent(obj.transform.root.gameObject) && !(obj.hideFlags == HideFlags.NotEditable || obj.hideFlags == HideFlags.HideAndDontSave))
                    objectsInScene.Add(obj);
            }

            return objectsInScene;
        }


        /// <summary>
        /// Finds an object located on the base of the hierarchy, or throws an exception if not found
        /// </summary>
        /// <param name="name">Name of the object to find</param>
        public GameObject GetRequiredObject(string name)
        {
            if (objectCache.ContainsKey(name)) {
                return objectCache[name];
            }

            // this method will grab all objects in the scene and return the one with your specified name
            // why can't we use GameObject.find() you ask?  Because of course it doesn't return inactive objects which causes many issues.  Which is stupid.
            var obj = GetAllObjectsOnlyInScene().FirstOrDefault(ob => ob.name.Equals(name));

            if (!UnityUtils.Exists(obj)) { 
                throw new MissingRequiredObjectException(gameObject.name, name);            
            }

            objectCache.Add(name, obj);
            return obj;
        }

        /// <summary>
        /// Gets a required child game object off the current game object, or throws an exception if not found
        /// </summary>
        protected GameObject GetRequiredChild(string name)
        {
            return GetRequiredChild(name, gameObject);
        }

        /// <summary>
        /// Gets a required child game object off the specified game object, or throws an exception if not found
        /// </summary>
        public static GameObject GetRequiredChild(string name, GameObject otherObject)
        {
            if (!UnityUtils.Exists(otherObject)) {
                throw new ArgumentException("otherObject can't be null", nameof(otherObject));
            }

            Transform transformObject = otherObject.transform.Find(name);

            if (!UnityUtils.Exists(transformObject))
            { 
                throw new MissingRequiredChildException(otherObject, name);
            }
            
            return transformObject.gameObject;
        }

        /// <summary>
        /// Return a component that is expected in a parent object.  Throws an exception if 
        /// it does not exist.
        /// </summary>
        /// <typeparam name="T">The component Type</typeparam>
        /// <param name="otherObject">the object to check from</param>
        /// <returns>The component</returns>
        public static T GetRequiredComponentInParent<T>(GameObject otherObject) where T : MonoBehaviour
        {
            return otherObject.GetComponentInParent<T>()
                ?? throw new MissingRequiredComponentException(otherObject, typeof(T));
        }

        /// <summary>
        /// Return a component that is expected in a parent object.  Throws an exception if 
        /// it does not exist.
        /// </summary>
        /// <typeparam name="T">The component Type</typeparam>
        /// <param name="otherObject">the object to check</param>
        /// <returns>The component</returns>
        protected T GetRequiredComponentInParent<T>() where T : MonoBehaviour
        {
            return GetRequiredComponentInParent<T>(this.gameObject);
        }

        /// <summary>
        /// Finds the canvas object in the hierarchy.  If it does not exist, creates it.
        /// </summary>
        /// <returns></returns>
        protected GameObject FindOrCreateCanvas()
        {
            if (canvasObject != null) {
                return canvasObject;
            }

            // There should only be one canvas
            canvasObject = GetRequiredObject("Canvas");

            if (canvasObject == null) {
                GameObject canvasPrefab = GetRequiredResource<GameObject>($"{ResourcePaths.PrefabsFolder}/System/Canvas");

                canvasObject = Instantiate(canvasPrefab);
            }

            return canvasObject;
        }

        #endregion

        #region Level

        /// <summary>
        /// Helper method that will instantate an object within the level object
        /// </summary>
        /// <returns></returns>
        public GameObject InstantiateInLevel(GameObject prefab, Vector3? position = null)
        {
            Vector3 pos = position ?? Vector3.zero;
            Transform parent = levelObject.transform;
            return InstantiatePrefab(prefab, pos, parent);
        }

        /// <summary>
        /// Creates a new game object with the specified components as a child of the level object
        /// </summary>
        /// <returns></returns>
        public GameObject InstantiateInLevel(string name, Vector3 position, params Type[] components)
        {
            GameObject inst = new GameObject(name, components);
            inst.transform.parent = levelObject.transform;
            inst.transform.position = position;
            return inst;
        }

        public InputManager Input => GetRequiredComponent<InputComponent>(levelObject).InputManager;

        #endregion
    }
}
