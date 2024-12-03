using App;
using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;


namespace ClientData
{
    [ProtoContract]
    public partial class Asset_pictureCategory : ProtoObject, IMerge
    {
        private static UDebugger debugger = new UDebugger("Asset_picture");
		
        [ProtoIgnore,BsonIgnore]
        private Dictionary<int, Asset_picture> dict = new Dictionary<int, Asset_picture>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<Asset_picture> list = new List<Asset_picture>();
		
        
        public void Merge(object o)
        {
            Asset_pictureCategory s = o as Asset_pictureCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (Asset_picture config in list)
            {
                config.EndInit();
                this.dict.Add(config.id, config);
            }            
            this.AfterEndInit();
        }
		
        public Asset_picture Get(int id)
        {
            this.dict.TryGetValue(id, out Asset_picture item);

            if (item == null)
            {
                //throw new Exception($"配置找不到，配置表名: {nameof (Asset_picture)}，配置id: {id}");
				debugger.LogError($"配置找不到，配置表名: {nameof (Asset_picture)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, Asset_picture> GetAll()
        {
            return this.dict;
        }

        public Asset_picture GetOne()
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
	public partial class Asset_picture: ProtoObject, IConfig
	{
		/// <summary>图标ID</summary>
		[ProtoMember(1)]
		public int id { get; set; }
		/// <summary>资源名</summary>
		[ProtoMember(2)]
		public string name { get; set; }
		/// <summary>备注</summary>
		[ProtoMember(3)]
		public string 备注 { get; set; }

	}
}
