using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Source.Components.Parallax
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AutoScrollParallaxComponent : ComponentBase
    {
        [SerializeField]
        [Tooltip("Enables horizontal scrolling")]
        private float horizontalScrollSpeed = 0f;

        [SerializeField]
        [Tooltip("Enables vertical scrolling")]
        private float verticalScrollSpeed = 0f;

        private SpriteRenderer spriteRenderer;
        private float width;
        private float height;

        private Vector2 originalPosition;


        public override void ComponentPreStart()
        {
            spriteRenderer = GetRequiredComponent<SpriteRenderer>();
            originalPosition = transform.position;

            width = CalculateWidthInUnits();
            height = CalculateHeightInUnits();

            CreateBuffers();
            base.ComponentStart();
        }


        private void CreateBuffers()
        {
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

            // Create buffers to the left and right

            AddBufferObject(template, "LeftBuffer", new Vector3(transform.position.x - width, transform.position.y, transform.position.z));
            AddBufferObject(template, "RightBuffer", new Vector3(transform.position.x + width, transform.position.y, transform.position.z));
            

            // Create buffers on top and bottom
            AddBufferObject(template, "TopBuffer", new Vector3(transform.position.x, transform.position.y + height, transform.position.z));
            AddBufferObject(template, "BottomBuffer", new Vector3(transform.position.x, transform.position.y - height, transform.position.z));

            // Add diagonal buffers
            AddBufferObject(template, "TopLeftBuffer", new Vector3(transform.position.x - width, transform.position.y + height, transform.position.z));
            AddBufferObject(template, "TopRightBuffer", new Vector3(transform.position.x + width, transform.position.y + height, transform.position.z));
            AddBufferObject(template, "BottomLeftBuffer", new Vector3(transform.position.x - width, transform.position.y - height, transform.position.z));
            AddBufferObject(template, "BottomRightBuffer", new Vector3(transform.position.x + width, transform.position.y - height, transform.position.z));
                
            // No need for the template to exist in the hierarchy
            Destroy(template);
        }

        // Duplicate an object and set some values
        private void AddBufferObject(GameObject template, string name, Vector3 position)
        {
            var dummyBottom = Instantiate(template, gameObject.transform);
            dummyBottom.name = name;
            dummyBottom.transform.position = position;
        }

        // Sprite's width in pixels / the pixels per unit
        private float CalculateWidthInUnits() => spriteRenderer.sprite.rect.width / spriteRenderer.sprite.pixelsPerUnit;

        // Sprites height in pixels / the pixels per unit
        private float CalculateHeightInUnits() => spriteRenderer.sprite.rect.height / spriteRenderer.sprite.pixelsPerUnit;

        public override void ComponentUpdate()
        {
            var posX = transform.position.x;
            var posY = transform.position.y;

            posX -= horizontalScrollSpeed;
            posY -= verticalScrollSpeed;

            var width = CalculateWidthInUnits();
            var height = CalculateHeightInUnits();

            if (posX < originalPosition.x - width || posX > originalPosition.x+width) {
                posX = originalPosition.x;
            }

            if (posY < originalPosition.y - height || posY > originalPosition.y + height) {
                posY = originalPosition.y;
            }

            transform.position = new Vector3(posX, posY, transform.position.z);
        }


    }
}
