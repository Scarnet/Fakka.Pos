using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Models
{
    public abstract class BaseUiModel : BindableBase
    {
        public string Id { get; set; }
    }
}
