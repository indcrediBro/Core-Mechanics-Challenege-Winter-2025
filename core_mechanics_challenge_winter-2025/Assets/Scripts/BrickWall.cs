using UnityEngine;

public class BrickWall : MonoBehaviour
{
    [SerializeField] private Sprite[] brickWallSprites;
    [SerializeField] private SpriteRenderer[] brickWallRenderer;

    public void OnEnable()
    {
        AllocateRandomeSprites();
    }

    public void OnDisable()
    {
        foreach (SpriteRenderer sr in brickWallRenderer)
        {
            sr.gameObject.SetActive(true);
        }
    }

    private void AllocateRandomeSprites()
    {
        foreach (SpriteRenderer sr in brickWallRenderer)
        {
            sr.sprite = brickWallSprites[Random.Range(0, brickWallSprites.Length)];
        }
    }
}
