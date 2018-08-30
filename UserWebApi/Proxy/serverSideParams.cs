using System;
using System.Linq;
using UserWebApi.Models;

namespace UserWebApi.Proxy
{
    // ReSharper disable once InconsistentNaming
    public class serverSideParams
    {
        private const char Separator = '|';

        private const string AscSort = "asc";

        private const string DescSort = "desc";

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

        /// Sorting by elements in (orderColumns, orderDirs)
        /// <param name="users">Query get users</param>
        /// <returns>Query with applied rules of sorting</returns>
        internal IQueryable<User> SortingByRules(IQueryable<User> users)
        {
            var numberColumns = orderColumns.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
            var ordDirs = orderDirs.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < numberColumns.Length; i++)
            {
                var index = int.Parse(numberColumns[i]);

                switch (index)
                {
                    case 0:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.Id);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.Id);
                        break;
                    case 1:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.Login);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.Login);
                        break;
                    case 2:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.Password);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.Password);
                        break;
                    case 3:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.LastName);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.LastName);
                        break;
                    case 4:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.FirstName);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.FirstName);
                        break;
                    case 5:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.MiddleName);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.MiddleName);
                        break;
                    case 6:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.Telephone);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.Telephone);
                        break;
                    case 7:
                        if (ordDirs[i].Equals(AscSort))
                            users = users.OrderBy(usr => usr.IsAdmin);
                        else if (ordDirs[i].Equals(DescSort))
                            users = users.OrderByDescending(usr => usr.IsAdmin);
                        break;
                    // ReSharper disable once RedundantEmptySwitchSection
                    default:
                        break;
                }
            }

            return users;
        }
    }
}
