using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Buff_Desc_buffCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Buff_Desc_buff");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Buff_Desc_buff> dict = new Dictionary<int, Buff_Desc_buff>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Buff_Desc_buff> list = new List<Buff_Desc_buff>();
		
        
        public void Merge(object o)
        {
            Buff_Desc_buffCategory s = o as Buff_Desc_buffCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Buff_Desc_buff config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Buff_Desc_buff Get(int id)
        {
            this.dict.TryGetValue(id, out Buff_Desc_buff item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Buff_Desc_buff)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Buff_Desc_buff)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Buff_Desc_buff> GetAll()
        {
            return this.dict;
        }

        public Buff_Desc_buff GetOne()
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
	public partial class Buff_Desc_buff: ProtoObject, IConfig
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
		/// <summary>Buff名称</summary>
		[ProtoMember(4)]
		public string name { get; set; }
		/// <summary>负面状态</summary>
		[ProtoMember(5)]
		public int debuff { get; set; }
		/// <summary>Buff类型</summary>
		[ProtoMember(6)]
		public int buff_type { get; set; }
		/// <summary>Buff图标</summary>
		[ProtoMember(7)]
		public string icon { get; set; }
		/// <summary>Buff特效</summary>
		[ProtoMember(8)]
		public int buff_display { get; set; }
		/// <summary>叠层上限</summary>
		[ProtoMember(9)]
		public int overlie_max { get; set; }
		/// <summary>同类互斥优先级</summary>
		[ProtoMember(10)]
		public int priority { get; set; }
		/// <summary>持续时间ms</summary>
		[ProtoMember(11)]
		public int time { get; set; }
		/// <summary>Buff描述</summary>
		[ProtoMember(12)]
		public string des { get; set; }

	}
}
