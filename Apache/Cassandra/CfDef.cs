/**
 * Autogenerated by Thrift Compiler (0.7.0-dev)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;
namespace Apache.Cassandra
{

  [Serializable]
  public partial class CfDef : TBase
  {
    private string _keyspace;
    private string _name;
    private string _column_type;
    private string _comparator_type;
    private string _subcomparator_type;
    private string _comment;
    private double _row_cache_size;
    private double _key_cache_size;
    private double _read_repair_chance;
    private List<ColumnDef> _column_metadata;
    private int _gc_grace_seconds;
    private string _default_validation_class;
    private int _id;
    private int _min_compaction_threshold;
    private int _max_compaction_threshold;
    private int _row_cache_save_period_in_seconds;
    private int _key_cache_save_period_in_seconds;
    private int _memtable_throughput_in_mb;
    private double _memtable_operations_in_millions;
    private bool _replicate_on_write;
    private double _merge_shards_chance;
    private string _key_validation_class;
    private string _row_cache_provider;
    private byte[] _key_alias;
    private string _compaction_strategy;
    private Dictionary<string, string> _compaction_strategy_options;

    public string Keyspace
    {
      get
      {
        return _keyspace;
      }
      set
      {
        __isset.keyspace = true;
        this._keyspace = value;
      }
    }

    public string Name
    {
      get
      {
        return _name;
      }
      set
      {
        __isset.name = true;
        this._name = value;
      }
    }

    public string Column_type
    {
      get
      {
        return _column_type;
      }
      set
      {
        __isset.column_type = true;
        this._column_type = value;
      }
    }

    public string Comparator_type
    {
      get
      {
        return _comparator_type;
      }
      set
      {
        __isset.comparator_type = true;
        this._comparator_type = value;
      }
    }

    public string Subcomparator_type
    {
      get
      {
        return _subcomparator_type;
      }
      set
      {
        __isset.subcomparator_type = true;
        this._subcomparator_type = value;
      }
    }

    public string Comment
    {
      get
      {
        return _comment;
      }
      set
      {
        __isset.comment = true;
        this._comment = value;
      }
    }

    public double Row_cache_size
    {
      get
      {
        return _row_cache_size;
      }
      set
      {
        __isset.row_cache_size = true;
        this._row_cache_size = value;
      }
    }

    public double Key_cache_size
    {
      get
      {
        return _key_cache_size;
      }
      set
      {
        __isset.key_cache_size = true;
        this._key_cache_size = value;
      }
    }

    public double Read_repair_chance
    {
      get
      {
        return _read_repair_chance;
      }
      set
      {
        __isset.read_repair_chance = true;
        this._read_repair_chance = value;
      }
    }

    public List<ColumnDef> Column_metadata
    {
      get
      {
        return _column_metadata;
      }
      set
      {
        __isset.column_metadata = true;
        this._column_metadata = value;
      }
    }

    public int Gc_grace_seconds
    {
      get
      {
        return _gc_grace_seconds;
      }
      set
      {
        __isset.gc_grace_seconds = true;
        this._gc_grace_seconds = value;
      }
    }

    public string Default_validation_class
    {
      get
      {
        return _default_validation_class;
      }
      set
      {
        __isset.default_validation_class = true;
        this._default_validation_class = value;
      }
    }

    public int Id
    {
      get
      {
        return _id;
      }
      set
      {
        __isset.id = true;
        this._id = value;
      }
    }

    public int Min_compaction_threshold
    {
      get
      {
        return _min_compaction_threshold;
      }
      set
      {
        __isset.min_compaction_threshold = true;
        this._min_compaction_threshold = value;
      }
    }

    public int Max_compaction_threshold
    {
      get
      {
        return _max_compaction_threshold;
      }
      set
      {
        __isset.max_compaction_threshold = true;
        this._max_compaction_threshold = value;
      }
    }

    public int Row_cache_save_period_in_seconds
    {
      get
      {
        return _row_cache_save_period_in_seconds;
      }
      set
      {
        __isset.row_cache_save_period_in_seconds = true;
        this._row_cache_save_period_in_seconds = value;
      }
    }

    public int Key_cache_save_period_in_seconds
    {
      get
      {
        return _key_cache_save_period_in_seconds;
      }
      set
      {
        __isset.key_cache_save_period_in_seconds = true;
        this._key_cache_save_period_in_seconds = value;
      }
    }

    public int Memtable_throughput_in_mb
    {
      get
      {
        return _memtable_throughput_in_mb;
      }
      set
      {
        __isset.memtable_throughput_in_mb = true;
        this._memtable_throughput_in_mb = value;
      }
    }

    public double Memtable_operations_in_millions
    {
      get
      {
        return _memtable_operations_in_millions;
      }
      set
      {
        __isset.memtable_operations_in_millions = true;
        this._memtable_operations_in_millions = value;
      }
    }

    public bool Replicate_on_write
    {
      get
      {
        return _replicate_on_write;
      }
      set
      {
        __isset.replicate_on_write = true;
        this._replicate_on_write = value;
      }
    }

    public double Merge_shards_chance
    {
      get
      {
        return _merge_shards_chance;
      }
      set
      {
        __isset.merge_shards_chance = true;
        this._merge_shards_chance = value;
      }
    }

    public string Key_validation_class
    {
      get
      {
        return _key_validation_class;
      }
      set
      {
        __isset.key_validation_class = true;
        this._key_validation_class = value;
      }
    }

    public string Row_cache_provider
    {
      get
      {
        return _row_cache_provider;
      }
      set
      {
        __isset.row_cache_provider = true;
        this._row_cache_provider = value;
      }
    }

    public byte[] Key_alias
    {
      get
      {
        return _key_alias;
      }
      set
      {
        __isset.key_alias = true;
        this._key_alias = value;
      }
    }

    public string Compaction_strategy
    {
      get
      {
        return _compaction_strategy;
      }
      set
      {
        __isset.compaction_strategy = true;
        this._compaction_strategy = value;
      }
    }

    public Dictionary<string, string> Compaction_strategy_options
    {
      get
      {
        return _compaction_strategy_options;
      }
      set
      {
        __isset.compaction_strategy_options = true;
        this._compaction_strategy_options = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool keyspace;
      public bool name;
      public bool column_type;
      public bool comparator_type;
      public bool subcomparator_type;
      public bool comment;
      public bool row_cache_size;
      public bool key_cache_size;
      public bool read_repair_chance;
      public bool column_metadata;
      public bool gc_grace_seconds;
      public bool default_validation_class;
      public bool id;
      public bool min_compaction_threshold;
      public bool max_compaction_threshold;
      public bool row_cache_save_period_in_seconds;
      public bool key_cache_save_period_in_seconds;
      public bool memtable_throughput_in_mb;
      public bool memtable_operations_in_millions;
      public bool replicate_on_write;
      public bool merge_shards_chance;
      public bool key_validation_class;
      public bool row_cache_provider;
      public bool key_alias;
      public bool compaction_strategy;
      public bool compaction_strategy_options;
    }

    public CfDef() {
      this._column_type = "Standard";
      this._comparator_type = "BytesType";
      this._row_cache_size = 0;
      this._key_cache_size = 200000;
      this._read_repair_chance = 1;
      this._row_cache_provider = "org.apache.cassandra.cache.ConcurrentLinkedHashCacheProvider";
    }

    public void Read (TProtocol iprot)
    {
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.String) {
              Keyspace = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Name = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Column_type = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.String) {
              Comparator_type = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 6:
            if (field.Type == TType.String) {
              Subcomparator_type = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 8:
            if (field.Type == TType.String) {
              Comment = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 9:
            if (field.Type == TType.Double) {
              Row_cache_size = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 11:
            if (field.Type == TType.Double) {
              Key_cache_size = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 12:
            if (field.Type == TType.Double) {
              Read_repair_chance = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 13:
            if (field.Type == TType.List) {
              {
                Column_metadata = new List<ColumnDef>();
                TList _list29 = iprot.ReadListBegin();
                for( int _i30 = 0; _i30 < _list29.Count; ++_i30)
                {
                  ColumnDef _elem31 = new ColumnDef();
                  _elem31 = new ColumnDef();
                  _elem31.Read(iprot);
                  Column_metadata.Add(_elem31);
                }
                iprot.ReadListEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 14:
            if (field.Type == TType.I32) {
              Gc_grace_seconds = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 15:
            if (field.Type == TType.String) {
              Default_validation_class = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 16:
            if (field.Type == TType.I32) {
              Id = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 17:
            if (field.Type == TType.I32) {
              Min_compaction_threshold = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 18:
            if (field.Type == TType.I32) {
              Max_compaction_threshold = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 19:
            if (field.Type == TType.I32) {
              Row_cache_save_period_in_seconds = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 20:
            if (field.Type == TType.I32) {
              Key_cache_save_period_in_seconds = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 22:
            if (field.Type == TType.I32) {
              Memtable_throughput_in_mb = iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 23:
            if (field.Type == TType.Double) {
              Memtable_operations_in_millions = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 24:
            if (field.Type == TType.Bool) {
              Replicate_on_write = iprot.ReadBool();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 25:
            if (field.Type == TType.Double) {
              Merge_shards_chance = iprot.ReadDouble();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 26:
            if (field.Type == TType.String) {
              Key_validation_class = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 27:
            if (field.Type == TType.String) {
              Row_cache_provider = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 28:
            if (field.Type == TType.String) {
              Key_alias = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 29:
            if (field.Type == TType.String) {
              Compaction_strategy = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 30:
            if (field.Type == TType.Map) {
              {
                Compaction_strategy_options = new Dictionary<string, string>();
                TMap _map32 = iprot.ReadMapBegin();
                for( int _i33 = 0; _i33 < _map32.Count; ++_i33)
                {
                  string _key34;
                  string _val35;
                  _key34 = iprot.ReadString();
                  _val35 = iprot.ReadString();
                  Compaction_strategy_options[_key34] = _val35;
                }
                iprot.ReadMapEnd();
              }
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("CfDef");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Keyspace != null && __isset.keyspace) {
        field.Name = "keyspace";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Keyspace);
        oprot.WriteFieldEnd();
      }
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Name);
        oprot.WriteFieldEnd();
      }
      if (Column_type != null && __isset.column_type) {
        field.Name = "column_type";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Column_type);
        oprot.WriteFieldEnd();
      }
      if (Comparator_type != null && __isset.comparator_type) {
        field.Name = "comparator_type";
        field.Type = TType.String;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Comparator_type);
        oprot.WriteFieldEnd();
      }
      if (Subcomparator_type != null && __isset.subcomparator_type) {
        field.Name = "subcomparator_type";
        field.Type = TType.String;
        field.ID = 6;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Subcomparator_type);
        oprot.WriteFieldEnd();
      }
      if (Comment != null && __isset.comment) {
        field.Name = "comment";
        field.Type = TType.String;
        field.ID = 8;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Comment);
        oprot.WriteFieldEnd();
      }
      if (__isset.row_cache_size) {
        field.Name = "row_cache_size";
        field.Type = TType.Double;
        field.ID = 9;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Row_cache_size);
        oprot.WriteFieldEnd();
      }
      if (__isset.key_cache_size) {
        field.Name = "key_cache_size";
        field.Type = TType.Double;
        field.ID = 11;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Key_cache_size);
        oprot.WriteFieldEnd();
      }
      if (__isset.read_repair_chance) {
        field.Name = "read_repair_chance";
        field.Type = TType.Double;
        field.ID = 12;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Read_repair_chance);
        oprot.WriteFieldEnd();
      }
      if (Column_metadata != null && __isset.column_metadata) {
        field.Name = "column_metadata";
        field.Type = TType.List;
        field.ID = 13;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.Struct, Column_metadata.Count));
          foreach (ColumnDef _iter36 in Column_metadata)
          {
            _iter36.Write(oprot);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      if (__isset.gc_grace_seconds) {
        field.Name = "gc_grace_seconds";
        field.Type = TType.I32;
        field.ID = 14;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Gc_grace_seconds);
        oprot.WriteFieldEnd();
      }
      if (Default_validation_class != null && __isset.default_validation_class) {
        field.Name = "default_validation_class";
        field.Type = TType.String;
        field.ID = 15;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Default_validation_class);
        oprot.WriteFieldEnd();
      }
      if (__isset.id) {
        field.Name = "id";
        field.Type = TType.I32;
        field.ID = 16;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Id);
        oprot.WriteFieldEnd();
      }
      if (__isset.min_compaction_threshold) {
        field.Name = "min_compaction_threshold";
        field.Type = TType.I32;
        field.ID = 17;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Min_compaction_threshold);
        oprot.WriteFieldEnd();
      }
      if (__isset.max_compaction_threshold) {
        field.Name = "max_compaction_threshold";
        field.Type = TType.I32;
        field.ID = 18;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Max_compaction_threshold);
        oprot.WriteFieldEnd();
      }
      if (__isset.row_cache_save_period_in_seconds) {
        field.Name = "row_cache_save_period_in_seconds";
        field.Type = TType.I32;
        field.ID = 19;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Row_cache_save_period_in_seconds);
        oprot.WriteFieldEnd();
      }
      if (__isset.key_cache_save_period_in_seconds) {
        field.Name = "key_cache_save_period_in_seconds";
        field.Type = TType.I32;
        field.ID = 20;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Key_cache_save_period_in_seconds);
        oprot.WriteFieldEnd();
      }
      if (__isset.memtable_throughput_in_mb) {
        field.Name = "memtable_throughput_in_mb";
        field.Type = TType.I32;
        field.ID = 22;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Memtable_throughput_in_mb);
        oprot.WriteFieldEnd();
      }
      if (__isset.memtable_operations_in_millions) {
        field.Name = "memtable_operations_in_millions";
        field.Type = TType.Double;
        field.ID = 23;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Memtable_operations_in_millions);
        oprot.WriteFieldEnd();
      }
      if (__isset.replicate_on_write) {
        field.Name = "replicate_on_write";
        field.Type = TType.Bool;
        field.ID = 24;
        oprot.WriteFieldBegin(field);
        oprot.WriteBool(Replicate_on_write);
        oprot.WriteFieldEnd();
      }
      if (__isset.merge_shards_chance) {
        field.Name = "merge_shards_chance";
        field.Type = TType.Double;
        field.ID = 25;
        oprot.WriteFieldBegin(field);
        oprot.WriteDouble(Merge_shards_chance);
        oprot.WriteFieldEnd();
      }
      if (Key_validation_class != null && __isset.key_validation_class) {
        field.Name = "key_validation_class";
        field.Type = TType.String;
        field.ID = 26;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Key_validation_class);
        oprot.WriteFieldEnd();
      }
      if (Row_cache_provider != null && __isset.row_cache_provider) {
        field.Name = "row_cache_provider";
        field.Type = TType.String;
        field.ID = 27;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Row_cache_provider);
        oprot.WriteFieldEnd();
      }
      if (Key_alias != null && __isset.key_alias) {
        field.Name = "key_alias";
        field.Type = TType.String;
        field.ID = 28;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Key_alias);
        oprot.WriteFieldEnd();
      }
      if (Compaction_strategy != null && __isset.compaction_strategy) {
        field.Name = "compaction_strategy";
        field.Type = TType.String;
        field.ID = 29;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Compaction_strategy);
        oprot.WriteFieldEnd();
      }
      if (Compaction_strategy_options != null && __isset.compaction_strategy_options) {
        field.Name = "compaction_strategy_options";
        field.Type = TType.Map;
        field.ID = 30;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteMapBegin(new TMap(TType.String, TType.String, Compaction_strategy_options.Count));
          foreach (string _iter37 in Compaction_strategy_options.Keys)
          {
            oprot.WriteString(_iter37);
            oprot.WriteString(Compaction_strategy_options[_iter37]);
          }
          oprot.WriteMapEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("CfDef(");
      sb.Append("Keyspace: ");
      sb.Append(Keyspace);
      sb.Append(",Name: ");
      sb.Append(Name);
      sb.Append(",Column_type: ");
      sb.Append(Column_type);
      sb.Append(",Comparator_type: ");
      sb.Append(Comparator_type);
      sb.Append(",Subcomparator_type: ");
      sb.Append(Subcomparator_type);
      sb.Append(",Comment: ");
      sb.Append(Comment);
      sb.Append(",Row_cache_size: ");
      sb.Append(Row_cache_size);
      sb.Append(",Key_cache_size: ");
      sb.Append(Key_cache_size);
      sb.Append(",Read_repair_chance: ");
      sb.Append(Read_repair_chance);
      sb.Append(",Column_metadata: ");
      sb.Append(Column_metadata);
      sb.Append(",Gc_grace_seconds: ");
      sb.Append(Gc_grace_seconds);
      sb.Append(",Default_validation_class: ");
      sb.Append(Default_validation_class);
      sb.Append(",Id: ");
      sb.Append(Id);
      sb.Append(",Min_compaction_threshold: ");
      sb.Append(Min_compaction_threshold);
      sb.Append(",Max_compaction_threshold: ");
      sb.Append(Max_compaction_threshold);
      sb.Append(",Row_cache_save_period_in_seconds: ");
      sb.Append(Row_cache_save_period_in_seconds);
      sb.Append(",Key_cache_save_period_in_seconds: ");
      sb.Append(Key_cache_save_period_in_seconds);
      sb.Append(",Memtable_throughput_in_mb: ");
      sb.Append(Memtable_throughput_in_mb);
      sb.Append(",Memtable_operations_in_millions: ");
      sb.Append(Memtable_operations_in_millions);
      sb.Append(",Replicate_on_write: ");
      sb.Append(Replicate_on_write);
      sb.Append(",Merge_shards_chance: ");
      sb.Append(Merge_shards_chance);
      sb.Append(",Key_validation_class: ");
      sb.Append(Key_validation_class);
      sb.Append(",Row_cache_provider: ");
      sb.Append(Row_cache_provider);
      sb.Append(",Key_alias: ");
      sb.Append(Key_alias);
      sb.Append(",Compaction_strategy: ");
      sb.Append(Compaction_strategy);
      sb.Append(",Compaction_strategy_options: ");
      sb.Append(Compaction_strategy_options);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
