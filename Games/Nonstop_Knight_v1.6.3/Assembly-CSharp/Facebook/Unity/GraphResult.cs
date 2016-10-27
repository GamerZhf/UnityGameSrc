namespace Facebook.Unity
{
    using Facebook.MiniJSON;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class GraphResult : ResultBase, IGraphResult, IResult
    {
        [CompilerGenerated]
        private IList<object> <ResultList>k__BackingField;
        [CompilerGenerated]
        private Texture2D <Texture>k__BackingField;

        internal GraphResult(WWW result) : base(result.text, result.error, false)
        {
            this.Init(this.RawResult);
            if (result.error == null)
            {
                this.Texture = result.texture;
            }
        }

        private void Init(string rawResult)
        {
            if (!string.IsNullOrEmpty(rawResult))
            {
                object obj2 = Json.Deserialize(this.RawResult);
                IDictionary<string, object> dictionary = obj2 as IDictionary<string, object>;
                if (dictionary != null)
                {
                    this.ResultDictionary = dictionary;
                }
                else
                {
                    IList<object> list = obj2 as IList<object>;
                    if (list != null)
                    {
                        this.ResultList = list;
                    }
                }
            }
        }

        public IList<object> ResultList
        {
            [CompilerGenerated]
            get
            {
                return this.<ResultList>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<ResultList>k__BackingField = value;
            }
        }

        public Texture2D Texture
        {
            [CompilerGenerated]
            get
            {
                return this.<Texture>k__BackingField;
            }
            [CompilerGenerated]
            private set
            {
                this.<Texture>k__BackingField = value;
            }
        }
    }
}

