using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Buff_Desc_buff_typeCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Buff_Desc_buff_type");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Buff_Desc_buff_type> dict = new Dictionary<int, Buff_Desc_buff_type>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Buff_Desc_buff_type> list = new List<Buff_Desc_buff_type>();
		
        
        public void Merge(object o)
        {
            Buff_Desc_buff_typeCategory s = o as Buff_Desc_buff_typeCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Buff_Desc_buff_type config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Buff_Desc_buff_type Get(int id)
        {
            this.dict.TryGetValue(id, out Buff_Desc_buff_type item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Buff_Desc_buff_type)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Buff_Desc_buff_type)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Buff_Desc_buff_type> GetAll()
        {
            return this.dict;
        }

        public Buff_Desc_buff_type GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            var enumerator = this.dict.Values.GetEnumerator();
            enumerator.MoveNext();
            return enumerator.Current; 
        }
    }


    [ProtoContract]
	public partial class Buff_Desc_buff_type: ProtoObject, IConfig
	{
		/// <summary>Buff类型</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>共存类型</summary>
		[ProtoMember(2)]
		public int buff_exist { get; set; }

	}
}
