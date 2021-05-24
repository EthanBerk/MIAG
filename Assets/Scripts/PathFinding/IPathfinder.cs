using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

namespace Enemies
{
    public interface IPathfinder
    {
        public bool Gravity { get; set; }
        public Vector2 Size{ get; set; }
        public int jumpHeight { get; set; }
        public int jumpWidth { get; set; }

    }
}