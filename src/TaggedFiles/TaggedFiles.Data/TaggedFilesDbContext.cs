using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaggedFiles.Models;
using TaggedFiles.Models.Enums;
using File = TaggedFiles.Models.File;


namespace TaggedFiles.Data;

/// <summary>
/// Контекст базы данных.
/// </summary>
public class TaggedFilesDbContext : DbContext
{
  public TaggedFilesDbContext() : base()
  {
    Database.EnsureCreated();
  }
  public TaggedFilesDbContext(DbContextOptions<TaggedFilesDbContext> opts) : base(opts)
  {
    Database.EnsureCreated();
  }





  public DbSet<Log> Logs { get; set; }
  
  public DbSet<Tag> Tags { get; set; }
  
  public DbSet<AutoTag> AutoTags { get; set; }
  
  public DbSet<AutoTagger> AutoTaggers { get; set; }
  
  public DbSet<AutoTaggerQuery> AutoTaggerQueries { get; set; }
  
  public DbSet<File> Files { get; set; }
  
  public DbSet<FileTag> FileTags { get; set; }
  
  public DbSet<FileFilter> FileFilters { get; set; }
  
  public DbSet<FileQuery> FileQueries { get; set; }
  
  public DbSet<FileWatcher> FileWatchers { get; set; }




  protected override void OnModelCreating(ModelBuilder mb)
  {

    mb.Entity<Log>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.WatcherId, e.ActionType, e.ObjId });
      ent.Property(e => e.ActionType).HasConversion(new EnumToStringConverter<ActionType>()).HasMaxLength(20);
      ent.Property(e => e.ObjType).HasConversion(new EnumToStringConverter<ObjectType>()).HasMaxLength(20);
      ent.Property(e => e.ObjName).HasMaxLength(300);
      ent.Property(e => e.NewName).IsRequired(false).HasMaxLength(300);
      ent.Property(e => e.Comment).IsRequired(false).HasMaxLength(500);
      ent.HasOne(e => e.FileWatcher).WithMany(w => w.Logs).HasForeignKey(e => e.WatcherId);
    });

    mb.Entity<Tag>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => e.Name).IsUnique(true);
      ent.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
    });
    
    mb.Entity<AutoTagger>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => e.Name);
      ent.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
      ent.Property(e => e.Descr).HasMaxLength(500);
    });
    
    mb.Entity<AutoTag>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.TaggerId, e.TagId }).IsUnique(true);
      ent.HasOne(e => e.Tag).WithMany(w => w.AutoTags).HasForeignKey(f => f.TagId).OnDelete(DeleteBehavior.Cascade);
      ent.HasOne(e => e.Tagger).WithMany(w => w.AutoTags).HasForeignKey(f => f.TaggerId).OnDelete(DeleteBehavior.Cascade);
    });
    
    mb.Entity<AutoTaggerQuery>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.TaggerId, e.QueryId }).IsUnique(true);
      ent.Property(e => e.Logic).IsRequired(true).HasMaxLength(10);
      ent.HasOne(e => e.Query).WithMany(w => w.TaggerQueries).HasForeignKey(f => f.QueryId).OnDelete(DeleteBehavior.Cascade);
      ent.HasOne(e => e.Tagger).WithMany(w => w.TaggerQueries).HasForeignKey(f => f.TaggerId).OnDelete(DeleteBehavior.Cascade);
    });
       
    mb.Entity<FileTag>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.FileId, e.TagId }).IsUnique(true);
      ent.HasOne(e => e.File).WithMany(w => w.FileTags).HasForeignKey(f => f.FileId).OnDelete(DeleteBehavior.Cascade);
      ent.HasOne(e => e.Tag).WithMany(w => w.FileTags).HasForeignKey(f => f.TagId).OnDelete(DeleteBehavior.Cascade);
    });
    
    mb.Entity<File>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.FullPath }).IsUnique(true);
      ent.Property(e => e.Path).IsRequired(true).HasMaxLength(500);
      ent.Property(e => e.Name).IsRequired(true).HasMaxLength(200);
      ent.Property(e => e.FullPath)
      //.HasComputedColumnSql("[Path]+[Name]", stored: true)
      ;
      ent.Property(e => e.Ext).HasMaxLength(50);
      ent.HasOne(e => e.FileWatcher).WithMany(w => w.Files).HasForeignKey(f => f.WatcherId);
    });
    
    mb.Entity<FileFilter>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => new { e.Field, e.Type, e.Value });
      ent.HasOne(e => e.Query).WithMany(w => w.Filters).HasForeignKey(f => f.QueryId).OnDelete(DeleteBehavior.Cascade);
      ent.Property(e => e.Field).IsRequired(true).HasMaxLength(50);
      ent.Property(e => e.Type).IsRequired(true).HasMaxLength(50);
      ent.Property(e => e.Value).HasMaxLength(200);
      ent.Property(e => e.Comparison).HasMaxLength(20);
    });

    mb.Entity<FileFilter>().HasData(
      new FileFilter(1, 1, "TagsJoin", "string", "", "isnull"),
      new FileFilter(2, 2, "Ext", "list", ".png,.gif,.jpg,.jpeg,.bmp,.tif,.ico", "eq"),
      new FileFilter(3, 2, "Ext", "string", "", "isnotnull"),
      new FileFilter(4, 2, "Ext", "string", "", "ne"),
      new FileFilter(5, 3, "Ext", "list", ".ttf,.woff,.woff2", "eq"),
      new FileFilter(6, 3, "Ext", "string", "", "isnotnull"),
      new FileFilter(7, 3, "Ext", "string", "", "ne"),
      new FileFilter(8, 3, "Ext", "string", ".tt", "ne"),
      new FileFilter(9, 4, "Ext", "string", ".sln", "eq"),
      new FileFilter(10, 5, "Ext", "string", ".dll", "eq"),
      new FileFilter(11, 6, "Ext", "string", ".xsd", "eq"),
      new FileFilter(12, 7, "Ext", "list", ".db,.sqlite", "eq"),
      new FileFilter(13, 7, "Ext", "string", "", "isnotnull"),
      new FileFilter(14, 7, "Ext", "string", "", "ne"),
      new FileFilter(15, 7, "Ext", "string", ".dbf", "ne"),
      new FileFilter(16, 8, "Ext", "string", ".js", "eq"),
      new FileFilter(17, 9, "Ext", "list", ".zip,.rar,.7z,.tar", "eq"),
      new FileFilter(18, 9, "Ext", "string", "", "isnotnull"),
      new FileFilter(19, 9, "Ext", "string", "", "ne"),
      new FileFilter(20, 10, "Ext", "string", ".ps1", "eq"),
      new FileFilter(21, 11, "Ext", "list", ".cer,.crt,.pem,.p7b,.p12,.pfx,.key", "eq"),
      new FileFilter(22, 11, "Ext", "string", "", "isnotnull"),
      new FileFilter(23, 11, "Ext", "string", "", "ne"),
      new FileFilter(24, 12, "Ext", "string", ".torrent", "eq")
      );
    
    mb.Entity<FileQuery>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => e.Name);
      ent.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
      ent.Property(e => e.Descr).HasMaxLength(500);
    });

    mb.Entity<FileQuery>().HasData(
      new FileQuery(1, "RawFiles", "Файлы, не имеющие никаких тегов"),
      new FileQuery(2, "PictureFiles", "Файлы с графикой/картинками/рисунками/изображениями/иконками"),
      new FileQuery(3, "FontFiles", "Файлы шрифтов"),
      new FileQuery(4, "VSSolutions", "Файлы, содержащие решения Visual Studio"),
      new FileQuery(5, "CompiledDLLs", "Файлы, содержащие скомпилированные сборки/библиотеки"),
      new FileQuery(6, "XSDFiles", "Файлы, содержащие XSD-схемы"),
      new FileQuery(7, "SQLiteFiles", "Файлы баз данных SQLite"),
      new FileQuery(8, "JSFiles", "Файлы, содержащие Javascript"),
      new FileQuery(9, "CompressedFiles", "Сжатые файлы (архивы)"),
      new FileQuery(10, "PowerShellFiles", "Файлы, содержащие сценарии PowerShell"),
      new FileQuery(11, "Certificates", "Файлы, содержащие SSL-сертификаты/приватные ключи"),
      new FileQuery(12, "TorrentFiles", "Торрент-файлы")
      );

    mb.Entity<FileWatcher>(ent =>
    {
      ent.HasKey(e => e.Id);
      ent.HasIndex(e => e.Path).IsUnique(true);
      ent.Property(e => e.Name).IsRequired(true).HasMaxLength(100);
      ent.Property(e => e.Path).IsRequired(true).HasMaxLength(500);
      ent.Property(e => e.Filter).IsRequired(true).HasMaxLength(50);
    });

    mb.Entity<FileWatcher>().HasData(
      new FileWatcher
      {
        Id = 1,
        Name = "TFTemp FileWatcher",
        Path = @"D:\TFTemp",
        Filter = "*.*",
        Enabled = true,
        BufferSize = 262144,
        IncludeSub = true,
      }
      );


    //mb.Entity<Tag>().HasData(Tag.InitData);
    //mb.Entity<Meta>().HasData(Meta.InitData);
    //mb.Entity<FileQuery>().HasData(FileQuery.InitData);
    //mb.Entity<FileFilter>().HasData(FileFilter.InitData);
  }

}
