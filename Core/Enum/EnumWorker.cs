using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Core.Enum
{
    public enum EnumWorker
    {
        [Description("1")]
        AddMessageWorker = 1,
        [Description("2")]
        SendSmsWorker = 2
    }
}
