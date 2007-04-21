namespace BlogEngine.Core.Entities
{
  /// <summary>
  /// A generic collection with the ability to 
  /// check if it has been changed.
  /// </summary>
  [System.Serializable]
  public class StateCollection<T> : System.Collections.ObjectModel.Collection<T>
  {

    #region Base overrides

    /// <summary>
    /// Inserts an element into the collection at the specified index and marks it changed.
    /// </summary>
    protected override void InsertItem(int index, T item)
    {
      base.InsertItem(index, item);
      _IsChanged = true;
    }

    /// <summary>
    /// Removes all the items in the collection and marks it changed.
    /// </summary>
    protected override void ClearItems()
    {
      base.ClearItems();
      _IsChanged = true;
    }

    /// <summary>
    /// Removes the element at the specified index and marks the collection changed.
    /// </summary>
    protected override void RemoveItem(int index)
    {
      base.RemoveItem(index);
      _IsChanged = true;
    }

    /// <summary>
    /// Replaces the element at the specified index and marks the collection changed.
    /// </summary>
    protected override void SetItem(int index, T item)
    {
      base.SetItem(index, item);
      _IsChanged = true;
    }

    #endregion

    private bool _IsChanged;
    /// <summary>
    /// Gets if this object's data has been changed.
    /// </summary>
    /// <returns>A value indicating if this object's data has been changed.</returns>
    public virtual bool IsChanged
    {
      get { return _IsChanged; }
      set { _IsChanged = value; }
    }

  }
}