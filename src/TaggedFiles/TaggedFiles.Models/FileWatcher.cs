using System.ComponentModel.DataAnnotations.Schema;

namespace TaggedFiles.Models;

/// <summary>
/// Настройки экземпляра FileWatcher'а.
/// </summary>
public class FileWatcher
{





  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Название экземпляра.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Полный путь к отслеживаемой папке.
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// Фильтр типов отслеживаемых файлов.
  /// </summary>
  public string Filter { get; set; }

  /// <summary>
  /// Экземпляр задействован?
  /// </summary>
  public bool Enabled { get; set; }

  /// <summary>
  /// Отслеживать подпапки?
  /// </summary>
  public bool IncludeSub { get; set; }

  /// <summary>
  /// Размер буфера, байт.
  /// </summary>
  public int BufferSize { get; set; }

  /// <summary>
  /// Экземпляр активен?
  /// </summary>
  public bool IsActive { get; set; }

  /// <summary>
  /// Общее количество файлов, отслеживаемых этим обозревателем.
  /// </summary>
  [NotMapped]
  public int FilesCount => Files?.Count ?? 0;

  /// <summary>
  /// Общее количество событий, зафиксированных этим обозревателем.
  /// </summary>
  [NotMapped]
  public int LogsCount => Logs?.Count ?? 0;


  [NotMapped]
  public string FilterInfo => IncludeSub ? $"{Filter} (включая подпапки)" : $"{Filter} (без подпапок)";


  /// <summary>
  /// Список файлов, отслеживаемых этим экземпляром.
  /// </summary>
  public List<File> Files { get; set; } = [];

  /// <summary>
  /// Список записей в журнале, созданных этим экземпляром.
  /// </summary>
  public List<Log> Logs { get; set; } = [];

}