using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Role_Initial_role_initialCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Role_Initial_role_initial");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Role_Initial_role_initial> dict = new Dictionary<int, Role_Initial_role_initial>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Role_Initial_role_initial> list = new List<Role_Initial_role_initial>();
		
        
        public void Merge(object o)
        {
            Role_Initial_role_initialCategory s = o as Role_Initial_role_initialCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Role_Initial_role_initial config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Role_Initial_role_initial Get(int id)
        {
            this.dict.TryGetValue(id, out Role_Initial_role_initial item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Role_Initial_role_initial)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Role_Initial_role_initial)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Role_Initial_role_initial> GetAll()
        {
            return this.dict;
        }

        public Role_Initial_role_initial GetOne()
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
	public class Role_Initial_role_initial_Damagevolume : ProtoObject
	{
		[ProtoMember(1)]
		public int[] offset { get; set; } 

		[ProtoMember(2)]
		public int radius { get; set; } 

	}


    [ProtoContract]
	public partial class Role_Initial_role_initial: ProtoObject, IConfig
	{
		/// <summary>ID</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>时装ID</summary>
		[ProtoMember(2)]
		public int appearance_fid { get; set; }
		/// <summary>移动速度</summary>
		[ProtoMember(3)]
		public int speed { get; set; }
		/// <summary>生命值</summary>
		[ProtoMember(4)]
		public int hp { get; set; }
		/// <summary>攻击距离</summary>
		[ProtoMember(5)]
		public int attack_distance { get; set; }
		/// <summary>天生技能06ID</summary>
		[ProtoMember(11)]
		public int[] role_initial_born_skill_ids { get; set; }
		/// <summary>伤害判定体积01.半径</summary>
		[ProtoMember(13)]
		public Role_Initial_role_initial_Damagevolume[] role_initial_damagevolumes { get; set; }

	}
}
