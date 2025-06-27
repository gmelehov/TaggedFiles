using TaggedFiles.Data;
using TaggedFiles.Models.Services;
using TaggedFiles.Services;

namespace TaggedFiles.WorkerService;


public class Worker(IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
  private readonly IServiceScopeFactory _scopeFactory = serviceScopeFactory;
  private string tempName;
  private string tempFullPath;
  private string newTempName;
  private string newTempFullPath;
  private readonly List<FileSystemWatcher> fileSystemWatchers = new List<FileSystemWatcher>();





  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using IServiceScope scope = _scopeFactory.CreateScope();
    var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

    var watchers = _taggedFilesService.GetAllFileWatchers();

    foreach (var watcher in watchers)
    {
      var fw = new FileSystemWatcher(watcher.Path, watcher.Filter);
      fw.IncludeSubdirectories = watcher.IncludeSub;
      fw.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.CreationTime;
      fw.InternalBufferSize = watcher.BufferSize;
      fw.EnableRaisingEvents = true;

      fw.Changed += async (o, e) => await Watcher_Changed(o, e);
      fw.Deleted += async (o, e) => await Watcher_Deleted(o, e);
      fw.Created += async (o, e) => await Watcher_Created(o, e);
      fw.Renamed += async (o, e) => await Watcher_Renamed(o, e);


      fileSystemWatchers.Add(fw);

      watcher.IsActive = true;
      _taggedFilesService.LogWatcherStarted(watcher);

    }

    while (!stoppingToken.IsCancellationRequested)
    {
      await Task.Delay(200, stoppingToken);
    }
  }



  public override async Task StopAsync(CancellationToken cancellationToken)
  {
    using IServiceScope scope = _scopeFactory.CreateScope();
    var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

    var watchers = _taggedFilesService.GetAllFileWatchers();
    foreach (var watcher in watchers)
    {
      watcher.IsActive = false;
      _taggedFilesService.LogWatcherStopped(watcher);
    }

    foreach (var fw in fileSystemWatchers)
    {
      fw.EnableRaisingEvents = false;
      fw?.Dispose();
    }
    await Task.CompletedTask;
  }






  private async Task Watcher_Renamed(object sender, RenamedEventArgs e)
  {
    bool renamedToTmp = e.Name.StartsWith(e.OldName) && e.FullPath.StartsWith(e.OldFullPath);
    bool renamedBack = e.Name == tempName && e.FullPath == tempFullPath;
    bool directoryChanged = System.IO.Directory.Exists(e.FullPath) && !System.IO.File.Exists(e.FullPath);

    using IServiceScope scope = _scopeFactory.CreateScope();
    var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

    if (renamedToTmp)
    {
      tempName = e.OldName;
      tempFullPath = e.OldFullPath;
      newTempName = e.Name;
      newTempFullPath = e.FullPath;
    }
    else if (renamedBack)
    {
      if (!directoryChanged)
      {
        var fw = _taggedFilesService.GetFileWatcherByPath(e.OldFullPath.Replace(e.OldName, "").TrimEnd('\\'));
        if (fw != null)
        {
          _taggedFilesService.RenameWatcherFile(e.OldFullPath, e.FullPath, fw);
        }
      }

      tempName = null;
      tempFullPath = null;
    }
    else if (!directoryChanged)
    {
      var fw = _taggedFilesService.GetFileWatcherByPath(e.OldFullPath.Replace(e.OldName, "").TrimEnd('\\'));
      if (fw != null)
      {
        _taggedFilesService.RenameWatcherFile(e.OldFullPath, e.FullPath, fw);
      }
    }

    await Task.CompletedTask;
  }


  private async Task Watcher_Changed(object sender, FileSystemEventArgs e)
  {
    bool tempFileChanged = e.Name.EndsWith("~") || e.Name.EndsWith(".temp");
    bool directoryChanged = Directory.Exists(e.FullPath) && !File.Exists(e.FullPath);

    if (tempName == null && tempFullPath == null && newTempName == null && newTempFullPath == null && !tempFileChanged && !directoryChanged)
    {
      using IServiceScope scope = _scopeFactory.CreateScope();
      var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

      var fw = _taggedFilesService.GetFileWatcherByPath(e.FullPath.Replace(e.Name, "").TrimEnd('\\'));
      if (fw != null)
      {
        _taggedFilesService.UpdateWatcherFile(e.FullPath, fw);
      }
    }

    await Task.CompletedTask;
  }


  private async Task Watcher_Created(object sender, FileSystemEventArgs e)
  {
    bool createdTmp = tempName == e.Name && tempFullPath == e.FullPath;
    bool tempFileCreated = e.Name.EndsWith("~") || e.Name.EndsWith(".TMP") || e.Name.EndsWith(".temp");
    bool directoryChanged = Directory.Exists(e.FullPath) && !File.Exists(e.FullPath);

    if (!createdTmp && !tempFileCreated && !directoryChanged)
    {
      using IServiceScope scope = _scopeFactory.CreateScope();
      var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

      var fw = _taggedFilesService.GetFileWatcherByPath(e.FullPath.Replace(e.Name, "").TrimEnd('\\'));
      if (fw != null)
      {
        _taggedFilesService.AddWatcherFile(e.FullPath, fw);
      }
    }

    await Task.CompletedTask;
  }


  private async Task Watcher_Deleted(object sender, FileSystemEventArgs e)
  {
    bool deletedTmp = e.Name == newTempName && e.FullPath == newTempFullPath;
    bool tempFileDeleted = e.Name.EndsWith("~") || e.Name.EndsWith(".TMP") || e.Name.EndsWith(".temp");
    bool directoryChanged = Directory.Exists(e.FullPath) && !File.Exists(e.FullPath);

    if (deletedTmp)
    {
      newTempName = null;
      newTempFullPath = null;
    }
    else if (!tempFileDeleted && !directoryChanged)
    {
      using IServiceScope scope = _scopeFactory.CreateScope();
      var _taggedFilesService = scope.ServiceProvider.GetRequiredService<ITaggedFilesService>();

      var fw = _taggedFilesService.GetFileWatcherByPath(e.FullPath.Replace(e.Name, "").TrimEnd('\\'));
      if (fw != null)
      {
        _taggedFilesService.DeleteWatcherFile(e.FullPath, fw);
      }
    }

    await Task.CompletedTask;
  }

  
  
  
}
