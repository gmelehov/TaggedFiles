using System.ComponentModel.DataAnnotations.Schema;


namespace TaggedFiles.Models;

/// <summary>
/// Файловый поисковый фильтр.
/// </summary>
public class FileFilter
{
  public FileFilter()
  {

  }
  public FileFilter(string field, string type, string value, string comparison = null) : this()
  {
    Field = field;
    Type = type;
    Value = value;
    Comparison = comparison;
  }
  public FileFilter(int id, int queryId, string field, string type, string value, string comparison = null) : this()
  {
    Field = field;
    Type = type;
    Value = value;
    Comparison = comparison;
    Id = id;
    QueryId = queryId;
  }



  /// <summary>
  /// Identity.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Имя поля (свойства) сущности.
  /// </summary>
  public string Field { get; set; }

  /// <summary>
  /// Тип данных поля.
  /// </summary>
  public string Type { get; set; }

  /// <summary>
  /// Сравниваемое значение.
  /// </summary>
  public string Value { get; set; }

  /// <summary>
  /// Тип сравнения с заданным значением.
  /// </summary>
  public string Comparison { get; set; }


  [NotMapped]
  public string Operation
  {
    get
    {
      switch (Comparison)
      {
        case "eq": return "==";
        case "ne": return "!=";
        case "gt": return Type == "date" || Type == "datetime" ? ">" : ">=";
        case "lt": return Type == "date" || Type == "datetime" ? "<" : "<=";

        case "before": return "<";
        case "after": return ">";
        case "on": return "==";


        default: return null;
      }
      ;
    }
  }


  [NotMapped]
  public string Condition
  {
    get
    {
      string ret;

      if (Type == "string")
      {
        var val = Value != null ? Value.ToString().EndsWith(@"\") ? Value.ToString().TrimEnd('\\') : Value.ToString() : null;
        switch (Comparison)
        {
          case "ends": ret = $"{Field}.EndsWith(\"{val}\")"; break;
          case "starts": ret = $"{Field}.StartsWith(\"{val}\")"; break;
          case "notends": ret = $"!{Field}.EndsWith(\"{val}\")"; break;
          case "notstarts": ret = $"!{Field}.StartsWith(\"{val}\")"; break;
          case "eq": ret = $"{Field} == \"{val}\""; break;
          case "ne": ret = $"{Field} != \"{val}\""; break;
          case "like": ret = $"{Field}.Contains(\"{val}\")"; break;
          case "notlike": ret = $"!{Field}.Contains(\"{val}\")"; break;
          case "isnull": ret = $"string.IsNullOrWhiteSpace({Field})"; break;
          case "isnotnull": ret = $"!string.IsNullOrWhiteSpace({Field})"; break;

          default: ret = $"{Field}.Contains(\"{val}\")"; break;
        }
        ;
      }
      else if (Type == "numeric")
      {
        ret = $"{Field} {Operation} {Value}";
      }
      else if (Type == "boolean")
      {
        ret = $"{Field} == {Value?.ToString()}";
      }
      else if (Type == "date" || Type == "datetime")
      {
        var list = Value?.ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        ret = $"{Field} {Operation} DateTime({list[2]}, {list[0]}, {list[1]})";
      }
      else if (Type == "list")
      {
        var list = Value?.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var str = $"\"{string.Join(",", list)}\"";
        ret = $"{str}.Contains({Field})";
      }
      else if (Type == "month")
      {
        var list = Value?.ToString().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var str = $"\"{string.Join(",", list)}\"";
        ret = $"{str}.Contains({Field}.Month.ToString(\"00\"))";
      }
      else
      {
        ret = $"{Field} {Operation} {Value?.ToString()}";
      }
      ;

      return ret;
    }
  }



  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int QueryId { get; set; }

  /// <summary>
  /// Ссылка на родительский запрос.
  /// </summary>
  public FileQuery Query { get; set; }

}