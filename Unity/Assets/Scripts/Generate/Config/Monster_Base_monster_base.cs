using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Monster_Base_monster_baseCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Monster_Base_monster_base");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Monster_Base_monster_base> dict = new Dictionary<int, Monster_Base_monster_base>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Monster_Base_monster_base> list = new List<Monster_Base_monster_base>();
		
        
        public void Merge(object o)
        {
            Monster_Base_monster_baseCategory s = o as Monster_Base_monster_baseCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Monster_Base_monster_base config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Monster_Base_monster_base Get(int id)
        {
            this.dict.TryGetValue(id, out Monster_Base_monster_base item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Monster_Base_monster_base)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Monster_Base_monster_base)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Monster_Base_monster_base> GetAll()
        {
            return this.dict;
        }

        public Monster_Base_monster_base GetOne()
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
	public partial class Monster_Base_monster_base: ProtoObject, IConfig
	{
		/// <summary>怪物id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>怪物名称</summary>
		[ProtoMember(2)]
		public string name { get; set; }
		/// <summary>怪物类型</summary>
		[ProtoMember(3)]
		public int type { get; set; }
		/// <summary>生命值</summary>
		[ProtoMember(4)]
		public int hp { get; set; }
		/// <summary>架势值</summary>
		[ProtoMember(5)]
		public int posture { get; set; }
		/// <summary>击杀得分</summary>
		[ProtoMember(6)]
		public int score { get; set; }
		/// <summary>移动速度</summary>
		[ProtoMember(7)]
		public int speed { get; set; }
		/// <summary>技能4</summary>
		[ProtoMember(11)]
		public int[] monster_base_skill_ids { get; set; }
		/// <summary>造型id</summary>
		[ProtoMember(12)]
		public int model_id { get; set; }

	}
}
