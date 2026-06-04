using UnityEngine;

public class SpecialEventData<T>
{
    public T data {get; set;}
    public SpecialEventData(T _data){
        data = _data;
    }
}
