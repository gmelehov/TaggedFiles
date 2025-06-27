using TaggedFiles.Models.Enums;


namespace TaggedFiles.Models;

/// <summary>
/// Запись в журнале изменений.
/// </summary>
public class Log
{
  public Log()
  {

  }
  public Log(int objid, ObjectType objtype, ActionType actiontype) : this()
  {
    ObjId = objid;
    ObjType = objtype;
    ActionType = actiontype;
    DateTime = DateTime.Now;
  }
  public Log(int objid, string name, ObjectType objtype, ActionType actiontype) : this(objid, objtype, actiontype)
  {
    ObjName = name;
  }
  public Log(int objid, string name, string newname, ObjectType objtype, ActionType actiontype) : this(objid, name, objtype, actiontype)
  {
    NewName = newname;
  }
  public Log(int objid, string name, string newname, ObjectType objtype, ActionType actiontype, string comment) : this(objid, name, newname, objtype, actiontype)
  {
    Comment = comment;
  }




  /// <summary>
  /// Identity
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// Тип действий, совершенных с объектом.
  /// </summary>
  public ActionType ActionType { get; set; }

  /// <summary>
  /// Identity объекта, над которым было выполнено действие.
  /// </summary>
  public int? ObjId { get; set; }

  /// <summary>
  /// Тип объекта, над которым было выполнено действие.
  /// </summary>
  public ObjectType ObjType { get; set; }

  /// <summary>
  /// Имя объекта, над которым было выполнено действие.
  /// </summary>
  public string ObjName { get; set; }

  /// <summary>
  /// Новое имя объекта, в случае его переименования.
  /// </summary>
  public string NewName { get; set; }

  /// <summary>
  /// Момент выполнения действия.
  /// </summary>
  public DateTime DateTime { get; set; }

  /// <summary>
  /// Опционально: поясняющий текст.
  /// </summary>
  public string Comment { get; set; }

  /// <summary>
  /// Внешний ключ.
  /// </summary>
  public int WatcherId { get; set; }

  /// <summary>
  /// Ссылка на экземпляр FileWatcher'а, создавшего эту запись.
  /// </summary>
  public FileWatcher FileWatcher { get; set; }

}