namespace TaggedFiles.Models.Enums;

/// <summary>
/// Тип объекта базы данных
/// </summary>
public enum ObjectType
{

  /// <summary>
  /// Тип не известен/не указан/не применим.
  /// </summary>
  NONE = 0,
  /// <summary>
  /// Файл.
  /// </summary>
  FILE = 1,
  /// <summary>
  /// Папка.
  /// </summary>
  FOLDER,
  /// <summary>
  /// Метаданные файла.
  /// </summary>
  META,
  /// <summary>
  /// Тег (метка).
  /// </summary>
  TAG,
  /// <summary>
  /// Таксон.
  /// </summary>
  TAXON,
  /// <summary>
  /// Группа MIME-типов.
  /// </summary>
  MIMEGROUP,
  /// <summary>
  /// MIME-тип.
  /// </summary>
  MIMETYPE,
  /// <summary>
  /// Обозреватель изменений в файловой системе.
  /// </summary>
  WATCHER,
}