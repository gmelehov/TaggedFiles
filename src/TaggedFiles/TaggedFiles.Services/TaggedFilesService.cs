using Microsoft.EntityFrameworkCore;
using TaggedFiles.Data;
using TaggedFiles.Models;
using TaggedFiles.Models.Enums;
using TaggedFiles.Models.Services;


namespace TaggedFiles.Services;

/// <summary>
/// Реализация сервиса для работы с данными.
/// </summary>
public class TaggedFilesService(TaggedFilesDbContext taggedFilesDbContext) : ITaggedFilesService
{
  private readonly TaggedFilesDbContext _dbContext = taggedFilesDbContext;





  public IEnumerable<FileWatcher> GetAllFileWatchers() => _dbContext.FileWatchers.AsEnumerable().ToList();

  public void AddTagsToFile(File file, params Tag[] tags)
  {
    _dbContext.FileTags.AddRange(tags.Where(w => !IsFileTaggedBy(file, w)).Select(s => new FileTag
    {
      File = file,
      FileId = file.Id,
      Tag = s,
      TagId = s.Id
    }));
    _dbContext.SaveChanges();
  }

  public void AddTagToFiles(Tag tag, params IEnumerable<File> files)
  {
    var fileTags = files
      .Where(w => !IsFileTaggedBy(w, tag))
      .Select(s => new FileTag
      {
        File = s,
        FileId = s.Id,
        Tag = tag,
        TagId = tag.Id
      });

    _dbContext.FileTags.AddRange(fileTags);
    _dbContext.SaveChanges();
  }

  public void AddWatcherFile(string fullPath, FileWatcher fileWatcher)
  {
    if (!IsFileExists(fullPath))
    {
      var fileinfo = new FileInfo(fullPath);
      var newfile = new File(fileinfo);
      fileWatcher.Files.Add(newfile);
      _dbContext.FileWatchers.Update(fileWatcher);
      _dbContext.SaveChanges();

      LogAction(fileWatcher, ActionType.CREATED, ObjectType.FILE, newfile.Id, fullPath, null);
    }
  }

  public void ClearWatcherFiles(FileWatcher fileWatcher)
  {
    fileWatcher.Files.RemoveAll(match => true);
    _dbContext.FileWatchers.Update(fileWatcher);
    _dbContext.SaveChanges();
  }

  public void ClearWatcherLogs(FileWatcher fileWatcher)
  {
    fileWatcher.Logs.RemoveAll(match => true);
    _dbContext.FileWatchers.Update(fileWatcher);
    _dbContext.SaveChanges();
  }

  public void DeleteWatcherFile(string fullPath, FileWatcher watcher)
  {
    if (IsFileExists(fullPath))
    {
      var file = GetFileByPath(fullPath);
      if(file != null)
      {
        var fileId = file.Id;
        watcher.Files.Remove(file);
        _dbContext.FileWatchers.Update(watcher);
        _dbContext.SaveChanges();

        LogAction(watcher, ActionType.DELETED, ObjectType.FILE, fileId, fullPath, null);
      }
    }
  }

  public File GetFileByPath(string path, bool includeAllEntities = true) => 
    includeAllEntities ?
    _dbContext.Files
    .Where(w => w.FullPath == path.Replace(@"\\", @"\"))
    .Include(i => i.FileWatcher)
    .Include(i => i.FileTags)
    .ThenInclude(t => t.Tag)
    .ThenInclude(t => t.AutoTags)
    .ThenInclude(t => t.Tagger)
    .ThenInclude(t => t.TaggerQueries)
    .ThenInclude(t => t.Query)
    .ThenInclude(t => t.Filters)
    .AsEnumerable()
    .FirstOrDefault()
    :
    _dbContext.Files
    .Where(w => w.FullPath == path.Replace(@"\\", @"\"))
    .AsEnumerable()
    .FirstOrDefault()
    ;

  public IEnumerable<FileInfo> GetFileInfosFromPath(string path, string filter, bool includeSubs)
  {
    var dirinfo = new DirectoryInfo(path);
    if (dirinfo != null)
    {
      return dirinfo
        .GetFiles(filter, includeSubs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)
        .Where(w => !w.Attributes.HasFlag(FileAttributes.Hidden) && !w.Attributes.HasFlag(FileAttributes.System))
        .ToList()
        ;
    }
    else
    {
      return null;
    }
    ;
  }

  public FileWatcher GetFileWatcherById(int watcherId) => _dbContext.FileWatchers.FirstOrDefault(f => f.Id == watcherId);

  public FileWatcher GetFileWatcherByPath(string path) => _dbContext.FileWatchers.FirstOrDefault(w => w.Path == path);

  public bool IsFileExists(string path) => _dbContext.Files.AsEnumerable().Any(a => a.FullPath == path.Replace(@"\\", @"\"));

  public bool IsFileTaggedBy(File file, params Tag[] tags) => tags.All(a => a != null && file.FileTags.Any(t => t.FileId == file.Id && t.TagId == a.Id));

  public bool IsPathWatched(FileWatcher fileWatcher, string path) => path.StartsWith(fileWatcher.Path);

  public bool IsWatcherForPathExists(string path)
  {
    throw new NotImplementedException();
  }

  public void LogAction(FileWatcher watcher, ActionType action, ObjectType objectType, int objId, string objName, string newName, string comment = null)
  {
    var log = new Log
    {
      ActionType = action,
      ObjId = objId,
      ObjType = objectType,
      ObjName = objName,
      NewName = newName,
      DateTime = DateTime.Now,
      Comment = comment,
      FileWatcher = watcher,
      WatcherId = watcher.Id
    };

    _dbContext.Logs.Add(log);
    _dbContext.SaveChanges();
  }

  public void LogWatcherCreated(FileWatcher fileWatcher)
  {
    var log = new Log
    {
      ActionType = ActionType.CREATED,
      ObjType = ObjectType.WATCHER,
      DateTime = DateTime.Now,
      ObjName = fileWatcher.Name,
      NewName = fileWatcher.Path,
      ObjId = fileWatcher.Id,
      Comment = $"Экземпляр службы создан, в базу данных загружено файлов - {fileWatcher.FilesCount}",
      WatcherId = fileWatcher.Id
    };

    _dbContext.Logs.Add(log);
    _dbContext.SaveChanges();
  }

  public void LogWatcherStarted(FileWatcher fileWatcher)
  {
    var log = new Log
    {
      ActionType = ActionType.STARTED,
      ObjType = ObjectType.WATCHER,
      DateTime = DateTime.Now,
      ObjName = fileWatcher.Name,
      NewName = fileWatcher.Path,
      ObjId = fileWatcher.Id,
      Comment = $"Экземпляр службы запущен",
      WatcherId= fileWatcher.Id
    };

    _dbContext.Logs.Add(log);
    _dbContext.FileWatchers.Update(fileWatcher);
    _dbContext.SaveChanges();
  }

  public void LogWatcherStopped(FileWatcher fileWatcher)
  {
    var log = new Log
    {
      ActionType = ActionType.STOPPED,
      ObjType = ObjectType.WATCHER,
      DateTime = DateTime.Now,
      ObjName = fileWatcher.Name,
      NewName = fileWatcher.Path,
      ObjId = fileWatcher.Id,
      Comment = $"Экземпляр службы остановлен",
      WatcherId = fileWatcher.Id
    };

    _dbContext.Logs.Add(log);
    _dbContext.FileWatchers.Update(fileWatcher);
    _dbContext.SaveChanges();
  }

  public void RemoveTagFromFiles(Tag tag, params IEnumerable<File> files)
  {
    var fileids = files.Select(s => s.Id).ToList();
    var ftags = _dbContext.FileTags.Where(w => fileids.Contains(w.FileId) && w.TagId == tag.Id).ToList();
    _dbContext.FileTags.RemoveRange(ftags);
    _dbContext.SaveChanges();
  }

  public void RemoveTagsFromFile(File file, params Tag[] tags)
  {
    bool removeAllTags = tags == null || !tags.Any();
    var tagIds = tags.Select(s => s.Id).ToList();
    var ftags = _dbContext.FileTags.Where(w => (removeAllTags ? true : tagIds.Contains(w.TagId)) && w.FileId == file.Id).ToList();
    _dbContext.FileTags.RemoveRange(ftags);
    _dbContext.SaveChanges();
  }

  public void RenameWatcherFile(string oldFullPath, string newFullPath, FileWatcher fileWatcher)
  {
    if(IsFileExists(oldFullPath) && !IsFileExists(newFullPath))
    {
      var oldFile = GetFileByPath(oldFullPath, false);
      var fileinfo = new System.IO.FileInfo(newFullPath);
      RenameFileFromFileInfo(oldFile, fileinfo);
      oldFile.Created = fileinfo.CreationTime;
      oldFile.Changed = fileinfo.LastWriteTime;
      _dbContext.Files.Update(oldFile);
      _dbContext.SaveChanges();

      LogAction(fileWatcher, ActionType.RENAMED, ObjectType.FILE, oldFile.Id, oldFullPath, newFullPath);
    }
  }

  public void UpdateWatcherFile(string fullPath, FileWatcher watcher)
  {
    if (IsFileExists(fullPath))
    {
      var file = GetFileByPath(fullPath, false);
      var fileinfo = new System.IO.FileInfo(fullPath);
      file.Changed = fileinfo.LastWriteTime;
      file.Length = fileinfo.Length;
      _dbContext.Files.Update(file);
      _dbContext.SaveChanges();

      LogAction(watcher, ActionType.UPDATED, ObjectType.FILE, file.Id, fullPath, null);
    }
  }







  private File RenameFileFromFileInfo(File file, FileInfo fileInfo)
  {
    file.Path = fileInfo.DirectoryName;
    file.Name = fileInfo.Name;
    file.Ext = fileInfo.Extension;
    file.Length = fileInfo.Length;
    file.Renamed = DateTime.Now;

    return file;
  }



}
