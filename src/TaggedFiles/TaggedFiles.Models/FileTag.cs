namespace TaggedFiles.Models;

/// <summary>
/// Привязка тега к файлу.
/// </summary>
public class FileTag
{
  public FileTag()
  {

  }
  public FileTag(File file, Tag tag) : this()
  {
    Tag = tag;
    TagId = tag.Id;
    File = file;
    FileId = file.Id;
  }




  /// <summary>
  /// Identity
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Tag Identity
  /// </summary>
  public int TagId { get; set; }

  /// <summary>
  /// Тег, привязанный к файлу.
  /// </summary>
  public Tag Tag { get; set; }

  /// <summary>
  /// File Identity
  /// </summary>
  public int FileId { get; set; }

  /// <summary>
  /// Файл, привязанный к тегу.
  /// </summary>
  public File File { get; set; }



  public override string ToString() => $"{File?.Name ?? ""}{(File != null && Tag != null ? ": " : "")}{Tag?.Name ?? ""}";

}
