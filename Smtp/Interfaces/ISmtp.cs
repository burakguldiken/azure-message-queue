using System;
using System.Collections.Generic;
using System.Text;

namespace Smtp.Interfaces
{
    public interface ISmtp
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="message"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        bool SendEmail(string message, string to);
    }
}
