using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;

namespace Assets.Source.Components.UI
{
    /// <summary>
    /// Used exclusively as a joke.
    /// </summary>
    public class UsernameTextMeshProSetter : ComponentBase
    {
        
        public override void ComponentAwake()
        {
            var textMesh = GetRequiredComponent<TextMeshProUGUI>();
            textMesh.SetText(Environment.UserName);
            base.ComponentAwake();
        }

    }
}
