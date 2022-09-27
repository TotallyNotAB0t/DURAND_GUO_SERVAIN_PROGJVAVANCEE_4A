using Enums;

namespace Interfaces
{
    public interface IInputProvider
    {
        public float GetAxis(Axis axis);
        public bool GetActionPressed(InputAction action);
    }
}
