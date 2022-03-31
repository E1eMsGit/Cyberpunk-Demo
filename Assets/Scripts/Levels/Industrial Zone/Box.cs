using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private Sprite[] _boxPressedSprites;

    private SpriteRenderer _spriteRenderer;
    private int _pressedStage = 0;
    private bool _isHummerCollision = false;

    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hummer" && !_isHummerCollision)
        {
            _isHummerCollision = true;            
            _spriteRenderer.sprite = _boxPressedSprites[_pressedStage];
            _pressedStage = _pressedStage == 0 ? 1 : 0;          
        }
        else if (collision.gameObject.tag == "Abyss")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isHummerCollision = false;
    }
}