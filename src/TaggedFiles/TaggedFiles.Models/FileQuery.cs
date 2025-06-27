using System.ComponentModel.DataAnnotations.Schema;


namespace TaggedFiles.Models;

/// <summary>
/// Файловый поисковый запрос.
/// </summary>
public class FileQuery
{
  public FileQuery()
  {
    
  }
  public FileQuery(string name, string descr = null) : this()
  {
    Name = name;
    Descr = descr;
  }
  public FileQuery(int id, string name, string descr = null, params FileFilter[] fileFilters) : this()
  {
    Id = id;
    Name = name;
    Descr = descr;
    Filters = fileFilters.ToList();
    Filters.ForEach(f =>
    {
      f.Query = this;
    });
  }




  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Название запроса.
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// Описание запроса.
  /// </summary>
  public string Descr { get; set; }



  [NotMapped]
  public string Predicate => Filters.Count > 0 ? string.Join(" AND ", Filters.Select(s => s.Condition)) : "true";



  /// <summary>
  /// Список фильтров.
  /// </summary>
  public List<FileFilter> Filters { get; set; } = [];

  /// <summary>
  /// Список привязок этого запроса к автоматическим теггерам.
  /// </summary>
  public List<AutoTaggerQuery> TaggerQueries { get; set; } = [];

}