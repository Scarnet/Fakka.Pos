using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fakka.Pos.Triggers
{
    public class ElementUnselectedTrigger : TriggerAction<VisualElement>
    {
        protected override void Invoke(VisualElement sender)
        {
            sender.BackgroundColor = (Color)App.Current.Resources["PrimaryColor"];
        }
    }
}
