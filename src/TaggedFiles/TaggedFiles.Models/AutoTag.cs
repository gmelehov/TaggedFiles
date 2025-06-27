namespace TaggedFiles.Models;

/// <summary>
/// Привязка тега к автотеггеру.
/// </summary>
public class AutoTag
{
  public AutoTag()
  {

  }
  public AutoTag(AutoTagger tagger, Tag tag) : this()
  {
    Tag = tag;
    TagId = tag.Id;
    Tagger = tagger;
    TaggerId = tagger.Id;
  }



  /// <summary>
  /// Identity
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int TagId { get; set; }

  /// <summary>
  /// Тег, привязанный к автотеггеру.
  /// </summary>
  public Tag Tag { get; set; }

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int TaggerId { get; set; }

  /// <summary>
  /// Автотеггер, привязанный к тегу.
  /// </summary>
  public AutoTagger Tagger { get; set; }

}