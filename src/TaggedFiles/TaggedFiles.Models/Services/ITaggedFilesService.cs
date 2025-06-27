using TaggedFiles.Models.Enums;


namespace TaggedFiles.Models.Services;

/// <summary>
/// Интерфейс сервиса для работы с данными.
/// </summary>
public interface ITaggedFilesService
{




  /// <summary>
  /// Создает запись в журнале событий.
  /// </summary>
  /// <param name="watcher">FileWatcher.</param>
  /// <param name="action">Тип действия/события.</param>
  /// <param name="objectType">Тип контекстного объекта.</param>
  /// <param name="objId">Идентификатор контекстного объекта.</param>
  /// <param name="objName">Имя контекстного объекта/путь к файлу.</param>
  /// <param name="newName">Новое имя контекстного объекта/новый путь к файлу.</param>
  /// <param name="comment">Опционально: комментарий к записи.</param>
  void LogAction(FileWatcher watcher, ActionType action, ObjectType objectType, int objId, string objName, string newName, string comment = null);

  /// <summary>
  /// Создает запись о создании нового экземпляра FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Созданный экземпляр FileWatcher.</param>
  void LogWatcherCreated(FileWatcher fileWatcher);

  /// <summary>
  /// Создает запись о запуске экземпляра FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Запущенный экземпляр FileWatcher.</param>
  void LogWatcherStarted(FileWatcher fileWatcher);

  /// <summary>
  /// Создает запись об остановке экземпляра FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Остановленный экземпляр FileWatcher.</param>
  void LogWatcherStopped(FileWatcher fileWatcher);

  /// <summary>
  /// Удаляет из базы данных все файлы, отслеживаемые указанным экземпляром FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Экземпляр FileWatcher.</param>
  void ClearWatcherFiles(FileWatcher fileWatcher);

  /// <summary>
  /// Удаляет из базы данных все записи в журнале событий, 
  /// созданные указанным экземпляром FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Экземпляр FileWatcher.</param>
  void ClearWatcherLogs(FileWatcher fileWatcher);

  /// <summary>
  /// Возвращает результат проверки, расположен ли каталог/файл с указанным полным именем
  /// в папке, которая отслеживается указанным экземпляром FileWatcher.
  /// </summary>
  /// <param name="fileWatcher">Экземпляр FileWatcher.</param>
  /// <param name="path">Путь к каталогу/файлу.</param>
  /// <returns></returns>
  bool IsPathWatched(FileWatcher fileWatcher, string path);

  /// <summary>
  /// Возвращает результат проверки, существует ли FileWatcher для указанного пути.
  /// </summary>
  /// <param name="path">Полный путь к папке.</param>
  /// <returns></returns>
  bool IsWatcherForPathExists(string path);

  /// <summary>
  /// Возвращает FileWatcher по указанному идентификатору.
  /// </summary>
  /// <param name="watcherId">Идентификатор FileWatcher.</param>
  /// <returns></returns>
  FileWatcher GetFileWatcherById(int watcherId);

  /// <summary>
  /// Возвращает FileWatcher по указанному отслеживаемому пути.
  /// </summary>
  /// <param name="path"></param>
  /// <returns></returns>
  FileWatcher GetFileWatcherByPath(string path);

  /// <summary>
  /// Добавляет файл по указанному полному пути к указанному FileWatcher.
  /// </summary>
  /// <param name="fullPath"></param>
  /// <param name="fileWatcher"></param>
  void AddWatcherFile(string fullPath, FileWatcher fileWatcher);

  /// <summary>
  /// Переименовывает файл по указанному полному пути в указанном FileWatcher.
  /// </summary>
  /// <param name="oldFullPath"></param>
  /// <param name="newFullPath"></param>
  /// <param name="fileWatcher"></param>
  void RenameWatcherFile(string oldFullPath, string newFullPath, FileWatcher fileWatcher);

  /// <summary>
  /// Удаляет файл по указанному полному пути из указанного FileWatcher.
  /// </summary>
  /// <param name="fullPath"></param>
  /// <param name="watcher"></param>
  void DeleteWatcherFile(string fullPath, FileWatcher watcher);

  /// <summary>
  /// Обновляет файл по указанному полному пути в указанном FileWatcher.
  /// </summary>
  /// <param name="fullPath"></param>
  /// <param name="watcher"></param>
  void UpdateWatcherFile(string fullPath, FileWatcher watcher);

  /// <summary>
  /// Возвращает все экземпляры FileWatcher из базы данных.
  /// </summary>
  /// <returns></returns>
  IEnumerable<FileWatcher> GetAllFileWatchers();





  /// <summary>
  /// Проверяет существование в базе данных записи о файле по указанному полному пути.
  /// </summary>
  /// <param name="path">Полный путь к файлу.</param>
  /// <returns></returns>
  bool IsFileExists(string path);

  /// <summary>
  /// Возвращает файл из базы данных по указанному полному пути.
  /// </summary>
  /// <param name="path">Полный путь к файлу.</param>
  /// <param name="includeAllEntities">Возвращать все данные о файле, включая связанные сущности?</param>
  /// <returns></returns>
  File GetFileByPath(string path, bool includeAllEntities = true);

  /// <summary>
  /// Помечает файл в базе данных указанными тегами.
  /// </summary>
  /// <param name="file">Файл в базе данных.</param>
  /// <param name="tags">Список тегов.</param>
  void AddTagsToFile(File file, params Tag[] tags);

  /// <summary>
  /// Добавляет указанный тег ко всем указанным файлам в базе данных.
  /// </summary>
  /// <param name="tag">Тег.</param>
  /// <param name="files">Список файлов в базе данных.</param>
  void AddTagToFiles(Tag tag, params IEnumerable<File> files);

  /// <summary>
  /// Удаляет указанный тег из всех указанных файлов в базе данных.
  /// </summary>
  /// <param name="tag">Тег.</param>
  /// <param name="files">Список файлов в базе данных.</param>
  void RemoveTagFromFiles(Tag tag, params IEnumerable<File> files);

  /// <summary>
  /// Удаляет из файла в базе данных указанные теги.
  /// </summary>
  /// <param name="file">Файл в базе данных.</param>
  /// <param name="tags">Список тегов.</param>
  void RemoveTagsFromFile(File file, params Tag[] tags);

  /// <summary>
  /// Проверяет, помечен ли файл в базе данных всеми указанными тегами.
  /// </summary>
  /// <param name="file">Файл в базе данных.</param>
  /// <param name="tags">Список тегов.</param>
  /// <returns></returns>
  bool IsFileTaggedBy(File file, params Tag[] tags);

  /// <summary>
  /// Возвращает список объектов FileInfo из указанной папки, соответствующих
  /// указанному поисковому фильтру.
  /// </summary>
  /// <param name="path">Полный путь к папке.</param>
  /// <param name="filter">Поисковый фильтр.</param>
  /// <param name="includeSubs">Признак включения подпапок в поиск.</param>
  /// <returns></returns>
  IEnumerable<FileInfo> GetFileInfosFromPath(string path, string filter, bool includeSubs);









}
