using UnityEngine;

public class BrickWall : MonoBehaviour
{
    [System.Serializable]
    private class WallSprite
    {
        public Sprite[] sprites;
    }

    [SerializeField] private WallSprite[] brickWallSprites;
    [SerializeField] private SpriteRenderer[] brickWallRenderer;

    public void OnEnable()
    {
        AllocateRandomeSprites();
    }

    private void AllocateRandomeSprites()
    {
        WallSprite wallSprite = brickWallSprites[Random.Range(0, brickWallSprites.Length)];
        for (int i = 0; i < brickWallRenderer.Length; i++)
        {
            var sr = brickWallRenderer[i];
            sr.sprite = wallSprite.sprites[i];
            sr.gameObject.SetActive(true);
        }
    }
}
