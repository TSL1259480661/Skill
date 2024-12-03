using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Skill_Desc_skill_passiveCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Skill_Desc_skill_passive");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Skill_Desc_skill_passive> dict = new Dictionary<int, Skill_Desc_skill_passive>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Skill_Desc_skill_passive> list = new List<Skill_Desc_skill_passive>();
		
        
        public void Merge(object o)
        {
            Skill_Desc_skill_passiveCategory s = o as Skill_Desc_skill_passiveCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Skill_Desc_skill_passive config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Skill_Desc_skill_passive Get(int id)
        {
            this.dict.TryGetValue(id, out Skill_Desc_skill_passive item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Skill_Desc_skill_passive)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Skill_Desc_skill_passive)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Skill_Desc_skill_passive> GetAll()
        {
            return this.dict;
        }

        public Skill_Desc_skill_passive GetOne()
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
	public partial class Skill_Desc_skill_passive: ProtoObject, IConfig
	{
		/// <summary>唯一id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>技能id</summary>
		[ProtoMember(2)]
		public int skill_id { get; set; }
		/// <summary>技能等级</summary>
		[ProtoMember(3)]
		public int grade { get; set; }
		/// <summary>参数索引值</summary>
		[ProtoMember(4)]
		public int index { get; set; }
		/// <summary>触发技能ID限制</summary>
		[ProtoMember(5)]
		public int trigger_skill { get; set; }
		/// <summary>触发技能限制</summary>
		[ProtoMember(6)]
		public int is_child_trigger { get; set; }
		/// <summary>触发条件</summary>
		[ProtoMember(7)]
		public int trigger_type { get; set; }
		/// <summary>触发条件参数</summary>
		[ProtoMember(8)]
		public int[] trigger_params { get; set; }
		/// <summary>触发概率</summary>
		[ProtoMember(9)]
		public int trigger_random { get; set; }
		/// <summary>无视CD</summary>
		[ProtoMember(10)]
		public int not_cd { get; set; }
		/// <summary>效果目标</summary>
		[ProtoMember(11)]
		public int target { get; set; }
		/// <summary>被动效果</summary>
		[ProtoMember(12)]
		public int skill_type { get; set; }
		/// <summary>参数10</summary>
		[ProtoMember(22)]
		public int[] skill_passive_params { get; set; }

	}
}
