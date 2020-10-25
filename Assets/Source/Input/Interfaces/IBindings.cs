using System.Collections.Generic;

namespace Assets.Source.Input.Interfaces
{
    public interface IBindings
    {
        /// <summary>
        /// The list of bindings and their key codes or gamepad codes
        /// </summary>
        Dictionary<string, IEnumerable<KeyCodeValue>> Bindings { get; set; }
    }
}
