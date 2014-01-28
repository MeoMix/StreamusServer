using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Streamus.Extensions
{
    //  http://nine69.wordpress.com/2013/02/02/asp-net-mvc-series-high-performance-json-parser-for-asp-net-mvc/
    public static class CollectionExtensions
    {
        public static ExpandoObject AsExpandoObject(this IDictionary<string, object> dictionary)
        {
            var epo = new ExpandoObject();
            var epoDic = epo as IDictionary<string, object>;

            foreach (var item in dictionary)
            {
                bool processed = false;

                if (item.Value is IDictionary<string, object>)
                {
                    epoDic.Add(item.Key, AsExpandoObject((IDictionary<string, object>) item.Value));
                    processed = true;
                }
                else if (item.Value is ICollection)
                {
                    var itemList = new List<object>();
                    foreach (object item2 in (ICollection) item.Value)
                        if (item2 is IDictionary<string, object>)
                            itemList.Add(AsExpandoObject((IDictionary<string, object>) item2));
                        else
                            itemList.Add(AsExpandoObject(new Dictionary<string, object> {{"Unknown", item2}}));

                    if (itemList.Count > 0)
                    {
                        epoDic.Add(item.Key, itemList);
                        processed = true;
                    }
                }

                if (!processed)
                    epoDic.Add(item);
            }

            return epo;
        }
    }
}