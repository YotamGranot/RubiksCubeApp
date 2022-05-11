using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RubiksCubeApp
{
    public class MusicRow
    {
        private int ResId;//each music file has id and name
        private string Name;
        public MusicRow(int resId, string name)
        {
            this.ResId = resId;
            this.Name = name;
        }
        public int getResId()
        {
            return ResId;
        }
        public string getName()
        {
            return Name;
        }
        public void setResId(int resId)
        {
            this.ResId = resId;
        }
        public void setName(string name)
        {
            this.Name = name;
        }
    }
}