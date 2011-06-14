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
  public partial class ColumnDef : TBase
  {
    private byte[] _name;
    private string _validation_class;
    private IndexType _index_type;
    private string _index_name;

    public byte[] Name
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

    public string Validation_class
    {
      get
      {
        return _validation_class;
      }
      set
      {
        __isset.validation_class = true;
        this._validation_class = value;
      }
    }

    public IndexType Index_type
    {
      get
      {
        return _index_type;
      }
      set
      {
        __isset.index_type = true;
        this._index_type = value;
      }
    }

    public string Index_name
    {
      get
      {
        return _index_name;
      }
      set
      {
        __isset.index_name = true;
        this._index_name = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool name;
      public bool validation_class;
      public bool index_type;
      public bool index_name;
    }

    public ColumnDef() {
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
              Name = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              Validation_class = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.I32) {
              Index_type = (IndexType)iprot.ReadI32();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              Index_name = iprot.ReadString();
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
      TStruct struc = new TStruct("ColumnDef");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Name != null && __isset.name) {
        field.Name = "name";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Name);
        oprot.WriteFieldEnd();
      }
      if (Validation_class != null && __isset.validation_class) {
        field.Name = "validation_class";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Validation_class);
        oprot.WriteFieldEnd();
      }
      if (__isset.index_type) {
        field.Name = "index_type";
        field.Type = TType.I32;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32((int)Index_type);
        oprot.WriteFieldEnd();
      }
      if (Index_name != null && __isset.index_name) {
        field.Name = "index_name";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Index_name);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("ColumnDef(");
      sb.Append("Name: ");
      sb.Append(Name);
      sb.Append(",Validation_class: ");
      sb.Append(Validation_class);
      sb.Append(",Index_type: ");
      sb.Append(Index_type);
      sb.Append(",Index_name: ");
      sb.Append(Index_name);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
