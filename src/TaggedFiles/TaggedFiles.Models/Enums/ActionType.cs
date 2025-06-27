using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaggedFiles.Models.Enums;

/// <summary>
/// Тип изменений, применяемых к объектам базы данных.
/// </summary>
public enum ActionType
{

  /// <summary>
  /// Тип не известен/не указан/не применим.
  /// </summary>
  NONE = 0,
  /// <summary>
  /// Создано.
  /// </summary>
  CREATED = 1,
  /// <summary>
  /// Удалено.
  /// </summary>
  DELETED,
  /// <summary>
  /// Обновлено/изменено.
  /// </summary>
  UPDATED,
  /// <summary>
  /// Переименовано.
  /// </summary>
  RENAMED,
  /// <summary>
  /// Начато.
  /// </summary>
  STARTED,
  /// <summary>
  /// Остановлено.
  /// </summary>
  STOPPED,
  /// <summary>
  /// Инсталлировано.
  /// </summary>
  INSTALLED,
  /// <summary>
  /// Деинсталлировано.
  /// </summary>
  UNINSTALLED,

}
