using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Asset_soundCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Asset_sound");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Asset_sound> dict = new Dictionary<int, Asset_sound>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Asset_sound> list = new List<Asset_sound>();
		
        
        public void Merge(object o)
        {
            Asset_soundCategory s = o as Asset_soundCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Asset_sound config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Asset_sound Get(int id)
        {
            this.dict.TryGetValue(id, out Asset_sound item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Asset_sound)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Asset_sound)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Asset_sound> GetAll()
        {
            return this.dict;
        }

        public Asset_sound GetOne()
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
	public partial class Asset_sound: ProtoObject, IConfig
	{
		/// <summary>c</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>资源名</summary>
		[ProtoMember(2)]
		public string name { get; set; }
		/// <summary>重叠类型</summary>
		[ProtoMember(3)]
		public int overlap { get; set; }

	}
}
