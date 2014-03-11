using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Teshe.Common
{
    public static class Helper
    {
        public static M ModifyMap<M, N>(M from, N to)
        {
            foreach (var toPro in to.GetType().GetProperties())
            {
                foreach (var fromPro in from.GetType().GetProperties())
                {
                    if (fromPro.Name == toPro.Name)
                    {
                        fromPro.SetValue(from, toPro.GetValue(to, null), null);
                    }
                }
            }
            return from;
        }
    }
}