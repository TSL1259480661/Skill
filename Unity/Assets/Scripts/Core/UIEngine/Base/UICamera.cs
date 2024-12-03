using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace UIEngine
{
	public class UICamera
	{
		public Camera camera { private set; get; }

		private int layerUI;
		public int layerScene { private set; get; }
		private int layerAll;

		public UICamera()
		{
			layerUI = LayerMask.GetMask("UI");
			layerScene = LayerMask.GetMask("Board", "Role");
			layerAll = LayerMask.GetMask("UI", "Board", "Role");
		}

		public void Init(string name, CameraRenderType rendererType)
		{
			GameObject go = new GameObject(name);
			GameObject.DontDestroyOnLoad(go);

			go.transform.localPosition = new Vector3(0F, 0F, 0F);
			go.transform.localScale = Vector3.one;
			go.transform.localEulerAngles = Vector3.zero;

			camera = go.AddComponent<Camera>();
			camera.orthographic = true;
			camera.clearFlags = CameraClearFlags.SolidColor;
			camera.orthographicSize = 10;
			camera.GetUniversalAdditionalCameraData().renderType = rendererType;

			ShowAll();
		}

		public void ShowAll()
		{
			camera.cullingMask = layerAll;
		}

		public void ShowSceneObjects()
		{
			camera.cullingMask = layerScene;
		}

		public void ShowUI()
		{
			camera.cullingMask = layerUI;
		}

		public void SetFarClipPlane(int farClipPlane)
		{
			if (camera != null)
			{
				camera.farClipPlane = farClipPlane;
			}
		}

		public void AddOverlayCamera(Camera child)
		{
			if (child != null)
			{
				UniversalAdditionalCameraData cameraData = camera.GetUniversalAdditionalCameraData();
				if (cameraData.cameraStack != null && !cameraData.cameraStack.Exists(node => (node == child)))
				{
					cameraData.cameraStack.Add(child);
				}
			}
		}
	}
}
