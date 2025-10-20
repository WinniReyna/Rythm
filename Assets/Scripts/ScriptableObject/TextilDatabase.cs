using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextilDatabase", menuName = "Textile/Textil Database")]
public class TextilDatabase : ScriptableObject
{
    public List<TextileData> textiles = new List<TextileData>();

    public TextileData GetTextilByID(int id)
    {
        return textiles.Find(t => t.id == id);
    }
}

