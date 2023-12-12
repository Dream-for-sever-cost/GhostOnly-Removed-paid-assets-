using UnityEngine;
using Util;

public class MinimapCameraController : MonoBehaviour
{
    [SerializeField] private Camera _cam;

    private Transform _player;
    private UI_Game _ui;
    private bool _isExpand = false;

    private void Awake()
    {
        _ui = GameObject.Find(Constants.GameObjects.UI_Game).GetComponent<UI_Game>();
        _player = GameObject.Find(Constants.GameObjects.Player).transform;

        _player.GetComponent<PlayerController>().SetMinimapCam(this);
    }

    private void LateUpdate()
    {
        if (_isExpand)
        {
            transform.position = new Vector3(0, 0, -10);
        }
        else
        {
            if (_player.position.y >= 20)
            {
                transform.position = new Vector3(_player.position.x, 20f, -10f);
            }
            else if (_player.position.y <= -20)
            {
                transform.position = new Vector3(_player.position.x, -20f, -10f);
            }
            else
            {
                transform.position = new Vector3(_player.position.x, _player.position.y, -10);
            }
        }
    }

    public void ToggleExpand()
    {
        _isExpand = !_isExpand;
        _ui.ExpandMap(_isExpand);
        _cam.orthographicSize = _isExpand ? Constants.Setting.ExpandMinimapCamSize : Constants.Setting.NormalMinimapCamSize;
    }
}
