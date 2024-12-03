using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Skill_Desc_skill_activeCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Skill_Desc_skill_active");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Skill_Desc_skill_active> dict = new Dictionary<int, Skill_Desc_skill_active>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Skill_Desc_skill_active> list = new List<Skill_Desc_skill_active>();
		
        
        public void Merge(object o)
        {
            Skill_Desc_skill_activeCategory s = o as Skill_Desc_skill_activeCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Skill_Desc_skill_active config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Skill_Desc_skill_active Get(int id)
        {
            this.dict.TryGetValue(id, out Skill_Desc_skill_active item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Skill_Desc_skill_active)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Skill_Desc_skill_active)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Skill_Desc_skill_active> GetAll()
        {
            return this.dict;
        }

        public Skill_Desc_skill_active GetOne()
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
	public partial class Skill_Desc_skill_active: ProtoObject, IConfig
	{
		/// <summary>唯一id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>技能id</summary>
		[ProtoMember(2)]
		public int skill_id { get; set; }
		/// <summary>伤害值</summary>
		[ProtoMember(3)]
		public int damage { get; set; }
		/// <summary>架势削减</summary>
		[ProtoMember(4)]
		public int posture_damage { get; set; }
		/// <summary>CD</summary>
		[ProtoMember(5)]
		public int cd { get; set; }
		/// <summary>触发CD类型</summary>
		[ProtoMember(6)]
		public int cd_trigger_type { get; set; }
		/// <summary>权重</summary>
		[ProtoMember(7)]
		public int weight { get; set; }
		/// <summary>血量百分比上限</summary>
		[ProtoMember(8)]
		public int max_hp_pct { get; set; }
		/// <summary>满足条件后立即触发一次</summary>
		[ProtoMember(9)]
		public int play_immediately { get; set; }
		/// <summary>无打断直接施放</summary>
		[ProtoMember(10)]
		public int play_directly { get; set; }
		/// <summary>打断帧区间</summary>
		[ProtoMember(11)]
		public ProtobufArrayInt interruptions { get; set; }
		/// <summary>位移</summary>
		[ProtoMember(12)]
		public ProtobufArrayInt shift { get; set; }
		/// <summary>目标位移</summary>
		[ProtoMember(13)]
		public ProtobufArrayInt target_shift { get; set; }
		/// <summary>技能类型</summary>
		[ProtoMember(14)]
		public int skill_type { get; set; }
		/// <summary>技能音效</summary>
		[ProtoMember(15)]
		public ProtobufArrayInt sounds { get; set; }
		/// <summary>技能特效</summary>
		[ProtoMember(16)]
		public ProtobufArrayInt effects { get; set; }
		/// <summary>特效参数</summary>
		[ProtoMember(17)]
		public ProtobufArrayInt effects_params { get; set; }
		/// <summary>命中特效</summary>
		[ProtoMember(18)]
		public int hit_effect { get; set; }
		/// <summary>蓄力</summary>
		[ProtoMember(19)]
		public int[] charge { get; set; }
		/// <summary>预警</summary>
		[ProtoMember(20)]
		public int[] warn { get; set; }
		/// <summary>前摇动画</summary>
		[ProtoMember(21)]
		public int[] wind_up { get; set; }
		/// <summary>蓄力动画</summary>
		[ProtoMember(22)]
		public int[] charge_animation { get; set; }
		/// <summary>技能主动画</summary>
		[ProtoMember(23)]
		public int[] animation { get; set; }
		/// <summary>后摇动画</summary>
		[ProtoMember(24)]
		public int[] wind_down { get; set; }
		/// <summary>参数9</summary>
		[ProtoMember(33)]
		public int[] skill_active_params { get; set; }

	}
}
