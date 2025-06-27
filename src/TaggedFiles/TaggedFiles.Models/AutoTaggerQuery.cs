namespace TaggedFiles.Models;

/// <summary>
/// Привязка автотеггера к файловому поисковому запросу.
/// </summary>
public class AutoTaggerQuery
{



  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Логический оператор.
  /// </summary>
  public string Logic { get; set; }

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int TaggerId { get; set; }

  /// <summary>
  /// Ссылка на автоматический теггер.
  /// </summary>
  public AutoTagger Tagger { get; set; }

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int QueryId { get; set; }

  /// <summary>
  /// Ссылка на файловый запрос.
  /// </summary>
  public FileQuery Query { get; set; }

}
