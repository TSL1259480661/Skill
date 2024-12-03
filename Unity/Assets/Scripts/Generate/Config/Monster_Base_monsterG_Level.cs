using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Monster_Base_monsterG_LevelCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Monster_Base_monsterG_Level");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Monster_Base_monsterG_Level> dict = new Dictionary<int, Monster_Base_monsterG_Level>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Monster_Base_monsterG_Level> list = new List<Monster_Base_monsterG_Level>();
		
        
        public void Merge(object o)
        {
            Monster_Base_monsterG_LevelCategory s = o as Monster_Base_monsterG_LevelCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Monster_Base_monsterG_Level config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Monster_Base_monsterG_Level Get(int id)
        {
            this.dict.TryGetValue(id, out Monster_Base_monsterG_Level item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Monster_Base_monsterG_Level)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Monster_Base_monsterG_Level)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Monster_Base_monsterG_Level> GetAll()
        {
            return this.dict;
        }

        public Monster_Base_monsterG_Level GetOne()
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
	public partial class Monster_Base_monsterG_Level: ProtoObject, IConfig
	{
		/// <summary>关卡id</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>关卡组id</summary>
		[ProtoMember(2)]
		public int groupId { get; set; }
		/// <summary>生命值提升系数（/百分比）</summary>
		[ProtoMember(3)]
		public int hpPrc { get; set; }
		/// <summary>分值提升系数（/百分比）</summary>
		[ProtoMember(4)]
		public int scorePrc { get; set; }

	}
}
