//   ==============================================================================
//   | Lynx Mixed Reality                                                         |
//   |======================================                                      |
//   | Lynx Planetarium                                                           |
//   | Simple script to mange the hole effect in the planetarium sphere.          |
//   ==============================================================================

using UnityEngine;

namespace Lynx.Planetarium
{
    public class SkyboxHoleManager : MonoBehaviour
    {
        #region INSPECTOR VARIABLES

        [SerializeField] private Renderer skybox;
        [SerializeField] private GameObject userhead;

        #endregion

        #region UNITY API

        // Start is called before the first frame update.
        private void Start()
        {
            userhead = Camera.main.gameObject;
        }

        // Update is called once per frame.
        void Update()
        {
            Vector3 headPos = userhead.transform.position;
            skybox.material.SetVector("_HeadPos", new Vector4(headPos.x, headPos.y, headPos.z, 0F));
        }

        #endregion
    }
}
