using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Buff_Desc_buff_effectCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Buff_Desc_buff_effect");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Buff_Desc_buff_effect> dict = new Dictionary<int, Buff_Desc_buff_effect>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Buff_Desc_buff_effect> list = new List<Buff_Desc_buff_effect>();
		
        
        public void Merge(object o)
        {
            Buff_Desc_buff_effectCategory s = o as Buff_Desc_buff_effectCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Buff_Desc_buff_effect config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Buff_Desc_buff_effect Get(int id)
        {
            this.dict.TryGetValue(id, out Buff_Desc_buff_effect item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Buff_Desc_buff_effect)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Buff_Desc_buff_effect)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Buff_Desc_buff_effect> GetAll()
        {
            return this.dict;
        }

        public Buff_Desc_buff_effect GetOne()
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
	public partial class Buff_Desc_buff_effect: ProtoObject, IConfig
	{
		/// <summary>唯一id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>BuffID</summary>
		[ProtoMember(2)]
		public int buff_id { get; set; }
		/// <summary>等级</summary>
		[ProtoMember(3)]
		public int grade { get; set; }
		/// <summary>索引</summary>
		[ProtoMember(4)]
		public int index { get; set; }
		/// <summary>周期性间隔</summary>
		[ProtoMember(5)]
		public int interval { get; set; }
		/// <summary>触发条件</summary>
		[ProtoMember(6)]
		public int trigger_type { get; set; }
		/// <summary>触发概率</summary>
		[ProtoMember(7)]
		public int trigger_random { get; set; }
		/// <summary>Buff效果类型</summary>
		[ProtoMember(8)]
		public int type { get; set; }
		/// <summary>效果目标</summary>
		[ProtoMember(9)]
		public int target { get; set; }
		/// <summary>参数10</summary>
		[ProtoMember(19)]
		public int[] buff_effect_params { get; set; }

	}
}
