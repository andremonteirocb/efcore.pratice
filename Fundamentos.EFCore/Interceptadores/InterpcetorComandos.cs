using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

namespace Fundamentos.EFCore.Interceptadores
{
    public class InterpcetorComandos : DbCommandInterceptor
    {
        private static readonly Regex _tableRegex =
            new Regex(@"(?<tableAlias>FROM +(\[.*\]\.)?(\[.*\]) AS (\[.*\])(?! WIDTH \(NOLOCK\)))",
                RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public override InterceptionResult<DbDataReader> ReaderExecuting(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result)
        {
            UsarNoLock(command);
            return base.ReaderExecuting(command, eventData, result);
        }

        private void UsarNoLock(DbCommand command)
        {
            if (command.CommandText.Contains("NOLOCK"))
                command.CommandText = _tableRegex.Replace(command.CommandText, "${tableAlias} WITH (NOLOCK)");
        }
    }
}
