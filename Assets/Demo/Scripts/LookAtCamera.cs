//   ==============================================================================
//   | Lynx Mixed Reality                                                         |
//   |======================================                                      |
//   | Lynx Planetarium                                                           |
//   | Simple script to ensure that the solar radiation quad faces the user.      |
//   ==============================================================================

using UnityEngine;

namespace Lynx.Planetarium
{
    public class LookAtCamera : MonoBehaviour
    {
        #region UNITY API

        // Update is called once per frame.
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }

        #endregion
    }
}