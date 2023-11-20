//   ==============================================================================
//   | Lynx Mixed Reality                                                         |
//   |======================================                                      |
//   | Lynx Planetarium                                                           |
//   | Simple script to recenter the scene automatically.                         |
//   ==============================================================================

using System.Collections;
using UnityEngine;

namespace Lynx.Planetarium
{
    public class AutoRecenter : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private RecenterManager recenterManager;

        #endregion

        #region UNITY API

        // Start is called before the first frame update.
        private void Start()
        {
            StartCoroutine(waitForTime());
        }

        #endregion

        #region PRIVATE METHODS

        /// Start this Coroutine to Reset to start after 0.5s.
        private IEnumerator waitForTime()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            recenterManager.ResetToStart();
        }

        #endregion
    }
}
