using UnityEngine;

namespace Assets.Source.Components.Parallax
{
    /// <summary>
    /// A stupider version of the parallax component that doesn't parallax but it works with different cinemachine cameras
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundRepeaterComponent : ComponentBase
    {

        [SerializeField]
        [Tooltip("how many repeated images to add horizontally")]
        private int repeatHorizontal = 2;

        [SerializeField]
        [Tooltip("how many repeated images to add vertically")]
        private int repeatVertical = 2;

        private SpriteRenderer spriteRenderer;


        public override void ComponentAwake()
        {
            spriteRenderer = GetRequiredComponent<SpriteRenderer>();

            var width = CalculateWidthInUnits();
            var height = CalculateHeightInUnits();

            // Create a template object to clone 
            var template = new GameObject("template");

            // Create a shallow copy of our sprite renderer
            var spriteRendererTemplate = template.AddComponent<SpriteRenderer>();
            spriteRendererTemplate.sprite = spriteRenderer.sprite;
            spriteRendererTemplate.color = spriteRenderer.color;
            spriteRendererTemplate.flipX = spriteRenderer.flipX;
            spriteRendererTemplate.flipY = spriteRenderer.flipY;
            spriteRendererTemplate.size = spriteRenderer.size;
            spriteRendererTemplate.drawMode = spriteRenderer.drawMode;
            spriteRendererTemplate.tileMode = spriteRenderer.tileMode;
            spriteRendererTemplate.spriteSortPoint = spriteRenderer.spriteSortPoint;
            spriteRendererTemplate.shadowCastingMode = spriteRendererTemplate.shadowCastingMode;

            spriteRendererTemplate.maskInteraction = spriteRenderer.maskInteraction;
            spriteRendererTemplate.spriteSortPoint = spriteRenderer.spriteSortPoint;
            spriteRendererTemplate.material = spriteRenderer.material;
            spriteRendererTemplate.sortingLayerID = spriteRenderer.sortingLayerID;
            spriteRendererTemplate.sortingOrder = spriteRenderer.sortingOrder;

            for (var x = -repeatHorizontal; x < repeatHorizontal+1; x++) {
                var w = width * x;
                
                // Make a new horizontal column
                var obj = AddBufferObject(template, $"Column: {x}", new Vector3(transform.position.x + w, transform.position.y, transform.position.z));

                // Create vertical rows
                for (var y = -repeatVertical; y < repeatVertical+1; y++) {
                    var h = height * y;

                    AddBufferObject(template, $"Row: {y}", new Vector3(transform.position.x + w, transform.position.y + h, transform.position.z), obj.transform);
                }
            }

            Destroy(template);

            base.ComponentAwake();
        }


        // Sprite's width in pixels / the pixels per unit
        private float CalculateWidthInUnits() => spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;

        // Sprites height in pixels / the pixels per unit
        private float CalculateHeightInUnits() => spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;

        // Duplicate an object and set some values
        private GameObject AddBufferObject(GameObject template, string name, Vector3 position, Transform parent = null)
        {
            var par = parent ?? transform;
            var dummyBottom = Instantiate(template, par);
            dummyBottom.name = name;
            dummyBottom.transform.position = position;
            return dummyBottom;
        }





    }
}
