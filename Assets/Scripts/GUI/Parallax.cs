using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{   
    [SerializeField] private Vector2 _parallaxEffectMultiplier;
    [SerializeField] private bool _isInfiniteHorizontal;
    [SerializeField] private bool _isInfiniteVertical;
    
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;
    private float _textureUnitSizeX;
    private float _textureUnitSizeY;

    void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPosition = _cameraTransform.position;

        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        _textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        _textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * _parallaxEffectMultiplier.x, deltaMovement.y * _parallaxEffectMultiplier.y);
        _lastCameraPosition = _cameraTransform.position;


        if (_isInfiniteHorizontal)
        {
            if (Mathf.Abs(_cameraTransform.position.x - transform.position.x) >= _textureUnitSizeX)
            {
                float offsetPositionX = (_cameraTransform.position.x - transform.position.x) % _textureUnitSizeX;
                transform.position = new Vector3(_cameraTransform.position.x + offsetPositionX, transform.position.y);
            }
        }

        if (_isInfiniteVertical)
        {
            if (Mathf.Abs(_cameraTransform.position.y - transform.position.y) >= _textureUnitSizeY)
            {
                float offsetPositionY = (_cameraTransform.position.y - transform.position.y) % _textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, _cameraTransform.position.y + offsetPositionY);
            }
        }
    }
}
