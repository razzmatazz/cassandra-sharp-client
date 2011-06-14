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
  public partial class TokenRange : TBase
  {
    private string _start_token;
    private string _end_token;
    private List<string> _endpoints;

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

    public List<string> Endpoints
    {
      get
      {
        return _endpoints;
      }
      set
      {
        __isset.endpoints = true;
        this._endpoints = value;
      }
    }


    public Isset __isset;
    [Serializable]
    public struct Isset {
      public bool start_token;
      public bool end_token;
      public bool endpoints;
    }

    public TokenRange() {
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
              Start_token = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.String) {
              End_token = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.List) {
              {
                Endpoints = new List<string>();
                TList _list20 = iprot.ReadListBegin();
                for( int _i21 = 0; _i21 < _list20.Count; ++_i21)
                {
                  string _elem22 = null;
                  _elem22 = iprot.ReadString();
                  Endpoints.Add(_elem22);
                }
                iprot.ReadListEnd();
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
      TStruct struc = new TStruct("TokenRange");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Start_token != null && __isset.start_token) {
        field.Name = "start_token";
        field.Type = TType.String;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(Start_token);
        oprot.WriteFieldEnd();
      }
      if (End_token != null && __isset.end_token) {
        field.Name = "end_token";
        field.Type = TType.String;
        field.ID = 2;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(End_token);
        oprot.WriteFieldEnd();
      }
      if (Endpoints != null && __isset.endpoints) {
        field.Name = "endpoints";
        field.Type = TType.List;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        {
          oprot.WriteListBegin(new TList(TType.String, Endpoints.Count));
          foreach (string _iter23 in Endpoints)
          {
            oprot.WriteString(_iter23);
          }
          oprot.WriteListEnd();
        }
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("TokenRange(");
      sb.Append("Start_token: ");
      sb.Append(Start_token);
      sb.Append(",End_token: ");
      sb.Append(End_token);
      sb.Append(",Endpoints: ");
      sb.Append(Endpoints);
      sb.Append(")");
      return sb.ToString();
    }

  }

}
