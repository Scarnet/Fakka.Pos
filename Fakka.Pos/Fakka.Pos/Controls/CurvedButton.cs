using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Controls
{
    public class CurvedButton : Button
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == HeightProperty.PropertyName)
                CornerRadius = (int)Height / 2;
        }
    }
}
