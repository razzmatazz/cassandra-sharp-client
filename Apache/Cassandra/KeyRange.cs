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
  public partial class KeyRange : TBase
  {
    private byte[] _start_key;
    private byte[] _end_key;
    private string _start_token;
    private string _end_token;
    private int _count;

    public byte[] Start_key
    {
      get
      {
        return _start_key;
      }
      set
      {
        __isset.start_key = true;
        this._start_key = value;
      }
    }

    public byte[] End_key
    {
      get
      {
        return _end_key;
      }
      set
      {
        __isset.end_key = true;
        this._end_key = value;
      }
    }

    public string Start_token
    {
      get
      {
        return _start_token;
      }
      set
      {
        __isset.start_token = true;
        this._start_token = value;
      }
    }

    public string End_token
    {
      get
      {
        return _end_token;
      }
      set
      {
        __isset.end_token = true;
        this._end_token = value;
      }
    }

    public int Count
    {
      get
      {
        return _count;
      }
      set
      {
        __isset.count = true;
        this._count = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool start_key;
      public bool end_key;
      public bool start_token;
      public bool end_token;
      public bool count;
    }

    public KeyRange() {
      this._count = 100;
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
              Start_key = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              End_key = iprot.ReadBinary();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              Start_token = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              End_token = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 5:
            if (field.Type == TType.I32) {
              Count = iprot.ReadI32();
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
      TStruct struc = new TStruct("KeyRange");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Start_key != null && __isset.start_key) {
        field.Name = "start_key";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(Start_key);
        oprot.WriteFieldEnd();
      }
      if (End_key != null && __isset.end_key) {
        field.Name = "end_key";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteBinary(End_key);
        oprot.WriteFieldEnd();
      }
      if (Start_token != null && __isset.start_token) {
        field.Name = "start_token";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Start_token);
        oprot.WriteFieldEnd();
      }
      if (End_token != null && __isset.end_token) {
        field.Name = "end_token";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(End_token);
        oprot.WriteFieldEnd();
      }
      if (__isset.count) {
        field.Name = "count";
        field.Type = TType.I32;
        field.ID = 5;
        oprot.WriteFieldBegin(field);
        oprot.WriteI32(Count);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("KeyRange(");
      sb.Append("Start_key: ");
      sb.Append(Start_key);
      sb.Append(",End_key: ");
      sb.Append(End_key);
      sb.Append(",Start_token: ");
      sb.Append(Start_token);
      sb.Append(",End_token: ");
      sb.Append(End_token);
      sb.Append(",Count: ");
      sb.Append(Count);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
