// FlaxVoxels (c) 2018-2019 Damian 'Erdroy' Korczowski

using System.Collections.Generic;
using System.Linq;
using FlaxEngine;

namespace FlaxVoxels.Gameplay
{
    /// <inheritdoc />
    /// <summary>
    /// FreeCameraController script. Provides basic camera controls.
    /// </summary>
	public class FreeCameraController : Script
    {
        private Vector3 _targetRotation;
        private Vector3 _targetPosition;
        private readonly List<Vector2> _deltaBuffer = new List<Vector2>();
        private Vector2 _lastDelta = Vector2.Zero;
        private Vector2 _lastCursorPosition = Vector2.Zero;

        public override void OnStart()
        {
            base.OnStart();
            _targetPosition = Transform.Translation;
        }

        public override void OnUpdate()
        {
            base.OnUpdate(); 
            if (Input.GetMouseButtonDown(MouseButton.Right))
                _lastCursorPosition = Input.MousePosition;

            if (!Input.GetMouseButton(MouseButton.Right))
                return;

            UpdateLook();
            UpdateMovement();

            Actor.Orientation = Quaternion.RotationYawPitchRoll(_targetRotation.X * Mathf.DegreesToRadians, _targetRotation.Y * Mathf.DegreesToRadians, _targetRotation.Z * Mathf.DegreesToRadians);
            Actor.Position = Vector3.Lerp(Actor.Position, _targetPosition, CameraSmoothing * Time.DeltaTime);
        }

        private void UpdateLook()
        {
            var cursorDelta = (Input.MousePosition - _lastCursorPosition) * MouseSensitivity;
            _lastCursorPosition = Input.MousePosition;
            
            if (MouseFiltering)
            {
                // Update buffer
                _deltaBuffer.Add(cursorDelta);

                while (_deltaBuffer.Count > MouseFilteringFrames)
                    _deltaBuffer.RemoveAt(0);

                cursorDelta = _deltaBuffer.Aggregate(Vector2.Zero, (current, delta) => current + delta);
                cursorDelta /= _deltaBuffer.Count;
            }

            if (MouseAcceleration)
            {
                var tmp = cursorDelta;
                cursorDelta = (cursorDelta + _lastDelta) * MouseAccelerationMultiplier;
                _lastDelta = tmp;
            }

            _targetRotation += new Vector3(cursorDelta.X / 10.0f, cursorDelta.Y / 10.0f, 0.0f);

            // Clamp Y
            _targetRotation.Y = Mathf.Clamp(_targetRotation.Y, -89.9f, 89.9f);
        }

        private void UpdateMovement()
        {
            var direction = Vector3.Zero;
            var transform = Transform;
            var currentSpeed = 10.0f;

            if (Input.GetKey(KeyboardKeys.Shift))
                currentSpeed *= 2.5f;

            if (Input.GetKey(KeyboardKeys.Control))
                currentSpeed *= 0.05f;

            if (Input.GetKey(KeyboardKeys.W))
                direction += transform.Forward;
            if (Input.GetKey(KeyboardKeys.S))
                direction -= transform.Forward;
            if (Input.GetKey(KeyboardKeys.A))
                direction -= transform.Right;
            if (Input.GetKey(KeyboardKeys.D))
                direction += transform.Right;
            if (Input.GetKey(KeyboardKeys.E))
                direction += transform.Up;
            if (Input.GetKey(KeyboardKeys.Q))
                direction -= transform.Up;

            direction.Normalize();
            direction *= currentSpeed;

            direction *= 100.0f; // Flax is in centimeters

            _targetPosition += direction * Time.DeltaTime;
        }
        
        public bool MouseFiltering { get; set; } = true;

        public int MouseFilteringFrames { get; set; } = 3;

        public bool MouseAcceleration { get; set; } = true;

        public float MouseAccelerationMultiplier { get; set; } = 0.7f;

        public float MouseSensitivity { get; set; } = 2.0f;

        public float CameraSmoothing { get; set; } = 20.0f;
    }
}
