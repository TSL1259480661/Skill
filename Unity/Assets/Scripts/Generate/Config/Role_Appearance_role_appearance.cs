using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Role_Appearance_role_appearanceCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Role_Appearance_role_appearance");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Role_Appearance_role_appearance> dict = new Dictionary<int, Role_Appearance_role_appearance>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Role_Appearance_role_appearance> list = new List<Role_Appearance_role_appearance>();
		
        
        public void Merge(object o)
        {
            Role_Appearance_role_appearanceCategory s = o as Role_Appearance_role_appearanceCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Role_Appearance_role_appearance config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Role_Appearance_role_appearance Get(int id)
        {
            this.dict.TryGetValue(id, out Role_Appearance_role_appearance item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Role_Appearance_role_appearance)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Role_Appearance_role_appearance)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Role_Appearance_role_appearance> GetAll()
        {
            return this.dict;
        }

        public Role_Appearance_role_appearance GetOne()
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
	public partial class Role_Appearance_role_appearance: ProtoObject, IConfig
	{
		/// <summary>时装ID</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>动画类型</summary>
		[ProtoMember(2)]
		public int type { get; set; }
		/// <summary>资源路径</summary>
		[ProtoMember(3)]
		public string main_asset_path { get; set; }
		/// <summary>缩放系数</summary>
		[ProtoMember(4)]
		public int scale { get; set; }
		/// <summary>中心偏移</summary>
		[ProtoMember(5)]
		public int[] center { get; set; }
		/// <summary>特效位置-头顶</summary>
		[ProtoMember(6)]
		public int[] effect_head { get; set; }
		/// <summary>特效位置-身上</summary>
		[ProtoMember(7)]
		public int[] effect_body { get; set; }
		/// <summary>特效位置-外围</summary>
		[ProtoMember(8)]
		public int[] effect_outside { get; set; }
		/// <summary>特效位置-脚底</summary>
		[ProtoMember(9)]
		public int[] effect_floor { get; set; }
		/// <summary>受击特效缩放</summary>
		[ProtoMember(10)]
		public int hit_scale { get; set; }
		/// <summary>受击特效</summary>
		[ProtoMember(11)]
		public int hit_asset { get; set; }
		/// <summary>移动播放速度</summary>
		[ProtoMember(12)]
		public int move_act_speed { get; set; }

	}
}
