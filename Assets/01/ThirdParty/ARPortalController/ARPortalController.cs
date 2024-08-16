using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

#if SEENSIOGO
using JKTechnologies.SeensioGo.Managers;
#endif

namespace JKTechnologies.SeensioGo.ARPortal {

    public class ARPortalController : MonoBehaviour {

        public Camera ARCamera { get { return mainCam; } }
        [SerializeField] private Camera mainCam;

        void Start() {
            AssignCamera();
        }

        public void AssignCamera() {
            if (mainCam == null) {
                #if SEENSIOGO
                mainCam = SeensioGoViewManager.Instance.arViewCamera;
                #endif 
            }

            if(mainCam!=null) {
                Debug.LogError("Found main camera");
            }
        }
    }
}