namespace TaggedFiles.Models;

/// <summary>
/// Автоматический теггер файлов.
/// </summary>
public class AutoTagger
{


  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Имя автотеггера.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Описание автотеггера.
  /// </summary>
  public string Descr { get; set; }


  /// <summary>
  /// Список файловых запросов, формирующих предикат для выборки файлов, подлежащих тегированию.
  /// </summary>
  public List<AutoTaggerQuery> TaggerQueries { get; set; } = [];

  /// <summary>
  /// Список привязок тегов к этому автотеггеру.
  /// </summary>
  public List<AutoTag> AutoTags { get; set; } = [];

}
