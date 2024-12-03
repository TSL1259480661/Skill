using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Skill_Upgrade_skill_upgradeCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Skill_Upgrade_skill_upgrade");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Skill_Upgrade_skill_upgrade> dict = new Dictionary<int, Skill_Upgrade_skill_upgrade>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Skill_Upgrade_skill_upgrade> list = new List<Skill_Upgrade_skill_upgrade>();
		
        
        public void Merge(object o)
        {
            Skill_Upgrade_skill_upgradeCategory s = o as Skill_Upgrade_skill_upgradeCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Skill_Upgrade_skill_upgrade config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Skill_Upgrade_skill_upgrade Get(int id)
        {
            this.dict.TryGetValue(id, out Skill_Upgrade_skill_upgrade item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Skill_Upgrade_skill_upgrade)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Skill_Upgrade_skill_upgrade)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Skill_Upgrade_skill_upgrade> GetAll()
        {
            return this.dict;
        }

        public Skill_Upgrade_skill_upgrade GetOne()
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
	public class Skill_Upgrade_skill_upgrade_Attr : ProtoObject
	{
		[ProtoMember(1)]
		public string name { get; set; } 

		[ProtoMember(2)]
		public int type { get; set; } 

		[ProtoMember(3)]
		public int value { get; set; } 

	}
	[ProtoContract]
	public class Skill_Upgrade_skill_upgrade_Consume : ProtoObject
	{
		[ProtoMember(1)]
		public int ftype { get; set; } 

		[ProtoMember(2)]
		public int fid { get; set; } 

		[ProtoMember(3)]
		public int num { get; set; } 

	}
	[ProtoContract]
	public class Skill_Upgrade_skill_upgrade_Effect : ProtoObject
	{
		[ProtoMember(1)]
		public int ftype { get; set; } 

		[ProtoMember(2)]
		public int id1 { get; set; } 

		[ProtoMember(3)]
		public int id2 { get; set; } 

	}


    [ProtoContract]
	public partial class Skill_Upgrade_skill_upgrade: ProtoObject, IConfig
	{
		/// <summary>唯一id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>技能名称</summary>
		[ProtoMember(2)]
		public string skill_name { get; set; }
		/// <summary>技能唯一ID</summary>
		[ProtoMember(3)]
		public int skill_originID { get; set; }
		/// <summary>技能等级</summary>
		[ProtoMember(4)]
		public int level { get; set; }
		/// <summary>参数5</summary>
		[ProtoMember(19)]
		public Skill_Upgrade_skill_upgrade_Attr[] skill_upgrade_attrs { get; set; }
		/// <summary>道具数量2</summary>
		[ProtoMember(25)]
		public Skill_Upgrade_skill_upgrade_Consume[] skill_upgrade_consumes { get; set; }
		/// <summary>提升属性id</summary>
		[ProtoMember(26)]
		public int add_attr_id { get; set; }
		/// <summary>属性值</summary>
		[ProtoMember(27)]
		public int add_attr_value { get; set; }
		/// <summary>效果2id2</summary>
		[ProtoMember(33)]
		public Skill_Upgrade_skill_upgrade_Effect[] skill_upgrade_effects { get; set; }
		/// <summary>提升战力</summary>
		[ProtoMember(34)]
		public int add_power { get; set; }
		/// <summary>升级描述</summary>
		[ProtoMember(35)]
		public string des { get; set; }

	}
}
