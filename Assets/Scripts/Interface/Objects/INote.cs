using UnityEngine;
public interface INote
{
    void Initialize(NoteKey key);
    void Hit();
    void Miss();
}
