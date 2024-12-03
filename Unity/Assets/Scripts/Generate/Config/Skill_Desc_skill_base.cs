using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Skill_Desc_skill_baseCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Skill_Desc_skill_base");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Skill_Desc_skill_base> dict = new Dictionary<int, Skill_Desc_skill_base>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Skill_Desc_skill_base> list = new List<Skill_Desc_skill_base>();
		
        
        public void Merge(object o)
        {
            Skill_Desc_skill_baseCategory s = o as Skill_Desc_skill_baseCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Skill_Desc_skill_base config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Skill_Desc_skill_base Get(int id)
        {
            this.dict.TryGetValue(id, out Skill_Desc_skill_base item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Skill_Desc_skill_base)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Skill_Desc_skill_base)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Skill_Desc_skill_base> GetAll()
        {
            return this.dict;
        }

        public Skill_Desc_skill_base GetOne()
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
	public partial class Skill_Desc_skill_base: ProtoObject, IConfig
	{
		/// <summary>唯一id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>技能ID</summary>
		[ProtoMember(2)]
		public int skill_id { get; set; }
		/// <summary>父技能ID</summary>
		[ProtoMember(3)]
		public int parentid { get; set; }
		/// <summary>技能名称</summary>
		[ProtoMember(4)]
		public string name { get; set; }
		/// <summary>技能类型</summary>
		[ProtoMember(5)]
		public int skill_type { get; set; }
		/// <summary>技能图标</summary>
		[ProtoMember(6)]
		public string icon { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(7)]
		public string des { get; set; }

	}
}
