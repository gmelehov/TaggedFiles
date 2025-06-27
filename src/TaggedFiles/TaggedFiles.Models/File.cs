using System.ComponentModel.DataAnnotations.Schema;


namespace TaggedFiles.Models;

/// <summary>
/// Файл.
/// </summary>
public class File
{
  private string _fullpath;


  public File()
  {
    
  }
  public File(FileInfo fileInfo) : this()
  {
    Path = fileInfo.DirectoryName;
    Name = fileInfo.Name;
    Ext = fileInfo.Extension;
    Length = fileInfo.Length;
    Created = fileInfo.CreationTime;
    Changed = fileInfo.LastWriteTime;
  }
  public File(FileWatcher watcher, FileInfo fileInfo) : this(fileInfo)
  {
    FileWatcher = watcher;
    WatcherId = watcher.Id;
  }





  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Полный путь к папке, содержащей этот файл.
  /// </summary>
  public string Path { get; set; }

  /// <summary>
  /// Собственное имя файла.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Полный путь к файлу, включая его собственное имя.
  /// </summary>
  public string FullPath
  {
    get => System.IO.Path.Combine(Path, Name);
    set => _fullpath = value;
  }

  /// <summary>
  /// Расширение файла.
  /// </summary>
  public string Ext { get; set; }

  /// <summary>
  /// Размер файла, в байтах.
  /// </summary>
  public long Length { get; set; }

  /// <summary>
  /// Момент создания файла.
  /// </summary>
  public DateTime? Created { get; set; }

  /// <summary>
  /// Момент изменения файла.
  /// </summary>
  public DateTime? Changed { get; set; }

  /// <summary>
  /// Момент переименования файла.
  /// </summary>
  public DateTime? Renamed { get; set; }

  /// <summary>
  /// Список всех привязанных к файлу тегов, объединенный в единую строку.
  /// </summary>
  [NotMapped]
  public string TagsJoin => string.Join(", ", FileTags?.Select(s => s.Tag?.Name).OrderBy(ord => ord).ToList());

  /// <summary>
  /// Список привязок тегов к этому файлу.
  /// </summary>
  public List<FileTag> FileTags { get; set; } = [];

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int WatcherId { get; set; }

  /// <summary>
  /// Ссылка на экземпляр FileWatcher'а, отслеживающего изменения в этом файле.
  /// </summary>
  public FileWatcher FileWatcher { get; set; }




  public override string ToString() => $@"{Path}\{Name}".Replace(@"\\", @"\");

}
