using System;
using System.Collections.Generic;
using System.Linq;

namespace UserWebApi.Proxy
{
    // ReSharper disable once InconsistentNaming
    public class serverSideParams
    {
        private const char Separator = '|';

        // ReSharper disable once InconsistentNaming
        public int draw { get; set; }

        // ReSharper disable once InconsistentNaming
        public int start { get; set; }

        // ReSharper disable once InconsistentNaming
        public int length { get; set; }

        // ReSharper disable once InconsistentNaming
        public string searchValue { get; set; }

        // ReSharper disable once InconsistentNaming
        public string searchRegex { get; set; }

        // ReSharper disable once InconsistentNaming
        public string orderColumns { get; set; }

        // ReSharper disable once InconsistentNaming
        public string orderDirs { get; set; }

        // ReSharper disable once InconsistentNaming
        public string columnsDatas { get; set; }

        // ReSharper disable once InconsistentNaming
        public string columnsSearchable { get; set; }

        // ReSharper disable once InconsistentNaming
        public string columnsOrderable { get; set; }

        // ReSharper disable once InconsistentNaming
        public string columnsSearchValue { get; set; }

        // ReSharper disable once InconsistentNaming
        public string columnsSearchRegex { get; set; }

        /// Get rules sorting (numberCol - number of column sorting; typeSorting - asc/desc)
        internal IEnumerable<(int numberCol, string typeSorting)> RulesSorting()
        {
            var numberColumns = orderColumns.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            var ordDirs = orderDirs.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

            return numberColumns.Select((t, i) => (int.Parse(t), ordDirs[i])).ToList();
        }
    }
}
