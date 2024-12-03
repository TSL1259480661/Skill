using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Monster_Base_monster_modelCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Monster_Base_monster_model");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Monster_Base_monster_model> dict = new Dictionary<int, Monster_Base_monster_model>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Monster_Base_monster_model> list = new List<Monster_Base_monster_model>();
		
        
        public void Merge(object o)
        {
            Monster_Base_monster_modelCategory s = o as Monster_Base_monster_modelCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Monster_Base_monster_model config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Monster_Base_monster_model Get(int id)
        {
            this.dict.TryGetValue(id, out Monster_Base_monster_model item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Monster_Base_monster_model)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Monster_Base_monster_model)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Monster_Base_monster_model> GetAll()
        {
            return this.dict;
        }

        public Monster_Base_monster_model GetOne()
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
	public class Monster_Base_monster_model_Damagevolume : ProtoObject
	{
		[ProtoMember(1)]
		public int[] offset { get; set; } 

		[ProtoMember(2)]
		public int radius { get; set; } 

	}


    [ProtoContract]
	public partial class Monster_Base_monster_model: ProtoObject, IConfig
	{
		/// <summary>造型id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>移动播放速度</summary>
		[ProtoMember(2)]
		public int move_act_speed { get; set; }
		/// <summary>动画类型</summary>
		[ProtoMember(3)]
		public int animation_type { get; set; }
		/// <summary>动画资源路径</summary>
		[ProtoMember(4)]
		public string animation { get; set; }
		/// <summary>缩放</summary>
		[ProtoMember(5)]
		public int scale { get; set; }
		/// <summary>特效位置-头顶</summary>
		[ProtoMember(6)]
		public int[] effect_head { get; set; }
		/// <summary>特效位置-身上</summary>
		[ProtoMember(7)]
		public int[] effect_body { get; set; }
		/// <summary>特效位置-子弹</summary>
		[ProtoMember(8)]
		public int[] effect_bullet { get; set; }
		/// <summary>特效位置-脚底</summary>
		[ProtoMember(9)]
		public int[] effect_floor { get; set; }
		/// <summary>中心点偏移</summary>
		[ProtoMember(10)]
		public int[] center_offset { get; set; }
		/// <summary>伤害判定体积2半径</summary>
		[ProtoMember(14)]
		public Monster_Base_monster_model_Damagevolume[] monster_model_damagevolumes { get; set; }
		/// <summary>死亡特效缩放</summary>
		[ProtoMember(15)]
		public int dying_scale { get; set; }
		/// <summary>死亡特效</summary>
		[ProtoMember(16)]
		public int dying_asset { get; set; }
		/// <summary>尸体特效缩放</summary>
		[ProtoMember(17)]
		public int corpse_scale { get; set; }
		/// <summary>尸体特效</summary>
		[ProtoMember(18)]
		public int corpse_asset { get; set; }
		/// <summary>受击特效缩放</summary>
		[ProtoMember(19)]
		public int hit_scale { get; set; }

	}
}
