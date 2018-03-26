//在原有对item的管理上增加了是否使用的bool判定
public class ObjectPoolContainer<T>
{
    private T item;

    public bool Used { get; private set; }

    public void Consume()
    {
        Used = true;
    }

    public void Release()
    {
        Used = false;
    }

    public T Item
    {
        get
        {
            return item;
        }
        set
        {
            item = value;
        }        
    }
}
