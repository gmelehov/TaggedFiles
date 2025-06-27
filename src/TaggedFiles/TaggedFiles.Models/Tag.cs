namespace TaggedFiles.Models;

/// <summary>
/// Тег (метка).
/// </summary>
public class Tag
{




  /// <summary>
  /// Identity
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Идентификатор (имя/значение) тега.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Список привязок файлов к этому тегу.
  /// </summary>
  public List<FileTag> FileTags { get; set; }

  /// <summary>
  /// Список привязок этого тега к автотеггерам.
  /// </summary>
  public List<AutoTag> AutoTags { get; set; }




  public override string ToString() => $"{Name}";

}
