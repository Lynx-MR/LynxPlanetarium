//   ==============================================================================
//   | Lynx Mixed Reality                                                         |
//   |======================================                                      |
//   | Lynx Planetarium                                                           |
//   | Simple script to ensure that the solar radiation quad faces the user.      |
//   ==============================================================================

using System.Collections.Generic;
using UnityEngine;

namespace Lynx.Planetarium
{
    public class SceneReset : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private GameObject[] planets;

        #endregion

        #region PRIVATE VARIABLES

        private Dictionary<Vector3, Quaternion> InitialTransforms = new Dictionary<Vector3, Quaternion>();

        #endregion

        #region UNITY API

        // Start is called before the first frame update.
        void Start()
        {
            foreach (GameObject go in planets)
            {
                Vector3 pos = new Vector3();
                Quaternion quat = new Quaternion();

                pos = go.transform.localPosition;
                quat = go.transform.localRotation;

                InitialTransforms.Add(pos, quat);
            }
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Call this function to reset all planets position and rotation.
        /// </summary>
        public void ResetSceneTransform()
        {
            planets[0].GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            planets[0].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            planets[0].transform.rotation = Quaternion.identity;
            int index = 0;

            foreach (KeyValuePair<Vector3, Quaternion> keyValue in InitialTransforms)
            {
                GameObject planet = planets[index];

                planet.transform.localPosition = keyValue.Key;
                planet.transform.localRotation = keyValue.Value;

                index++;
            }
        }

        #endregion
    }
}
